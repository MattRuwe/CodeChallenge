Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports MvcArchStarter.Core.Ioc
Imports MvcArchStarter.Core.Persistence

<DependencyDiscovery(GetType(IUnitOfWork), IocLifetime.Hierarchical)> _
Class MvcStarterArchContextUnitOfWork
    Implements IUnitOfWork
    Private _context As DbContext
    Private ReadOnly _factory As IContextFactory

    Public Sub New(factory As IContextFactory)
        _factory = factory
    End Sub

    Public Function RepositoryOf(Of TEntity As Class)() As IRepository(Of TEntity) Implements IUnitOfWork.RepositoryOf
        EnsureDbContext()
        Dim [set] = _context.[Set](Of TEntity)()
        Return New DbSetRepository(Of TEntity)([set])
    End Function

    Public Sub SaveChanges() Implements IUnitOfWork.SaveChanges
        If _context IsNot Nothing Then
            _context.SaveChanges()
        End If
    End Sub

    Public Sub RejectChanges() Implements IUnitOfWork.RejectChanges
        If _context IsNot Nothing Then
            For Each entry In _context.ChangeTracker.Entries()
                If entry.State = EntityState.Modified Then
                    entry.State = EntityState.Unchanged
                ElseIf entry.State = EntityState.Added Then
                    entry.State = EntityState.Detached
                End If
            Next
        End If
    End Sub

    Public Property ProxyCreationEnabled() As Boolean Implements IUnitOfWork.ProxyCreationEnabled
        Get
            Return _context.Configuration.ProxyCreationEnabled
        End Get
        Set(value As Boolean)
            _context.Configuration.ProxyCreationEnabled = value
        End Set
    End Property

    Public Property LazyLoadingEnabled() As Boolean Implements IUnitOfWork.LazyLoadingEnabled
        Get
            Return _context.Configuration.LazyLoadingEnabled
        End Get
        Set(value As Boolean)
            _context.Configuration.LazyLoadingEnabled = value
        End Set
    End Property

    Public Property AutoDetectChangesEnabled() As Boolean Implements IUnitOfWork.AutoDetectChangesEnabled
        Get
            Return _context.Configuration.AutoDetectChangesEnabled
        End Get
        Set(value As Boolean)
            _context.Configuration.AutoDetectChangesEnabled = value
        End Set
    End Property

    Public Property ValidateOnSaveEnabled() As Boolean Implements IUnitOfWork.ValidateOnSaveEnabled
        Get
            Return _context.Configuration.ValidateOnSaveEnabled
        End Get
        Set(value As Boolean)
            _context.Configuration.ValidateOnSaveEnabled = value
        End Set
    End Property

    Public Sub Dispose() Implements IDisposable.Dispose
        If _context IsNot Nothing Then
            _context.Dispose()
        End If
    End Sub

    Private Sub EnsureDbContext()
        If _context Is Nothing Then
            _context = _factory.Create()
            _context.Configuration.ProxyCreationEnabled = True
            _context.Configuration.LazyLoadingEnabled = True
            _context.Configuration.AutoDetectChangesEnabled = True
            _context.Configuration.ValidateOnSaveEnabled = True
        End If

    End Sub
End Class
