Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Objects.DataClasses
Imports System.ServiceModel.DomainServices.Server

Public Class ResultsListing

    <Key()>
    Public Property ID As Integer

    Public Property Position As Integer
    Public Property AssemblyFullName As String

    Public Property CodeChallengeID As Integer
    Public Property CodeChallengeName As String

    Public Property DateAdded As DateTime
    Public Property DateRan As Nullable(Of DateTime)
    Public Property FinalScore As Nullable(Of Long)
    Public Property TotalExecutionTime As Nullable(Of Integer)
    Public Property ExecutionDetails As String
    Public Property Username As String
    Public Property UserID As Guid
    Public Property IsPublished As Boolean
    Public Property Status As String
    Public Property IsHidden As Boolean?

    <Include()>
    <Association("ResultsListing_Details", "ID", "EntryID")>
    Public Property Results As IEnumerable(Of ResultDetailsListing)
End Class
