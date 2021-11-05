Imports System.IO
Imports System.Reflection
Imports OmahaMTG.Challenge.ChallengeCommon

Public Class ChallengeManager
    Public Sub New(ByVal challengeImplementationType As Type)

        If challengeImplementationType.GetInterface("IChallenge") Is Nothing Then
            Throw New InvalidOperationException(String.Format("The type {0} does not derive from IChallenge", challengeImplementationType.ToString))
        End If

        Dim executors As List(Of Type) = GetClassesImplementingInterfaceOrBaseType(GetType(ChallengeExecutorBase(Of )), "C:\Development\OmahaMTG2\Development\Tools\CodeChallenge\Challenges", True)

        'Loop through each executor to find the appropriate one to execute our challenge
        For Each executorType As Type In executors
            Dim implementationInterfaceTypes As Type() = executorType.BaseType.GetGenericArguments()

            For Each implementationInterfaceType As Type In implementationInterfaceTypes
                Dim challengeImplementationInterfaces As Type() = challengeImplementationType.GetInterfaces
                For Each challengeImplementationInterface As Type In challengeImplementationInterfaces
                    If implementationInterfaceType.FullName = challengeImplementationInterface.FullName Then
                        Dim implementationInstance = Activator.CreateInstance(challengeImplementationType)
                        Dim executor = Activator.CreateInstance(executorType, New Object() {implementationInstance})
                        executor.RunChallenge()
                    End If
                Next
            Next
        Next
    End Sub

    Private Function GetClassesImplementingInterfaceOrBaseType(ByVal interfaceOrBaseType As Type, ByVal rootPath As String, ByVal recursive As Boolean) As List(Of Type)
        Dim returnValue As New List(Of Type)

        If Not Directory.Exists(rootPath) Then
            Throw New FileNotFoundException(String.Format("The directory at path {0} could not be found", rootPath))
        End If

        Dim files As String() = Directory.GetFiles(rootPath, "*.dll")
        For Each file As String In files
            Dim iterationTypes As New List(Of Type)

            Dim theAssembly As Assembly = Nothing

            Try
                theAssembly = Assembly.LoadFrom(file)
            Catch ex As Exception
                Continue For
            End Try


            Try
                Dim assemblyTypes As Type() = theAssembly.GetTypes

                For Each assemblyType In assemblyTypes
                    If assemblyType.GetInterface(interfaceOrBaseType.FullName) IsNot Nothing Then
                        iterationTypes.Add(assemblyType)
                    ElseIf assemblyType.BaseType Is interfaceOrBaseType Then
                        iterationTypes.Add(assemblyType)
                    ElseIf assemblyType.BaseType IsNot Nothing AndAlso assemblyType.BaseType.IsGenericType AndAlso assemblyType.BaseType.GetGenericTypeDefinition Is interfaceOrBaseType Then
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
End Class
