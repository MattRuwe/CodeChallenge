Imports EnvDTE
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.ViewModels
Imports System.IO
Imports System.Reflection
Imports OmahaMTG.Challenge.ChallengeCommon
Imports Mono.Cecil
Imports Mono.Cecil.Rocks

Namespace ViewModels
    Public Class MyEntriesViewModel
        Inherits ViewModelBase

        Public Sub New()
        End Sub

        Private _selectedChallenge As CodeChallengeListing
        Public Property SelectedChallenge() As CodeChallengeListing
            Get
                Return _selectedChallenge
            End Get
            Set(ByVal value As CodeChallengeListing)
                _selectedChallenge = value
                OnPropertyChanged("SelectedChallenge")
                If value IsNot Nothing Then
                    ExecuteChallengeServiceCall(Of ResultsListing())(
                        Function(params As Object())
                            If value IsNot Nothing Then
                                Results = ChallengeClient.GetResults(params(0)).RootResults
                            End If
                        End Function, New Object() {value.id})
                Else
                    Results = Nothing
                End If
            End Set
        End Property

        Private _results As ResultsListing()
        Public Property Results() As ResultsListing()
            Get
                Return _results
            End Get
            Set(ByVal value As ResultsListing())
                _results = value
                OnPropertyChanged("Results")
            End Set
        End Property

        Private _selectedResults As ResultsListing
        Public Property SelectedResult() As ResultsListing
            Get
                Return _selectedResults
            End Get
            Set(ByVal value As ResultsListing)
                If value IsNot _selectedResults Then
                    _selectedResults = value
                    OnPropertyChanged("SelectedResult")
                End If
            End Set
        End Property

        Private ReadOnly _addEntryCommand As New SimpleDelegateCommand(Sub(param As Object)
                                                                           AddEntry()
                                                                       End Sub)
        Public ReadOnly Property AddEntryCommand As ICommand
            Get
                Return _addEntryCommand
            End Get
        End Property

        Private Sub AddEntry()
            Dim outputFiles As List(Of String)

            outputFiles = GetOutputBinaries()

            For Each outputFile As String In outputFiles
                Dim localOutputFile As String = outputFile

                Dim outputModule As ModuleDefinition = ModuleDefinition.ReadModule(outputFile)

                For Each assemblyType As TypeDefinition In outputModule.Types

                    Dim localAssemblyType As TypeDefinition = assemblyType

                    If assemblyType.Interfaces.Any(Function(myType) myType.FullName = GetType(IChallenge).FullName) Then

                        Dim assemBytes() As Byte = File.ReadAllBytes(outputFile)

                        Dim result As String() = Split(ExecuteChallengeServiceCall(Function(params As IEnumerable(Of Object))
                                                                                       Return ChallengeClient.GetIChallengeImplementations(assemBytes)
                                                                                   End Function, Nothing), "||")

                        Dim newChallengeEntry As New CodeChallenge_Entry With
                                                            {
                                                                .AssemblyFullName = outputModule.Assembly.FullName,
                                                                .CodeChallenge_Id = result(2),
                                                                .Submission = assemBytes,
                                                                .IsTest = True,
                                                                .IsPublished = False,
                                                                .TypeName = localAssemblyType.FullName
                                                            }

                        Dim newItems(0) As ChangeSetEntry
                        newItems(0) = New ChangeSetEntry
                        With newItems(0)
                            .OriginalEntity = Nothing
                            .Entity = newChallengeEntry
                            .Operation = DomainOperation.Insert
                        End With

                        Dim submitResult As ChangeSetEntry() = ExecuteChallengeServiceCall(Function(params As IEnumerable(Of Object)) As ChangeSetEntry()
                                                                                               Return ChallengeClient.SubmitChanges(newItems)
                                                                                           End Function, New Object() {})
                    End If
                Next
            Next

        End Sub

        Private Function GetOutputBinaries() As List(Of String)
            Dim returnValue = New List(Of String)
            For Each project As Project In ActiveProjects
                Dim cm As ConfigurationManager = project.ConfigurationManager
                If cm IsNot Nothing Then
                    Dim ac As Configuration = cm.ActiveConfiguration
                    For Each grpOut As EnvDTE.OutputGroup In ac.OutputGroups
                        If grpOut.DisplayName = "Primary output" Then
                            Dim lst As Array = grpOut.FileURLs
                            For i As Long = 0 To lst.Length - 1
                                Dim filePath As String = lst.GetValue(i)
                                If filePath.StartsWith("file:///") Then
                                    filePath = filePath.Substring(8)
                                End If
                                returnValue.Add(filePath)
                            Next
                        End If
                    Next
                End If
            Next
            Return returnValue
        End Function
    End Class
End Namespace