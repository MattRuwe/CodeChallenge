Imports System.Data.Entity

Public Interface IContextFactory
    Function Create() As DbContext
End Interface
