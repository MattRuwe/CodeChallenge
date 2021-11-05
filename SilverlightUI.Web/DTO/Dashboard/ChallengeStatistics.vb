Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Server

Public Class ChallengeStatistics
    <Key()>
    Public Property ChallengeID As Integer
    Public Property ChallengeName As String
    Public Property PublishedEntryCount As Integer
    Public Property UnpublishedEntryCount As Integer
    Public Property PendingEntryCount As Integer
    Public Property QueuedEntryCount As Integer
    Public Property InitializingEntryCount As Integer
    Public Property RunningEntryCount As Integer
    Public Property FinishedEntryCount As Integer
    Public Property AveragePublishedScore As Long
    Public Property AverageUnpublishedScore As Long
    Public Property AverageScore As Long
    Public Property MaxPublishedScore As Long
    Public Property MaxUnpublishedScore As Long
    Public Property MaxScore As Long


    Private _userStatistics As List(Of UserChallengeStatistics)
    <Include()>
    <Association("ChallengeStatistics_UserStatistics", "ChallengeID", "ChallengeID")>
    Public ReadOnly Property UserStatistics() As List(Of UserChallengeStatistics)
        Get
            If _userStatistics Is Nothing Then
                _userStatistics = New List(Of UserChallengeStatistics)
            End If

            Return _userStatistics
        End Get
    End Property
End Class
