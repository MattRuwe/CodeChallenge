Imports System.IO
Imports System.ServiceModel.DomainServices.Client

Partial Public Class Assemblies
    Inherits Page

    Public Sub New()
        InitializeComponent()
    End Sub

    'Executes when the user navigates to this page.
    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)

    End Sub

    Private Sub CodeChallenge_AssemblyDomainDataSource_LoadedData(ByVal sender As System.Object, ByVal e As System.Windows.Controls.LoadedDataEventArgs) Handles CodeChallenge_AssemblyDomainDataSource.LoadedData

        If e.HasError Then
            System.Windows.MessageBox.Show(e.Error.ToString, "Load Error", System.Windows.MessageBoxButton.OK)
            e.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub CodeChallenge_AssemblyDataGrid_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If e.Key = Key.Delete AndAlso CodeChallenge_AssemblyDataGrid.SelectedItem IsNot Nothing Then
            CodeChallenge_AssemblyDomainDataSource.DataView.Remove(CodeChallenge_AssemblyDataGrid.SelectedItem)
            CodeChallenge_AssemblyDomainDataSource.SubmitChanges()
        End If
    End Sub

    Private Sub hlbAddAssembly_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbAddAssembly.Click
        Dim openFileDialog As New OpenFileDialog
        Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
        If dialogResult.HasValue AndAlso openFileDialog.File IsNot Nothing Then
            Using fs As FileStream = openFileDialog.File.OpenRead
                Using ms As New MemoryStream
                    Dim buffer(8192) As Byte
                    Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
                    While bytesRead > 0
                        ms.Write(buffer, 0, bytesRead)
                        bytesRead = fs.Read(buffer, 0, buffer.Length)
                    End While
                    Dim file() As Byte = ms.ToArray
                    If file.Length > 0 Then
                        Dim codeChallengeAssembly As New CodeChallenge_Assembly
                        codeChallengeAssembly.assembly = file

                        Dim domainContext As New CodeChallengeDomainContext
                        Dim op As InvokeOperation(Of String) = domainContext.GetAssemblyFullname(file)

                        AddHandler op.Completed,
                            Sub(sender2 As Object, e2 As EventArgs)
                                If op.Error Is Nothing AndAlso Not String.IsNullOrWhiteSpace(op.Value) Then
                                    codeChallengeAssembly.AssemblyFullName = op.Value
                                    CodeChallenge_AssemblyDomainDataSource.DataView.Add(codeChallengeAssembly)
                                    CodeChallenge_AssemblyDomainDataSource.SubmitChanges()
                                Else
                                    MessageBox.Show("The assembly is not valid")
                                    CodeChallenge_AssemblyDomainDataSource.RejectChanges()
                                End If
                            End Sub
                    End If

                End Using
            End Using
        End If
    End Sub


    Private Sub GridDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If CodeChallenge_AssemblyDataGrid.SelectedItem IsNot Nothing Then
            CodeChallenge_AssemblyDomainDataSource.DataView.Remove(CodeChallenge_AssemblyDataGrid.SelectedItem)
            CodeChallenge_AssemblyDomainDataSource.SubmitChanges()
        End If
    End Sub

    Private Sub CodeChallenge_AssemblyDataGrid_RowEditEnded(ByVal sender As System.Object, ByVal e As System.Windows.Controls.DataGridRowEditEndedEventArgs) Handles CodeChallenge_AssemblyDataGrid.RowEditEnded
        CodeChallenge_AssemblyDomainDataSource.SubmitChanges()
    End Sub
End Class
