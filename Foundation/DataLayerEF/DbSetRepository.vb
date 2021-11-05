Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Text

Imports MvcArchStarter.Core.Persistence

Class DbSetRepository(Of TEntity As Class)
    Implements IRepository(Of TEntity)
    Private ReadOnly dbSet As IDbSet(Of TEntity)
    Private ReadOnly includePaths As List(Of String)

    Public Sub New(dbSet As IDbSet(Of TEntity))
        Me.dbSet = dbSet
        includePaths = New List(Of String)()
    End Sub

    Public Function FindAll() As IQueryable(Of TEntity) Implements IRepository(Of TEntity).FindAll
        Return Me.dbSet
    End Function

    Public Function FindWhere(predicate As Expression(Of Func(Of TEntity, Boolean))) As IQueryable(Of TEntity) Implements IRepository(Of TEntity).FindWhere
        If includePaths IsNot Nothing AndAlso includePaths.Count > 0 Then
            'IQueryable<TEntity> include = dbSet.Include(includePaths[0]);
            'for (int i = 1; i < includePaths.Count; i++)
            '{
            '    include = include.Include(includePaths[i]);
            '}

            Dim include As IQueryable(Of TEntity) = dbSet

            includePaths.ForEach(Function(i) InlineAssignHelper(include, include.Include(i)))

            includePaths.Clear()

            Return include.Where(predicate)
        Else
            Return Me.dbSet.Where(predicate)
        End If
    End Function

    Public Function Include(pathExpression As Expression(Of Func(Of TEntity, Object))) As IRepository(Of TEntity) Implements IRepository(Of TEntity).Include
        Dim path As String = pathExpression.GetExpressionPath()
        Return Include(path)
    End Function

    Public Function Include(path As String) As IRepository(Of TEntity) Implements IRepository(Of TEntity).Include
        includePaths.Add(path)
        Return Me
    End Function

    Public Sub Add(entity As TEntity) Implements IRepository(Of TEntity).Add
        Me.dbSet.Add(entity)
    End Sub

    Public Sub Remove(entity As TEntity) Implements IRepository(Of TEntity).Remove
        Me.dbSet.Remove(entity)
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
End Class
