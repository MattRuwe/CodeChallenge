Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports System.ServiceModel.DomainServices.Client
Imports System.ComponentModel
Imports System.IO.IsolatedStorage

''' <summary>
''' Home page for the application.
''' </summary>
Partial Public Class Home
    Inherits Page

    Private _ctx As New CodeChallengeDomainContext
    Private _authWrapper As AuthWrapper

    ''' <summary>
    ''' Creates a new <see cref="Home"/> instance.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
        _authWrapper = Application.Current.Resources("AuthWrapper")
        AddHandler _authWrapper.PropertyChanged, AddressOf AuthWrapper_PropertyChanged

        hlbAnnouncementsRss.NavigateUri = New Uri(New Uri(Application.Current.Host.Source.AbsoluteUri), "../AnnouncementsRss.aspx")
    End Sub

    ''' <summary>
    ''' Executes when the user navigates to this page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
        UpdateLoginState()
    End Sub


    Private Sub hlbAddEntry_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbAddEntry.Click
        Dim newEntry As New NewEntryForm()
        AddHandler newEntry.Closed,
            Sub(sender2 As Object, e2 As EventArgs)
                If newEntry.NewEntry IsNot Nothing Then
                    Dim domaindatasource = Me.Resources("DomainContext")

                    Dim dc As CodeChallengeDomainContext = CType(domaindatasource, CodeChallengeDomainContext)
                    dc.CodeChallenge_Entries.Add(newEntry.NewEntry)
                    dc.SubmitChanges(Sub(so As SubmitOperation)
                                         If so.HasError Then
                                             MessageBox.Show(so.Error.Message)
                                             so.MarkErrorAsHandled()
                                         Else
                                             ddsLatestEntries.Load()
                                             'CodeChallenge_EntryDomainDataSource.Load()
                                         End If
                                     End Sub, Nothing)
                End If
            End Sub

        newEntry.Show()
    End Sub

    'Private Sub CodeChallenge_EntryDomainDataSource_LoadedData(ByVal sender As System.Object, ByVal e As System.Windows.Controls.LoadedDataEventArgs) Handles CodeChallenge_EntryDomainDataSource.LoadedData

    '    If e.HasError Then
    '        System.Windows.MessageBox.Show(e.Error.ToString, "Load Error", System.Windows.MessageBoxButton.OK)
    '        e.MarkErrorAsHandled()
    '    End If

    'End Sub

    'Private Sub CodeChallenge_EntryDomainDataSource_SubmittedChanges(ByVal sender As Object, ByVal e As System.Windows.Controls.SubmittedChangesEventArgs) Handles CodeChallenge_EntryDomainDataSource.SubmittedChanges
    '    If e.HasError Then
    '        System.Windows.MessageBox.Show(e.Error.ToString, "Save Error", System.Windows.MessageBoxButton.OK)
    '        e.MarkErrorAsHandled()
    '    End If
    'End Sub

    Private Sub AuthWrapper_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName = "IsLoggedIn" Then
            If _authWrapper.IsLoggedIn Then
                'CodeChallenge_EntryDomainDataSource.Load()
            End If
            UpdateLoginState()
        End If
    End Sub

    Private Sub UpdateLoginState()
        If _authWrapper.IsLoggedIn Then
            If _authWrapper.IsAdmin Then
                VisualStateManager.GoToState(Me, "loggedInAdmin", True)
            Else
                VisualStateManager.GoToState(Me, "loggedInNormal", True)
            End If
        Else
            VisualStateManager.GoToState(Me, "loggedOut", True)
        End If
    End Sub


    Private Sub hlbRefresh_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbRefreshLatestEntries.Click
        If ddsLatestEntries.CanLoad Then
            ddsLatestEntries.Load()
        End If
    End Sub

    Private Sub hlbRefreshLeadboard_Click(sender As Object, e As RoutedEventArgs) Handles hlbRefreshLeadboard.Click
        If ddsEntries.CanLoad Then
            ddsEntries.Load()
        End If
    End Sub

    Private Sub ddsLatestEntries_LoadingData(sender As Object, e As System.Windows.Controls.LoadingDataEventArgs) Handles ddsLatestEntries.LoadingData
        e.LoadBehavior = LoadBehavior.RefreshCurrent
    End Sub

    Private Sub ddlEntries_LoadingData(sender As Object, e As LoadingDataEventArgs) Handles ddsEntries.LoadingData
        e.LoadBehavior = LoadBehavior.RefreshCurrent
    End Sub

    Private Shared _animationRan As Boolean = False

    Private Sub Home_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If Not _animationRan Then
            AnnouncementsTranslate.TranslateX = AnnouncementsBorder.ActualWidth + (AnnouncementsBorder.ActualWidth * 0.5)
            AnnouncementsStartingKeyFrame.Value = AnnouncementsBorder.ActualWidth + (AnnouncementsBorder.ActualWidth * 0.5)
            AddHandler Startup.Completed,
                Sub(sender2 As Object, e2 As EventArgs)
                    _animationRan = True
                End Sub
            Startup.Begin()
        Else
            TitleBorderTransform.ScaleX = 1
            TitleBorderTransform.ScaleY = 1

            LatestEntriesBorderTransform.TranslateX = 0

            AnnouncementsTranslate.TranslateX = 0
        End If
    End Sub

    Private Sub LatestEntriesBorder_SizeChanged(sender As Object, e As System.Windows.SizeChangedEventArgs) Handles LatestEntriesBorder.SizeChanged
        LeaderboardBorder.Height = LatestEntriesBorder.ActualHeight
        LeaderboardScrollView.Height = LatestEntriesBorder.ActualHeight * 0.84
        AnnouncementsBorder.Height = LatestEntriesBorder.ActualHeight
    End Sub

    Private Sub cbCodeChallenges_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cbCodeChallenges.SelectionChanged
        Try
            If IsolatedStorageSettings.ApplicationSettings.Contains("UserPreferredChallengeID") Then
                IsolatedStorageSettings.ApplicationSettings("UserPreferredChallengeID") = CType(cbCodeChallenges.SelectedItem, CodeChallengeListing).id
            Else
                IsolatedStorageSettings.ApplicationSettings.Add("UserPreferredChallengeID", CType(cbCodeChallenges.SelectedItem, CodeChallengeListing).id)
            End If
            IsolatedStorageSettings.ApplicationSettings.Save()
        Catch
        End Try
    End Sub

    Private Sub ddsCodeChallenges_LoadedData(sender As Object, e As System.Windows.Controls.LoadedDataEventArgs) Handles ddsCodeChallenges.LoadedData
        Try
            If IsolatedStorageSettings.ApplicationSettings.Contains("UserPreferredChallengeID") Then
                Dim id As Integer = IsolatedStorageSettings.ApplicationSettings("UserPreferredChallengeID")

                If id > 0 Then
                    For Each entry As CodeChallengeListing In cbCodeChallenges.Items
                        If entry.id = id Then
                            cbCodeChallenges.SelectedItem = entry
                        End If
                    Next
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub btnShowChallengeStats_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim chartView As New ChallengeStats(cbCodeChallenges.SelectedItem.ID)
        chartView.Height = App.Current.Host.Content.ActualHeight * 0.97
        chartView.Width = App.Current.Host.Content.ActualWidth * 0.97
        chartView.Show()
    End Sub

    Private Sub hlCreateAChallenge_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim videoPlayer As New VideoPlayer()
        'videoPlayer.Height = ActualHeight * 0.9
        'videoPlayer.Width = ActualWidth * 0.9
        videoPlayer.Show()
    End Sub
End Class