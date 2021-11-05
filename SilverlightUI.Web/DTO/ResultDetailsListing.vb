Imports System.ComponentModel.DataAnnotations
Imports System

Public Class ResultDetailsListing
    <Key()>
    Public Property ID As Integer
    Public Property EntryID As Integer
    Public Property AuthorNote As String
    Public Property Duration As Integer
    Public Property [Error] As String
    Public Property ResultMessage As String
    Public Property Score As Long
    Public Property Successful As Boolean
    Public Property CpuCycles As Nullable(Of Decimal)
    Public Property TestDataAvailable As Boolean
End Class
