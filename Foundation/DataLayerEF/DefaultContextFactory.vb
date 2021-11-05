Imports MvcArchStarter.Core.Ioc
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Text

Imports MvcArchStarter.Core.Persistence

<DependencyDiscovery(GetType(IContextFactory), IocLifetime.Singleton)> _
    Public Class DefaultContextFactory
    Implements IContextFactory
    Public Sub New()
    End Sub

    Public Function Create() As System.Data.Entity.DbContext Implements IContextFactory.Create
        'Database.SetInitializer(New TestDataInitializer())
        Return New CodeChallengeEntities()
    End Function
End Class