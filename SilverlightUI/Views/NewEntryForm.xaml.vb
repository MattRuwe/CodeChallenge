Imports System.IO
Imports System.ServiceModel.DomainServices.Client
Imports System.ComponentModel

Partial Public Class NewEntryForm
    Inherits ChildWindow

    Public Sub New()
        InitializeComponent()
        NewEntry = GetNewCodeChallenge_Entry()
        Grid1.DataContext = NewEntry

        'Dim query As EntityQuery(Of vw_codechallenge_secure) = _ctx.GetCodeChallengeSecureQuery().Where(Function(c) c.StartDate < DateTime.Now AndAlso c.EndDate > DateTime.Now)
        'Dim lo As LoadOperation(Of vw_codechallenge_secure) = _ctx.Load(Of vw_codechallenge_secure)(query)

        'AddHandler lo.Completed, Sub(sender As Object, e As EventArgs)
        '                             If lo.HasError Then
        '                                 MessageBox.Show(lo.Error.Message)
        '                                 lo.MarkErrorAsHandled()
        '                             End If
        '                         End Sub

        'cbCodeChallenge.ItemsSource = lo.Entities
    End Sub

    Private Function GetNewCodeChallenge_Entry() As CodeChallenge_Entry
        Return New CodeChallenge_Entry With {.IsPublished = True, .IsTest = False}
    End Function


    Private _newEntry As CodeChallenge_Entry
    Public Property NewEntry() As CodeChallenge_Entry
        Get
            Return _newEntry
        End Get
        Set(ByVal value As CodeChallenge_Entry)
            _newEntry = value
        End Set
    End Property

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        NewEntry.AuthorUserId = WebContext.Current.User.UserID
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        NewEntry = Nothing
        Me.DialogResult = False
    End Sub

    Private Sub hlbUploadEntry_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbUploadEntry.Click
        BusyIndicator.IsBusy = True

        tbMessage.Text = String.Empty

        OKButton.IsEnabled = False
        CancelButton.IsEnabled = False

        Dim openFileDialog As New OpenFileDialog
        Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
        If dialogResult.HasValue AndAlso openFileDialog.File IsNot Nothing Then
            BusyIndicator.BusyContent = "Please Wait...  Loading file..."
            Using fs As FileStream = openFileDialog.File.OpenRead
                Dim buffer(8192) As Byte
                Using ms As New MemoryStream
                    Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
                    While bytesRead > 0
                        ms.Write(buffer, 0, bytesRead)
                        bytesRead = fs.Read(buffer, 0, buffer.Length)
                    End While
                    _newEntry.Submission = ms.ToArray



                    If _newEntry.Submission.Length > 0 Then
                        Dim domainContext As New CodeChallengeDomainContext
                        BusyIndicator.BusyContent = "Please Wait...  Retrieving assembly fullname..."
                        Dim op As InvokeOperation(Of String) = domainContext.GetAssemblyFullname(_newEntry.Submission)

                        AddHandler op.Completed,
                            Sub(sender2 As Object, e2 As EventArgs)
                                If op.Error Is Nothing Then
                                    _newEntry.AssemblyFullName = op.Value
                                    op = Nothing

                                    BusyIndicator.BusyContent = "Please Wait...  Finding IChallenge implementations..."
                                    Dim op2 As InvokeOperation(Of String) = domainContext.GetIChallengeImplementations(_newEntry.Submission)
                                    AddHandler op2.Completed,
                                        Sub(sender3 As Object, e3 As EventArgs)
                                            If op2.Error Is Nothing Then
                                                If op2.Value IsNot Nothing Then
                                                    Dim challengeImplementationResult() As String = Split(op2.Value, "||")
                                                    Dim implementationFullName As String = challengeImplementationResult(0)
                                                    Dim challengeName As String = challengeImplementationResult(1)
                                                    Dim challengeid As String = challengeImplementationResult(2)
                                                    Dim loadingError As String = challengeImplementationResult(3)

                                                    If String.IsNullOrWhiteSpace(loadingError) AndAlso challengeid > 0 AndAlso Not String.IsNullOrEmpty(challengeName) AndAlso Not String.IsNullOrEmpty(implementationFullName) Then
                                                        _newEntry.TypeName = implementationFullName
                                                        _newEntry.CodeChallenge_Id = Integer.Parse(challengeid)
                                                        tbChallengeName.Text = challengeName
                                                        tbMessage.Text = "The entry was successfully validated!"
                                                        OKButton.IsEnabled = True
                                                        CancelButton.IsEnabled = True
                                                    ElseIf Not String.IsNullOrWhiteSpace(loadingError) Then
                                                        NewEntry = GetNewCodeChallenge_Entry()
                                                        Grid1.DataContext = NewEntry
                                                        MessageBox.Show("Couldn't load your entry due to the following error: " & loadingError)
                                                        OKButton.IsEnabled = False
                                                        CancelButton.IsEnabled = True
                                                    Else
                                                        NewEntry = GetNewCodeChallenge_Entry()
                                                        Grid1.DataContext = NewEntry
                                                        tbMessage.Text = "The assembly could not be loaded."
                                                        OKButton.IsEnabled = False
                                                        CancelButton.IsEnabled = True
                                                    End If
                                                Else
                                                    OKButton.IsEnabled = False
                                                    CancelButton.IsEnabled = True
                                                End If
                                            Else
                                                OKButton.IsEnabled = False
                                                CancelButton.IsEnabled = True
                                            End If
                                            BusyIndicator.IsBusy = False
                                        End Sub
                                End If
                            End Sub
                    Else
                        tbMessage.Text = "Could not load assembly"
                        BusyIndicator.IsBusy = False
                    End If
                End Using
            End Using
        Else
            OKButton.IsEnabled = False
            CancelButton.IsEnabled = True
            BusyIndicator.IsBusy = False
        End If
    End Sub


    Private Sub chkIsPublished_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles chkIsPublished.Click
        If chkIsPublished.IsChecked Then
            NewEntry.IsTest = False
        End If
    End Sub

    Private Sub chkIsTest_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles chkIsTest.Click
        If chkIsTest.IsChecked Then
            NewEntry.IsPublished = False
        End If
    End Sub
End Class
