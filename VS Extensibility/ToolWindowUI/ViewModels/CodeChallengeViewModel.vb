Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService

Namespace ViewModels

    Public Class CodeChallengeViewModel
    Inherits ViewModelBase

        Public Sub New()

        End Sub

        Private _loginCommand As ICommand = New SimpleDelegateCommand(Sub(param As Object)
                                                                          If (ValidateLogin(Username, Password)) Then
                                                                              IsLoggedIn = True
                                                                              RefreshChallenges()
                                                                          End If
                                                                      End Sub)

        Public ReadOnly Property LoginCommand() As ICommand
            Get
                Return _loginCommand
            End Get
        End Property

        Private _logoffCommand As ICommand = New SimpleDelegateCommand(Sub(param As Object)
                                                                           Logoff()
                                                                       End Sub)

        Public ReadOnly Property LogoffCommand() As ICommand
            Get
                Return _logoffCommand
            End Get
        End Property

        Private _username As String
        Public Property Username() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                If value <> _username Then
                    _username = value
                    OnPropertyChanged("Username")
                End If
            End Set
        End Property

        Private _password As String
        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                If value <> _password Then
                    _password = value
                    OnPropertyChanged("Password")
                End If
            End Set
        End Property


    End Class

End Namespace
