Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Server

Public Class UserChallengeStatistics

    Public Sub New()
        ID = Guid.NewGuid
    End Sub

    <Key()>
    Public Property ID As Guid

    Public Property ChallengeID As Integer

    Private _username As String
    Public Property Username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Private _scores As List(Of UserScore)

    <Include()>
    <Association("UserChallengeStatistics_Scores", "ID", "StatID")>
    Public ReadOnly Property Scores() As List(Of UserScore)
        Get
            If _scores Is Nothing Then
                _scores = New List(Of UserScore)
            End If

            Return _scores
        End Get
    End Property

End Class

