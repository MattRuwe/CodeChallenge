Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports System.IO
Imports System.ServiceModel.DomainServices.Client

''' <summary>
''' <see cref="Page"/> class to present information about the current application.
''' </summary>
Partial Public Class Entry
    Inherits Page

    ''' <summary>
    ''' Creates a new instance of the <see cref="About"/> class.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.CodeChallengeEntryPageTitle
        _newEntry = New CodeChallenge_Entry
    End Sub

    Private _newEntry As CodeChallenge_Entry

    ''' <summary>
    ''' Executes when the user navigates to this page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub

    'Private Sub hlbAddEntry_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbAddEntry.Click
    '    hlbSubmitEntry.IsEnabled = False
    '    hlbCancelEntry.IsEnabled = False
    '    hlbAddEntry.IsEnabled = False

    '    Dim openFileDialog As New OpenFileDialog
    '    Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
    '    If dialogResult.HasValue AndAlso openFileDialog.File IsNot Nothing Then
    '        Using fs As FileStream = openFileDialog.File.OpenRead
    '            Dim buffer(8192) As Byte
    '            Using ms As New MemoryStream
    '                Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
    '                While bytesRead > 0
    '                    ms.Write(buffer, 0, bytesRead)
    '                    bytesRead = fs.Read(buffer, 0, buffer.Length)
    '                End While
    '                _newEntry.Submission = ms.ToArray



    '                If _newEntry.Submission.Length > 0 Then
    '                    Dim domainContext As New CodeChallengeDomainContext
    '                    Dim op As InvokeOperation(Of String) = domainContext.GetAssemblyFullname(_newEntry.Submission)

    '                    AddHandler op.Completed,
    '                        Sub(sender2 As Object, e2 As EventArgs)
    '                            If op.Error Is Nothing Then
    '                                _newEntry.AssemblyFullName = op.Value
    '                                op = Nothing

    '                                Dim op2 As InvokeOperation(Of IEnumerable(Of String)) = domainContext.GetIChallengeImplementations(_newEntry.Submission)
    '                                AddHandler op2.Completed,
    '                                    Sub(sender3 As Object, e3 As EventArgs)
    '                                        If op2.Error Is Nothing Then
    '                                            If op2.Value.Count = 1 Then
    '                                                _newEntry.TypeName = op2.Value.ToList()(0)
    '                                            End If

    '                                            hlbSubmitEntry.IsEnabled = True
    '                                            hlbCancelEntry.IsEnabled = True
    '                                            hlbAddEntry.IsEnabled = True
    '                                        End If
    '                                    End Sub
    '                            End If

    '                        End Sub
    '                Else
    '                    tbAssemblyFullName.Text = "Could not load assembly"
    '                End If
    '            End Using
    '        End Using
    '    End If
    'End Sub

    'Private Sub hlbSubmitEntry_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbSubmitEntry.Click
    '    Dim context As New CodeChallengeDomainContext

    '    context.CodeChallenge_Entries.Add(_newEntry)

    '    context.SubmitChanges()

    'End Sub

    Private Sub CodeChallenge_EntryDomainDataSource_LoadedData(ByVal sender As System.Object, ByVal e As System.Windows.Controls.LoadedDataEventArgs) Handles CodeChallenge_EntryDomainDataSource.LoadedData

        If e.HasError Then
            System.Windows.MessageBox.Show(e.Error.ToString, "Load Error", System.Windows.MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        End If

    End Sub

    Private Sub hlbAddEntry_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbAddEntry.Click
        Dim newEntry As New NewEntryForm()
        AddHandler newEntry.Closed,
            Sub(sender2 As Object, e2 As EventArgs)
                If newEntry.NewEntry IsNot Nothing Then
                    Dim domaindatasource = CodeChallenge_EntryDomainDataSource

                    Dim dc As CodeChallengeDomainContext = CType(domaindatasource.DomainContext, CodeChallengeDomainContext)
                    dc.CodeChallenge_Entries.Add(newEntry.NewEntry)
                    dc.SubmitChanges(Sub(so As SubmitOperation)
                                         If so.HasError Then
                                             MessageBox.Show(so.Error.Message)
                                             so.MarkErrorAsHandled()
                                         Else
                                             CodeChallenge_EntryDomainDataSource.Load()
                                         End If
                                     End Sub, Nothing)
                End If
            End Sub

        newEntry.Show()
    End Sub

    Private Sub CodeChallenge_EntryDomainDataSource_LoadingData(sender As Object, e As System.Windows.Controls.LoadingDataEventArgs) Handles CodeChallenge_EntryDomainDataSource.LoadingData
        e.LoadBehavior = LoadBehavior.RefreshCurrent
    End Sub

    Private Sub CodeChallenge_EntryDomainDataSource_SubmittedChanges(ByVal sender As Object, ByVal e As System.Windows.Controls.SubmittedChangesEventArgs) Handles CodeChallenge_EntryDomainDataSource.SubmittedChanges
        If e.HasError Then
            System.Windows.MessageBox.Show(e.Error.ToString, "Save Error", System.Windows.MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub hlbRefresh_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbRefresh.Click
        If CodeChallenge_EntryDomainDataSource.CanLoad Then
            CodeChallenge_EntryDomainDataSource.Load()
        End If
    End Sub
End Class