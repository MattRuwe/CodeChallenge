Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.ComponentModel
Imports System.Threading

Public Class AuthWrapper
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private _authService As AuthenticationService
    Private _isInitialized As Boolean = False
    Private _timer As Timer

    Private Sub Initialize()
        If Not _isInitialized Then
            _authService = WebContext.Current.Authentication
            If _authService IsNot Nothing Then
                AddHandler _authService.LoggedIn, AddressOf Auth_LoggedIn
                AddHandler _authService.LoggedOut, AddressOf Auth_LoggedOut
            End If
            _isInitialized = True
            _timer = New Timer(AddressOf TimerElapased, Nothing, 60000, 60000)
        End If
    End Sub

    Private _isLoggedIn As Boolean
    Public ReadOnly Property IsLoggedIn As Boolean
        Get
            Initialize()
            If _isAdmin <> WebContext.Current.User.IsAuthenticated Then
                _isLoggedIn = WebContext.Current.User.IsAuthenticated
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsAdmin"))
            End If

            Return _isLoggedIn
        End Get
    End Property

    Private _isAdmin As Boolean
    Public ReadOnly Property IsAdmin As Boolean
        Get
            Initialize()
            If _isAdmin <> WebContext.Current.Authentication.User.IsInRole("CodeChallengeAdmin") Then
                _isAdmin = WebContext.Current.Authentication.User.IsInRole("CodeChallengeAdmin")
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsAdmin"))
            End If

            Return _isAdmin
        End Get
    End Property

    Private Sub Auth_LoggedIn(ByVal sender As Object, ByVal e As AuthenticationEventArgs)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsLoggedIn"))
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsAdmin"))
    End Sub

    Private Sub Auth_LoggedOut(ByVal sender As Object, ByVal e As AuthenticationEventArgs)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsLoggedIn"))
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsAdmin"))
    End Sub

    Private Sub TimerElapased(ByVal state As Object)
        Dim isLoggedIn As Boolean = Me.IsLoggedIn
        Dim isAdmin As Boolean = Me.IsAdmin
    End Sub


End Class
