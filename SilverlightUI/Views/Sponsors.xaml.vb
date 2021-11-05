Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' <see cref="Page"/> class to present information about the current application.
''' </summary>
Partial Public Class Sponsors
    Inherits Page

    ''' <summary>
    ''' Creates a new instance of the <see cref="About"/> class.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.SponsorsPageTitle
    End Sub

    ''' <summary>
    ''' Executes when the user navigates to this page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub

    Private Sub btnOk_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnOk.Click
        SponsorDomainDataSource.SubmitChanges()
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        SponsorDomainDataSource.RejectChanges()
    End Sub

    Private Sub hlbAdd_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbAdd.Click
        Dim newSponsor As New CodeChallenge_Sponsor
        SponsorDomainDataSource.DataView.Add(newSponsor)
        lstSponsors.SelectedItem = newSponsor
    End Sub

    Private Sub hlbDelete_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbDelete.Click
        SponsorDomainDataSource.DataView.Remove(lstSponsors.SelectedItem)
        SponsorDomainDataSource.SubmitChanges()
    End Sub

    Private Sub SponsorDomainDataSource_LoadedData(sender As Object, e As System.Windows.Controls.LoadedDataEventArgs) Handles SponsorDomainDataSource.LoadedData
        If e.HasError Then
            MessageBox.Show(e.Error.ToString)
            e.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub SponsorDomainDataSource_SubmittedChanges(sender As Object, e As System.Windows.Controls.SubmittedChangesEventArgs) Handles SponsorDomainDataSource.SubmittedChanges
        If e.HasError Then
            MessageBox.Show(e.Error.ToString)
            e.MarkErrorAsHandled()
        End If
    End Sub
End Class