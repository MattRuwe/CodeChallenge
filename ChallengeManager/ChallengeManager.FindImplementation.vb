Imports System.Reflection
Imports System.IO


<Serializable()>
Partial Public Class ChallengeManager
    Public Function GetAllChallengeTypeStrings(ByVal assemblyBytes() As Byte) As List(Of ImplementationChallenge) 'GetAllChallengeTypeStringsResult
        Dim returnValue As List(Of ImplementationChallenge)

        Dim loadingDomain As AppDomain = Nothing
        Try
            Assembly.LoadFrom(Assembly.GetExecutingAssembly.Location)
            loadingDomain = CreateLoadingAppdomain()

            loadingDomain.SetData("assemblyBytes", assemblyBytes)

            loadingDomain.DoCallBack(AddressOf GetAllChallengeTypeStrings)

            returnValue = loadingDomain.GetData("returnValue")
        Finally
            If loadingDomain IsNot Nothing Then
                AppDomain.Unload(loadingDomain)
            End If
        End Try

        Return returnValue
    End Function

    Private Sub GetAllChallengeTypeStrings()
        Dim returnValue As New List(Of ImplementationChallenge)
        Dim assemblyBytes() As Byte = AppDomain.CurrentDomain.GetData("assemblyBytes")

        AddHandler AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf ReflectionOnlyAssemblyResolve

        Dim amb As Assembly = Assembly.ReflectionOnlyLoad(assemblyBytes)

        For Each ambType As Type In amb.GetTypes()
            If ambType.GetInterface(GetType(Global.OmahaMTG.Challenge.ChallengeCommon.IChallenge).FullName) IsNot Nothing Then
                Dim interfaces As Type() = ambType.GetInterfaces()


                If interfaces IsNot Nothing AndAlso interfaces.Count = 2 Then
                    Dim result As New ImplementationChallenge With {
                        .ImplementationTypeString = ambType.AssemblyQualifiedName,
                        .ChallengeInterfaceTypeString = interfaces(0).AssemblyQualifiedName}
                    returnValue.Add(result)
                End If
            End If
        Next

        RemoveHandler AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf ReflectionOnlyAssemblyResolve

        AppDomain.CurrentDomain.SetData("returnValue", returnValue)
    End Sub
End Class
