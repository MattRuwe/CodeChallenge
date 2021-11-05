Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.ServiceModel.DomainServices.Client
Imports System.IO

Partial Public Class CodeChallenges
    Inherits Page

    Private WithEvents authService As AuthenticationService = WebContext.Current.Authentication

    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.CodeChallengesPageTitle
    End Sub

    'Executes when the user navigates to this page.
    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)
        UpdateLoginState()
    End Sub

    Private Sub hlbAdd_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbAdd.Click
        Dim newChallengeForm As New NewChallengeForm(Nothing)
        AddHandler newChallengeForm.Closed,
            Sub(sender2 As Object, e2 As EventArgs)
                If newChallengeForm.NewChallenge IsNot Nothing Then
                    Dim ctx As CodeChallengeDomainContext = CodeChallengeDataSource.DomainContext

                    ctx.CodeChallenges.Add(newChallengeForm.NewChallenge)
                    ctx.SubmitChanges(Sub(so As SubmitOperation)
                                          If Not so.HasError Then
                                              CodeChallengeDataSource.Load()
                                          Else
                                              MessageBox.Show("Could not save challenge:  " & so.Error.ToString)
                                              so.MarkErrorAsHandled()
                                          End If
                                      End Sub, Nothing)
                End If
            End Sub
        newChallengeForm.Show()
    End Sub

    Private Sub hlbEdit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbEdit.Click
        Dim newChallengeForm As New NewChallengeForm(CodeChallengeDataSource)

        AddHandler newChallengeForm.Closed,
            Sub(sender2 As Object, e2 As EventArgs)
                If newChallengeForm.NewChallenge IsNot Nothing Then
                    CodeChallengeDataSource.DomainContext.SubmitChanges(
                        Sub(so As SubmitOperation)
                            CodeChallengeDataSource.Load()
                        End Sub, Nothing)
                Else
                    CodeChallengeDataSource.RejectChanges()
                End If
            End Sub

        Dim challengeId As Integer = CType(lstChallenges.SelectedItem, vw_codechallenge_secure).id

        Dim ctx As CodeChallengeDomainContext = CodeChallengeDataSource.DomainContext
        Dim query As EntityQuery(Of CodeChallenge) = ctx.GetCodeChallengesQuery().Where(Function(c) c.id = challengeId)
        Dim lo As LoadOperation(Of CodeChallenge) = ctx.Load(Of CodeChallenge)(query)

        AddHandler lo.Completed,
            Sub(sender2 As Object, e2 As EventArgs)
                If lo.Entities.Count > 0 Then
                    newChallengeForm.NewChallenge = lo.Entities().First
                    newChallengeForm.Show()
                End If
            End Sub
    End Sub

    Private Sub hlbDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbDelete.Click
        CodeChallengeDataSource.DataView.Remove(lstChallenges.SelectedItem)
        CodeChallengeDataSource.SubmitChanges()
    End Sub

    Private Sub Authentication_LoggedIn(ByVal sender As Object, ByVal e As AuthenticationEventArgs) Handles authService.LoggedIn
        Me.UpdateLoginState()
    End Sub

    Private Sub Authentication_LoggedOut(ByVal sender As Object, ByVal e As AuthenticationEventArgs) Handles authService.LoggedOut
        Me.UpdateLoginState()
    End Sub

    Private Sub UpdateLoginState()
        If WebContext.Current.User.IsAuthenticated Then
            If WebContext.Current.Authentication.User.IsInRole("CodeChallengeAdmin") Then
                VisualStateManager.GoToState(Me, "loggedInAdmin", True)
            Else
                VisualStateManager.GoToState(Me, "loggedInNormal", True)
            End If
        Else
            VisualStateManager.GoToState(Me, "loggedOut", True)
        End If
    End Sub

    Private Sub CodeChallengeDataSource_LoadedData(sender As System.Object, e As System.Windows.Controls.LoadedDataEventArgs)
        If e.HasError Then
            MessageBox.Show(e.Error.Message)
            e.MarkErrorAsHandled()
        End If
    End Sub


    Private Sub CodeChallengeDataSource_LoadingData(sender As Object, e As System.Windows.Controls.LoadingDataEventArgs) Handles CodeChallengeDataSource.LoadingData
        e.LoadBehavior = LoadBehavior.RefreshCurrent
    End Sub
End Class

