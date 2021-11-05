Imports Unity.Mvc4
Imports Microsoft.Practices.Unity

Public Class WebUIDependencyResolver
    Implements IDependencyResolver

    Private _resolver As IDependencyResolver

    Public Sub New(container As IUnityContainer)
        _resolver = New UnityDependencyResolver(container)
    End Sub

    Public Function GetService(serviceType As Type) As Object Implements IDependencyResolver.GetService
        Dim returnValue As Object = _resolver.GetService(serviceType)

        Return returnValue
    End Function

    Public Function GetServices(serviceType As Type) As IEnumerable(Of Object) Implements IDependencyResolver.GetServices
        Return _resolver.GetServices(serviceType)
    End Function

End Class
