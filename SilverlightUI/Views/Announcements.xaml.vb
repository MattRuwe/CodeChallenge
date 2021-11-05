Imports Telerik.Windows.Documents.FormatProviders.Html
Imports Telerik.Windows.Documents.Model
Imports System.IO

Partial Public Class Announcements
    Inherits Page

    Public Sub New()
        InitializeComponent()

        CType(provider2.FormatProvider, HtmlFormatProvider).ExportSettings = New HtmlExportSettings With {.DocumentExportLevel = DocumentExportLevel.Fragment, .StylesExportMode = StylesExportMode.Inline}
    End Sub

    'Executes when the user navigates to this page.
    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)

    End Sub

    Private Sub CodeChallenge_AnnouncementDomainDataSource_LoadedData(sender As System.Object, e As System.Windows.Controls.LoadedDataEventArgs) Handles CodeChallenge_AnnouncementDomainDataSource.LoadedData

        If e.HasError Then
            System.Windows.MessageBox.Show(e.Error.ToString, "Load Error", System.Windows.MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub hlbAdd_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbAdd.Click
        Dim newAnnouncement = New CodeChallenge_Announcement With {.Title = "New Posting"}
        CodeChallenge_AnnouncementDomainDataSource.DataView.Add(newAnnouncement)

        CodeChallenge_AnnouncementListBox.SelectedItem = newAnnouncement
    End Sub

    Private Sub hlbDelete_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbDelete.Click
        If CodeChallenge_AnnouncementListBox.SelectedItem IsNot Nothing Then
            CodeChallenge_AnnouncementDomainDataSource.DataView.Remove(CodeChallenge_AnnouncementListBox.SelectedItem)
        End If
    End Sub

    Private Sub CodeChallenge_AnnouncementDomainDataSource_SubmittedChanges(sender As Object, e As System.Windows.Controls.SubmittedChangesEventArgs) Handles CodeChallenge_AnnouncementDomainDataSource.SubmittedChanges
        If e.HasError Then
            MessageBox.Show(e.Error.ToString, "Submit Error", MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        End If
    End Sub

    'Private Sub CodeChallenge_AnnouncementDomainDataSource_SubmittingChanges(sender As Object, e As System.Windows.Controls.SubmittingChangesEventArgs) Handles CodeChallenge_AnnouncementDomainDataSource.SubmittingChanges
    '    For Each announcement As CodeChallenge_Announcement In CodeChallenge_AnnouncementDomainDataSource.DataView
    '        announcement.AnnouncementHtml = 
    '    Next
    'End Sub

    'Public Function ExportToHtml(ByVal document As RadDocument) As String
    '    Dim provider As New HtmlFormatProvider()
    '    Dim settings As New HtmlExportSettings()
    '    settings.DocumentExportLevel = DocumentExportLevel.Fragment
    '    settings.StylesExportMode = StylesExportMode.Inline

    '    provider.ExportSettings = settings

    '    Return provider.Export(document)
    'End Function
End Class
