'Imports System.Reflection
'Imports System.IO

'Partial Public Class ChallengeManager
'    Private _solutionRoot As String

'    Public Sub New(solutionRoot As String)
'        _solutionRoot = solutionRoot
'    End Sub



'    Public Function CreateLoadingAppdomain() As AppDomain
'        Dim appdomainsetup As New AppDomainSetup

'        'If HostingEnvironment.IsHosted Then
'        '    appdomainsetup.PrivateBinPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "bin")
'        '    appdomainsetup.ApplicationBase = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "bin")
'        'Else
'        '    appdomainsetup.PrivateBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location)
'        '    appdomainsetup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location)
'        'End If

'        appdomainsetup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location)
'        appdomainsetup.PrivateBinPath = appdomainsetup.ApplicationBase

'        Dim returnValue As AppDomain = AppDomain.CreateDomain("LoadingDomain", Nothing, appdomainsetup)

'        'AddHandler returnValue.AssemblyResolve, AddressOf MainDomainAssemblyResolve

'        'returnValue.Load(File.ReadAllBytes(Assembly.GetExecutingAssembly.Location))


'        Return returnValue
'    End Function

'    Private Function MainDomainAssemblyResolve(sender As Object, args As ResolveEventArgs) As Assembly
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' Preloads assemblies in the reflection only context
'    ''' </summary>
'    ''' <param name="sender"></param>
'    ''' <param name="args"></param>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Private Function ReflectionOnlyAssemblyResolve(ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly
'        Dim name As New AssemblyName(args.Name)

'        Dim solutionRootDirectory As String = Path.GetDirectoryName(_solutionRoot)
'        Dim potentialAssemblyPath = Path.Combine(solutionRootDirectory, name.Name & ".dll")
'        If File.Exists(potentialAssemblyPath) Then
'            Return Assembly.ReflectionOnlyLoadFrom(potentialAssemblyPath)
'        Else
'            Return Nothing
'        End If


'        'Dim currentAssemblyPath As String = Assembly.GetExecutingAssembly.Location

'        'Dim name As New AssemblyName(args.Name)

'        'Dim assemblyToLoadPath As String = Path.Combine(Path.GetDirectoryName(currentAssemblyPath), name.Name & ".dll")

'        'If File.Exists(assemblyToLoadPath) Then
'        '    Return Assembly.ReflectionOnlyLoadFrom(assemblyToLoadPath)
'        'Else
'        '    Dim db As New CodeChallengeModel(_connString)
'        '    Dim result = (From dev In db.CodeChallenge_DeveloperAssembly Where dev.assembly_fullname.ToLower = name.FullName.ToLower Select dev.assembly).FirstOrDefault()

'        '    If result IsNot Nothing AndAlso result.Length > 0 Then
'        '        Return Assembly.ReflectionOnlyLoad(result)
'        '    Else
'        '        result = (From assem In db.CodeChallenge_Assembly Where assem.AssemblyFullName.ToLower = name.FullName.ToLower Select assem.assembly).FirstOrDefault()
'        '        If result IsNot Nothing Then
'        '            Return Assembly.ReflectionOnlyLoad(result)
'        '        Else
'        '            Return Nothing
'        '        End If
'        '    End If

'        'End If
'    End Function
'End Class
