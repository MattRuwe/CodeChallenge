Imports System.ComponentModel.DataAnnotations

Public Class EntryListing
    <Key()>
    Public Property EntryID As Integer
    Public Property AuthorUsername As String
    Public Property ChallengeName As String
    Public Property FinalScore As Long?
    Public Property DateAdded As System.DateTime
    Public Property IsPublished As Boolean
    Public Property IsChallengeHidden As Boolean?
End Class
