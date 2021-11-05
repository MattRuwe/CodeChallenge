Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports System.ServiceModel.DomainServices.Client
Imports System.IO.IsolatedStorage

''' <summary>
''' <see cref="Page"/> class to present information about the current application.
''' </summary>
Partial Public Class Results
    Inherits Page

    Private _ctx As CodeChallengeDomainContext

    ''' <summary>
    ''' Creates a new instance of the <see cref="About"/> class.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.ResultsPageTitle
        _ctx = New CodeChallengeDomainContext

        Dim adminVisibility As Visibility = If(Application.Current.Resources("AuthWrapper").IsAdmin, Visibility.Visible, Visibility.Collapsed)

        dgResults.Columns(dgResults.Columns.Count - 1).Visibility = adminVisibility
        dgResults.Columns(dgResults.Columns.Count - 2).Visibility = adminVisibility
        dgResults.Columns(dgResults.Columns.Count - 3).Visibility = adminVisibility

    End Sub

    ''' <summary>
    ''' Executes when the user navigates to this page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub

    Private Sub lstChallenges_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstChallenges.SelectionChanged
        Try
            If IsolatedStorageSettings.ApplicationSettings.Contains("UserResultChallengeID") Then
                IsolatedStorageSettings.ApplicationSettings("UserResultChallengeID") = CType(lstChallenges.SelectedItem, CodeChallengeListing).id
            Else
                IsolatedStorageSettings.ApplicationSettings.Add("UserResultChallengeID", CType(lstChallenges.SelectedItem, CodeChallengeListing).id)
            End If
            IsolatedStorageSettings.ApplicationSettings.Save()
        Catch
        End Try
    End Sub

    Private Sub hlbRefresh_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbRefresh.Click
        ResultsDomainDataSource.Load()
    End Sub

    Private Sub CodeChallengeDomainDataSource_LoadedData(ByVal sender As System.Object, ByVal e As System.Windows.Controls.LoadedDataEventArgs) Handles CodeChallengeDomainDataSource.LoadedData
        If e.HasError Then
            System.Windows.MessageBox.Show(e.Error.ToString, "Load challenges error", System.Windows.MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        Else
            Try
                If IsolatedStorageSettings.ApplicationSettings.Contains("UserResultChallengeID") Then
                    Dim id As Integer = IsolatedStorageSettings.ApplicationSettings("UserResultChallengeID")

                    If id > 0 Then
                        For Each entry As CodeChallengeListing In lstChallenges.Items
                            If entry.id = id Then
                                lstChallenges.SelectedItem = entry
                            End If
                        Next
                    End If
                End If
            Catch
            End Try
        End If
    End Sub

    Private Sub ResultsDomainDataSource_LoadedData(sender As Object, e As System.Windows.Controls.LoadedDataEventArgs) Handles ResultsDomainDataSource.LoadedData
        If e.HasError Then
            MessageBox.Show(e.Error.ToString, "Load results error", MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub ExecutionDetails_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        tbExecutionDetails.Text = CType(sender, FrameworkElement).Tag

        popExecutionDetails.IsOpen = True
    End Sub

    Private Sub Rerun_Click(sender As Object, e As RoutedEventArgs)
        _ctx.RerunEntry(CType(CType(sender, FrameworkElement).Tag, ResultsListing).ID,
                        Sub(io As InvokeOperation)
                            ResultsDomainDataSource.Load()
                        End Sub,
                        Nothing)
    End Sub

    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)
        _ctx.PurgeEntry(CType(CType(sender, FrameworkElement).Tag, ResultsListing).ID,
                        Sub(io As InvokeOperation)
                            ResultsDomainDataSource.Load()
                        End Sub,
                        Nothing)
    End Sub

    Private Sub popExecutionDetails_SizeChanged(sender As System.Object, e As System.Windows.SizeChangedEventArgs)
        gdExecutionDetails.Height = Me.ActualHeight
        gdExecutionDetails.Width = Me.ActualWidth
    End Sub

    Private Sub btnClosePopup_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        tbExecutionDetails.Text = ""
        popExecutionDetails.IsOpen = False
    End Sub

    Private Sub ResultsDomainDataSource_LoadingData(sender As Object, e As System.Windows.Controls.LoadingDataEventArgs) Handles ResultsDomainDataSource.LoadingData
        e.LoadBehavior = LoadBehavior.RefreshCurrent
    End Sub
End Class