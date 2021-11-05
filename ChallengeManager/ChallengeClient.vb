Imports System.IO
Imports System.Reflection
Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.Manager
Imports System.Security.Policy
Imports System.Security
Imports System.Security.Permissions
Imports System.Runtime.Remoting
Imports System.Threading
Imports System.Text.RegularExpressions
Imports OmahaMTG.Challenge.ExecutionCommon
Imports System.Net
Imports System.Configuration

Public Class ChallengeClient
    Inherits MarshalByRefObject

    Private _executorType As Type
    Private _challengeImplementationType As Type
    Private _untrustedPath As String
    Private _challengeInterfaceType As Type
    Private _connectionString As String
    Private _isTest As Boolean

    Public Sub New(ByVal challengeImplementationType As Type, isTest As Boolean)
        _connectionString = ConfigurationManager.ConnectionStrings("CodeChallengeModelChallengeManager").ConnectionString
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf CurrentDomain_AssemblyResolve
        If challengeImplementationType.GetInterface("IChallenge") Is Nothing Then
            Throw New InvalidOperationException(String.Format("The type {0} does not derive from IChallenge", challengeImplementationType.ToString))
        End If

        _challengeImplementationType = challengeImplementationType
        _isTest = isTest

        InitializeFromType()
    End Sub

    Private Sub New()

    End Sub

    Private Sub InitializeFromType()
        _untrustedPath = Path.GetDirectoryName(_challengeImplementationType.Assembly.Location)

        'Find all available executors
        Console.WriteLine("Locating executor...")
        Dim executors As List(Of Type) = GetClassesImplementingInterfaceOrBaseType(GetType(ChallengeExecutorBase(Of )), _untrustedPath, False)

        'Loop through each executor to find the appropriate one to execute our challenge
        Console.WriteLine("Looping through each executor found")
        For Each executorType As Type In executors
            'Get the generic type of the executor to determine which interface it should be used to execute against
            Dim implementationInterfaceTypes As Type() = executorType.BaseType.GetGenericArguments()

            'Find the interfaces on the generic type that implement IChallenge
            For Each implementationInterfaceType As Type In implementationInterfaceTypes
                If implementationInterfaceType.GetInterfaces().Length > 0 AndAlso implementationInterfaceType.GetInterfaces(0) Is GetType(IChallenge) Then

                    'Find the executor for the specific Challenge
                    Dim challengeImplementationInterfaces As Type() = _challengeImplementationType.GetInterfaces
                    For Each challengeImplementationInterface As Type In challengeImplementationInterfaces

                        'Does the executor's interface match the challenge's interface
                        If implementationInterfaceType.FullName = challengeImplementationInterface.FullName Then
                            _challengeInterfaceType = implementationInterfaceType
                            _executorType = executorType
                            Console.WriteLine("Matching executor has been found!")
                            Console.WriteLine("    {0}", _executorType.ToString)
                        End If

                    Next

                End If
            Next
        Next
        Console.WriteLine("Finished looping through each executor found")
    End Sub

    Public Function ExecuteChallenge() As List(Of ChallengeResult)
        Console.WriteLine("ExecuteChallenge")
        Dim untrustedAssemblyLocation As String = _challengeImplementationType.Assembly.Location
        Dim adSetup As New AppDomainSetup
        adSetup.ApplicationBase = Path.GetDirectoryName(untrustedAssemblyLocation)

        'Setup the permission for the app domain
        Dim permSet As New PermissionSet(PermissionState.None)
        permSet.AddPermission(New SecurityPermission(SecurityPermissionFlag.Execution Or SecurityPermissionFlag.ControlThread))



        'Setup strong names that can be trusted
        Dim fullTrustAssembly As New List(Of StrongName)

        Dim strongName As StrongName = GetType(ExecutionCommon.ChallengeExecutorBase(Of )).Assembly.Evidence.GetHostEvidence(Of StrongName)()
        If strongName Is Nothing Then
            Console.WriteLine("The ExecutionCommon assembly is not strong named!")
        End If
        Console.WriteLine("Adding strong name to full trust assemblies: {0}", strongName.ToString())
        fullTrustAssembly.Add(strongName)

        strongName = _executorType.Assembly.Evidence.GetHostEvidence(Of StrongName)()
        If strongName Is Nothing Then
            Console.WriteLine("The executor assembly is not strong named!")
        End If
        Console.WriteLine("Adding strong name to full trust assemblies: {0}", strongName.ToString())
        fullTrustAssembly.Add(strongName)

        strongName = GetType(ChallengeClient).Assembly.Evidence.GetHostEvidence(Of StrongName)()
        If strongName Is Nothing Then
            Console.WriteLine("The challenge client assembly is not strong named!")
        End If
        Console.WriteLine("Adding strong name to full trust assemblies: {0}", strongName.ToString())
        fullTrustAssembly.Add(strongName)

        'Create the app domain that will be used to execute the untrusted code
        Console.WriteLine("Creating sandbox")
        Dim newDomain As AppDomain = AppDomain.CreateDomain("Sandbox", Nothing, adSetup, permSet, fullTrustAssembly.ToArray())
        Console.WriteLine("--Sandbox created")
        Console.WriteLine("Setting data on new app domain")
        newDomain.SetData("IsTest", _isTest)
        Console.WriteLine("--Data has been set")

        Console.WriteLine("Creating an instance of the ChallengeClient object in the new app domain")
        'Create a new instance of the ChallengeClient type that will be hosted inside of the untrusted app domain
        Dim handle As ObjectHandle = Activator.CreateInstanceFrom(newDomain,
                                                                  GetType(ChallengeClient).Assembly.ManifestModule.FullyQualifiedName,
                                                                  GetType(ChallengeClient).FullName,
                                                                  True,
                                                                  BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.CreateInstance,
                                                                  Nothing,
                                                                  New Object() {},
                                                                  Thread.CurrentThread.CurrentCulture,
                                                                  Nothing)

        'Get the new instance of the ChallengeClient so the ExecuteUntrustedCode can be executed
        Dim newDomainInstance As ChallengeClient = CType(handle.Unwrap, ChallengeClient)

        Console.WriteLine("--New ChallengeClient object created")

        'Get the implementation type and qualify it with the assembly file name (strip off the strong name tokens etc.)
        Dim implementationAssemblyQualifiedName As String = _challengeImplementationType.AssemblyQualifiedName
        implementationAssemblyQualifiedName = Regex.Replace(implementationAssemblyQualifiedName, "(?<=^[^,]+,\s)[^,]+", Path.GetFileNameWithoutExtension(untrustedAssemblyLocation))

        Console.WriteLine("Running code in sandboxed app domain")
        Console.WriteLine("--Implementation Assembly: {0}", implementationAssemblyQualifiedName)
        Console.WriteLine("--Executor Type:           {0}", _executorType.AssemblyQualifiedName)
        Console.WriteLine("--Is Test:                 {0}", _isTest)

        'Start the untrusted code execution
        Dim returnValue As List(Of ChallengeResult) = newDomainInstance.ExecuteUntrustedCode(implementationAssemblyQualifiedName, _executorType.AssemblyQualifiedName, _isTest)

        AppDomain.Unload(newDomain)

        Return returnValue

    End Function

    ''' <summary>
    ''' This method is used to JIT compile assemblies at runtime
    ''' </summary>
    ''' <param name="assm"></param>
    ''' <remarks></remarks>
    Private Sub PrepareAssembly(assm As Assembly)
        Dim sw As Stopwatch = Stopwatch.StartNew()
        Console.WriteLine("Preparing assembly: {0}", assm.FullName)
        Dim typeCount As Integer = 0
        Dim methodCount As Integer = 0
        Dim failedMethodCount As Integer = 0
        For Each type As Type In assm.GetTypes()
            typeCount += 1
            For Each method As MethodInfo In type.GetMethods(BindingFlags.DeclaredOnly Or BindingFlags.NonPublic Or BindingFlags.[Public] Or BindingFlags.Instance Or BindingFlags.[Static])
                If Not method.IsAbstract AndAlso Not method.ContainsGenericParameters Then
                    Try
                        System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle)
                        Console.WriteLine("Successfully prepared method: {0}.{1}", method.DeclaringType.FullName, method.Name)
                        methodCount += 1
                    Catch ex As Exception
                        Console.WriteLine("Failed to prepare the method: {0}.{1}", method.DeclaringType.FullName, method.Name)
                        Console.WriteLine(ex.ToString())
                        failedMethodCount += 1
                    End Try
                End If
            Next
        Next

        Console.WriteLine("Finished preparing {0} methods in {1} types in {2} milliseconds.", methodCount, typeCount, sw.ElapsedMilliseconds)
        Console.WriteLine("{0} methods could not be prepared.", failedMethodCount)
    End Sub

    <SecuritySafeCritical()>
    Private Function ExecuteUntrustedCode(ByVal implementationTypeString As String, ByVal executorTypeString As String, isTest As Boolean) As List(Of ChallengeResult)
        If implementationTypeString Is Nothing Then
            Throw New ArgumentNullException("implementationTypeString")
        End If

        If executorTypeString Is Nothing Then
            Throw New ArgumentNullException("executorTypeString")
        End If

        Dim implementationType As Type = Type.GetType(implementationTypeString)
        If implementationType Is Nothing Then
            Throw New InvalidOperationException(String.Format("The implementation type '{0}' could not be found", implementationTypeString))
        End If

        _isTest = isTest

        'This only works because this assembly has it's strong name key in the fully trusted assemblies list
        'Create an instance of the implementation type
        Dim implementation As Object = Activator.CreateInstance(implementationType)

        Dim executorType As Type = Type.GetType(executorTypeString)
        If executorType Is Nothing Then
            Throw New InvalidOperationException(String.Format("The executor type '{0}' could not be found", executorTypeString))
        End If

        Dim permSet As New PermissionSet(PermissionState.Unrestricted)
        permSet.Assert()

        'Pre JIT compile the executor and implementation types
        PrepareAssembly(executorType.Assembly)
        PrepareAssembly(implementationType.Assembly)

        'Create a new instance of the executor
        Dim executor As Object = Activator.CreateInstance(executorType, New Object() {})
        Dim isTestProperty As PropertyInfo = executor.GetType().GetProperty("IsTest", BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.NonPublic)

        If isTestProperty IsNot Nothing Then
            Console.WriteLine("IsTest property is available.  Setting the value to '{0}'", _isTest.ToString())
            isTestProperty.SetValue(executor, _isTest, New Object() {})
        Else
            Console.WriteLine("IsTest property is NOT available, ignoring IsTest argument of '{0}'.", _isTest.ToString())
        End If
        PermissionSet.RevertAssert()

        Console.WriteLine("Executing entry code")
        'Run the executor giving it the instance of the implementation code
        Dim returnValue As List(Of ChallengeResult) = executor.RunChallenge(implementation)
        Console.WriteLine("Finished executing entry code")

        Return returnValue
    End Function

    Private Function GetClassesImplementingInterfaceOrBaseType(ByVal interfaceOrBaseType As Type, ByVal rootPath As String, ByVal recursive As Boolean) As List(Of Type)
        Console.WriteLine("Searching for classes that implement or inherit from ""{0}""", interfaceOrBaseType.ToString())

        If recursive Then
            Console.WriteLine("Currently searching recursively through path {0}", rootPath)
        Else
            Console.WriteLine("Currently searching only path {0}", rootPath)
        End If

        Dim returnValue As New List(Of Type)

        If Not Directory.Exists(rootPath) Then
            Throw New FileNotFoundException(String.Format("The directory at path {0} could not be found", rootPath))
        End If

        Dim files As String() = Directory.GetFiles(rootPath, "*.dll")
        For Each file As String In files
            Console.WriteLine("Now searching assembly ""{0}""", Path.GetFileName(file))
            Dim iterationTypes As New List(Of Type)

            Dim theAssembly As Assembly = Nothing

            Try
                theAssembly = Assembly.LoadFile(file)
            Catch ex As Exception
                Continue For
            End Try


            Try
                Dim assemblyTypes As Type() = theAssembly.GetTypes

                For Each assemblyType In assemblyTypes
                    If assemblyType.GetInterface(interfaceOrBaseType.FullName) IsNot Nothing Then
                        Console.WriteLine("Interface {0} matched", assemblyType.ToString())
                        iterationTypes.Add(assemblyType)
                    ElseIf assemblyType.BaseType Is interfaceOrBaseType Then
                        Console.WriteLine("Base type {0} matched", assemblyType.ToString())
                        iterationTypes.Add(assemblyType)
                    ElseIf assemblyType.BaseType IsNot Nothing AndAlso assemblyType.BaseType.IsGenericType AndAlso assemblyType.BaseType.GetGenericTypeDefinition Is interfaceOrBaseType Then
                        Console.WriteLine("Generic base type {0} matched", assemblyType.ToString())
                        iterationTypes.Add(assemblyType)
                    End If
                Next
            Catch ex As Exception

            End Try

            returnValue.AddRange(iterationTypes)
        Next

        If recursive Then
            Dim directories As String() = Directory.GetDirectories(rootPath)
            For Each directory As String In directories
                returnValue.AddRange(GetClassesImplementingInterfaceOrBaseType(interfaceOrBaseType, directory, True))
            Next
        End If

        returnValue = returnValue.Distinct(New TypeComparer).ToList

        Return returnValue
    End Function

    Private Function CurrentDomain_AssemblyResolve(sender As Object, args As ResolveEventArgs) As Assembly
        Console.WriteLine("Attempting to resolve assembly {0}", args.Name)
        Dim returnValue As Assembly = Nothing
        Dim name As New AssemblyName(args.Name)
        Dim assemblyToLoadPath As String = Path.Combine(_untrustedPath, name.Name & ".dll")

        If File.Exists(assemblyToLoadPath) Then
            returnValue = Assembly.LoadFrom(assemblyToLoadPath)
        Else
            Console.WriteLine("Establishing connection to database ('{0}')", _connectionString)
            Dim db As New CodeChallengeModel(_connectionString)

            Console.WriteLine("Attempting to resolve assembly from the database")
            Dim result = (From assm In db.CodeChallenge_Assembly Where assm.AssemblyFullName.ToLower = args.Name.ToLower).FirstOrDefault

            If result IsNot Nothing Then
                Console.WriteLine("  The assembly was located and is about to be loaded into the AppDomain")
                returnValue = Assembly.Load(result.assembly)
            Else
                Console.WriteLine("  The assembly was not found in the database")
            End If
        End If

        Return returnValue
    End Function



End Class
