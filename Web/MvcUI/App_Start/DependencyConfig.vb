Imports Microsoft.Practices.Unity
Imports Microsoft.Practices.Unity.InterceptionExtension
Imports MvcArchStarter.Core.Ioc
Imports Unity.Mvc4

Public Class DependencyConfig
    Public Shared Sub RegisterDependencyInjection()
        Dim container = BuildUnityContainer(New FindTypesInConfiguration())
        DependencyResolver.SetResolver(New WebUIDependencyResolver(container))
    End Sub

    Private Shared Function BuildUnityContainer(typeFinder As ITypeFinder) As IUnityContainer
        Dim container = New UnityContainer
        container.AddNewExtension(Of Interception)()
        Dim discovery As New DependencyDiscovery(container, typeFinder, Nothing)
        discovery.LoadContainer()

        Return container
    End Function
End Class
