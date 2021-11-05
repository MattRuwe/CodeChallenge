Imports System.IO
Imports System.Reflection
Imports System.ServiceModel.DomainServices.Client
Imports System.ComponentModel
Imports Telerik.Windows.Documents.Model
Imports Telerik.Windows.Documents.FormatProviders.Xaml

Partial Public Class NewChallengeForm
    Inherits ChildWindow
    Implements INotifyPropertyChanged

    Private _domainDataSource As DomainDataSource

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Public Sub New(ByVal domainDataSource As DomainDataSource)
        InitializeComponent()

        If _domainDataSource IsNot Nothing Then
            Resources.Add("DomainContext", _domainDataSource)
        Else
            Resources.Add("DomainContext", New CodeChallengeDomainContext)
        End If
        SponsorDomainDataSource.DomainContext = Resources("DomainContext")
        SponsorDomainDataSource.AutoLoad = True

        NewChallenge = New CodeChallenge
        _domainDataSource = domainDataSource

    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        NewChallenge = Nothing
        Me.DialogResult = False
    End Sub


    Private _newChallenge As CodeChallenge
    Public Property NewChallenge() As CodeChallenge
        Get
            Return GridNewChallenge.DataContext
        End Get
        Set(ByVal value As CodeChallenge)
            GridNewChallenge.DataContext = value
        End Set
    End Property

    Private Sub hlbSetExecutor_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbSetExecutor.Click
        OKButton.IsEnabled = False
        CancelButton.IsEnabled = False
        hlbSetExecutor.IsEnabled = False

        Dim openFileDialog As New OpenFileDialog
        Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
        If openFileDialog.File IsNot Nothing Then
            Using fs As FileStream = openFileDialog.File.OpenRead
                Dim buffer(8192) As Byte
                Using ms As New MemoryStream
                    Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
                    While bytesRead > 0
                        ms.Write(buffer, 0, bytesRead)
                        bytesRead = fs.Read(buffer, 0, buffer.Length)
                    End While
                    NewChallenge.ExecutorAssembly = ms.ToArray
                    If NewChallenge.ExecutorAssembly.Length > 0 Then
                        Dim domainContext As New CodeChallengeDomainContext
                        Dim op As InvokeOperation(Of String) = domainContext.GetAssemblyFullname(NewChallenge.ExecutorAssembly)

                        AddHandler op.Completed,
                            Sub(sender2 As Object, e2 As EventArgs)
                                NewChallenge.ExecutorAssemblyFullName = op.Value
                                OKButton.IsEnabled = True
                                CancelButton.IsEnabled = True
                                hlbSetExecutor.IsEnabled = True
                            End Sub
                    Else
                        NewChallenge.ExecutorAssemblyFullName = "Could not load assembly"
                    End If
                End Using
            End Using
        Else
            OKButton.IsEnabled = True
            CancelButton.IsEnabled = True
            hlbSetExecutor.IsEnabled = True
        End If
    End Sub

    Private Sub hlbAddDevAssembly_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hlbAddDevAssembly.Click
        OKButton.IsEnabled = False
        CancelButton.IsEnabled = False
        hlbAddDevAssembly.IsEnabled = False
        Dim openFileDialog As New OpenFileDialog
        Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
        If openFileDialog.File IsNot Nothing Then
            Using fs As FileStream = openFileDialog.File.OpenRead
                Dim buffer(8192) As Byte
                Using ms As New MemoryStream
                    Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
                    While bytesRead > 0
                        ms.Write(buffer, 0, bytesRead)
                        bytesRead = fs.Read(buffer, 0, buffer.Length)
                    End While
                    Dim devAssem As New CodeChallenge_DeveloperAssembly With {.assembly = ms.ToArray}
                    'devAssem.assembly_fullname = Path.GetFileNameWithoutExtension(openFileDialog.File.FullName)
                    Dim domainContext As New CodeChallengeDomainContext
                    Dim op As InvokeOperation(Of String) = domainContext.GetAssemblyFullname(devAssem.assembly)

                    AddHandler op.Completed,
                        Sub(sender2 As Object, e2 As EventArgs)
                            devAssem.assembly_fullname = op.Value
                            NewChallenge.CodeChallenge_DeveloperAssembly.Add(devAssem)
                            OKButton.IsEnabled = True
                            CancelButton.IsEnabled = True
                            hlbAddDevAssembly.IsEnabled = True
                        End Sub


                End Using
            End Using
        End If
    End Sub

    Private Sub btnDeleteDevAssembly_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If _domainDataSource Is Nothing Then
            NewChallenge.CodeChallenge_DeveloperAssembly.Remove(CodeChallenge_DeveloperAssemblyDataGrid.SelectedItem)
        Else
            TryCast(_domainDataSource.DomainContext, CodeChallengeDomainContext).CodeChallenge_DeveloperAssemblies.Remove(CodeChallenge_DeveloperAssemblyDataGrid.SelectedItem)
        End If
    End Sub

    Private Sub hlbSetExecutionCommon_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbSetExecutionCommon.Click
        OKButton.IsEnabled = False
        CancelButton.IsEnabled = False
        hlbSetExecutionCommon.IsEnabled = False

        Dim openFileDialog As New OpenFileDialog
        Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
        If openFileDialog.File IsNot Nothing Then
            Using fs As FileStream = openFileDialog.File.OpenRead
                Dim buffer(8192) As Byte
                Using ms As New MemoryStream
                    Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
                    While bytesRead > 0
                        ms.Write(buffer, 0, bytesRead)
                        bytesRead = fs.Read(buffer, 0, buffer.Length)
                    End While
                    NewChallenge.ExecutionCommonAssembly = ms.ToArray
                    If NewChallenge.ExecutorAssembly.Length > 0 Then
                        Dim domainContext As New CodeChallengeDomainContext
                        Dim op As InvokeOperation(Of String) = domainContext.GetAssemblyFullname(NewChallenge.ExecutionCommonAssembly)

                        AddHandler op.Completed,
                            Sub(sender2 As Object, e2 As EventArgs)
                                NewChallenge.ExecutionCommonAssemblyFullName = op.Value
                                OKButton.IsEnabled = True
                                CancelButton.IsEnabled = True
                                hlbSetExecutionCommon.IsEnabled = True
                            End Sub
                    Else
                        NewChallenge.ExecutorAssemblyFullName = "Could not load assembly"
                    End If
                End Using
            End Using
        End If
    End Sub

    Private Sub hlbSetDocumentation_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbSetDocumentation.Click
        Dim filename As String = Nothing
        Dim file As Byte() = ReadFile(filename)

        If file IsNot Nothing AndAlso file.Length > 0 AndAlso filename.Trim.Length > 0 Then
            NewChallenge.Documentation = file
            NewChallenge.DocumentationFilename = filename
        End If
    End Sub

    Private Sub hlbSetSampleProject_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles hlbSetSampleProject.Click
        Dim filename As String = Nothing
        Dim file As Byte() = ReadFile(filename)

        If file IsNot Nothing AndAlso file.Length > 0 AndAlso filename.Trim.Length > 0 Then
            NewChallenge.SampleProject = file
            NewChallenge.SampleProjectFilename = filename
        End If
    End Sub

    Private Function ReadFile(ByRef filename As String) As Byte()
        Dim returnValue As Byte() = Nothing
        Dim openFileDialog As New OpenFileDialog
        Dim dialogResult As Boolean? = openFileDialog.ShowDialog.HasValue
        If openFileDialog.File IsNot Nothing Then
            Using fs As FileStream = openFileDialog.File.OpenRead
                Dim buffer(8192) As Byte
                Using ms As New MemoryStream
                    Dim bytesRead As Integer = fs.Read(buffer, 0, buffer.Length)
                    While bytesRead > 0
                        ms.Write(buffer, 0, bytesRead)
                        bytesRead = fs.Read(buffer, 0, buffer.Length)
                    End While
                    returnValue = ms.ToArray

                    If returnValue.Length > 0 Then
                        filename = openFileDialog.File.Name
                    End If
                End Using
            End Using
        End If

        Return returnValue
    End Function

    Private Sub cbSponsor_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cbSponsor.SelectionChanged
        Dim sponsorID As Integer? = CType(cbSponsor.SelectedItem, CodeChallenge_Sponsor).id

        If sponsorID <= 0 Then
            NewChallenge.CodeChallenge_Sponsor_id = Nothing
        Else
            NewChallenge.CodeChallenge_Sponsor_id = sponsorID
        End If

    End Sub

    Private Sub SponsorDomainDataSource_LoadedData(sender As Object, e As System.Windows.Controls.LoadedDataEventArgs) Handles SponsorDomainDataSource.LoadedData
        cbSponsor.Items.Clear()
        cbSponsor.Items.Add(New CodeChallenge_Sponsor With {.id = 0, .Name = "No sponsor"})
        For Each cs As CodeChallenge_Sponsor In e.AllEntities().ToList()
            cbSponsor.Items.Add(New CodeChallenge_Sponsor With {.id = cs.id, .Name = cs.Name})
        Next


        If NewChallenge IsNot Nothing AndAlso NewChallenge.CodeChallenge_Sponsor_id.HasValue Then
            For Each sponsor As CodeChallenge_Sponsor In cbSponsor.Items
                If sponsor.id = NewChallenge.CodeChallenge_Sponsor_id Then
                    cbSponsor.SelectedItem = sponsor
                    Exit For
                End If
            Next

        End If
    End Sub
End Class
