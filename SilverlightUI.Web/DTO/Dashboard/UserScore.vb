Imports System
Imports System.ComponentModel.DataAnnotations

Public Class UserScore

    Public Sub New()
        Me.ID = Guid.NewGuid
    End Sub

    Public Sub New(statId As Guid)
        Me.New()
        Me.StatID = statId
    End Sub

    <Key()>
    Public Property ID As Guid

    Public Property StatID As Guid

    Private _dateAdded As DateTime
    Public Property DateAdded() As DateTime
        Get
            Return _dateAdded
        End Get
        Set(ByVal value As DateTime)
            _dateAdded = value
        End Set
    End Property

    Private _score As Long
    Public Property Score() As Long
        Get
            Return _score
        End Get
        Set(ByVal value As Long)
            _score = value
        End Set
    End Property
End Class
