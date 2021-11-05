Imports System.ComponentModel.DataAnnotations

Public Class CodeChallengeListing

    Public Sub New()

    End Sub

    <Key()>
    Property id As Integer
    Property ChallengeName As String
    Property SponsorName As String
    Property IsHidden As Boolean?
End Class
