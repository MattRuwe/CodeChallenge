Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.AuthenticationService

Public Class LoginContext
    Private Sub New()

    End Sub

    Private Shared _objectLock As New Object
    Private Shared _instance As LoginContext
    Public Shared ReadOnly Property Instance As LoginContext
        Get
            If _instance Is Nothing Then
                SyncLock _objectLock
                    If _instance Is Nothing Then
                        _instance = New LoginContext
                    End If
                End SyncLock
            End If

            Return _instance
        End Get
    End Property

    Private _loggedInUser As User
    Public Property LoggedInUser() As User
        Get
            Return _loggedInUser
        End Get
        Set(ByVal value As User)
            _loggedInUser = value
        End Set
    End Property

End Class
