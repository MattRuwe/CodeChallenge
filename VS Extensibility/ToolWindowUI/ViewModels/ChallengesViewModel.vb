Imports System.Reflection
Imports System.Windows.Forms
Imports EnvDTE
Imports EnvDTE80
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService
Imports VSLangProj
Imports System.Net
Imports OmahaMTG.Challenge.ChallengeCommon

Namespace ViewModels
    Public Class ChallengesViewModel
        Inherits ViewModelBase

        Public Sub New()
            ExecuteChallengeServiceCall(Of ResultsListing())(
                        Function(params As Object())
                            Dim results As QueryResultOfvw_codechallenge_secure = ChallengeClient.GetCodeChallengesWithDeveloperAssemblies()
                            ChallengesWithDetails = New List(Of vw_codechallenge_secure)(results.RootResults)

                            DeveloperAssemblies = New List(Of CodeChallenge_DeveloperAssembly)
                            For Each cda As CodeChallenge_DeveloperAssembly In results.IncludedResults
                                DeveloperAssemblies.Add(cda)
                            Next

                            Return Nothing
                        End Function, New Object() {})
        End Sub

        Private _challengesWithDetails As List(Of vw_codechallenge_secure)
        Public Property ChallengesWithDetails() As List(Of vw_codechallenge_secure)
            Get
                Return _challengesWithDetails
            End Get
            Set(ByVal value As List(Of vw_codechallenge_secure))
                _challengesWithDetails = value
                OnPropertyChanged("ChallengesWithDetails")
            End Set
        End Property

        Private _developerAssemblies As List(Of CodeChallenge_DeveloperAssembly)
        Public Property DeveloperAssemblies() As List(Of CodeChallenge_DeveloperAssembly)
            Get
                Return _developerAssemblies
            End Get
            Set(ByVal value As List(Of CodeChallenge_DeveloperAssembly))
                _developerAssemblies = value
                OnPropertyChanged("DeveloperAssemblies")
            End Set
        End Property


        Private _selectedChallenge As vw_codechallenge_secure
        Public Property SelectedChallenge() As vw_codechallenge_secure
            Get
                Return _selectedChallenge
            End Get
            Set(ByVal value As vw_codechallenge_secure)
                _selectedChallenge = value
                OnPropertyChanged("SelectedChallenge")
            End Set
        End Property

        Private _createNewProjectCommand As ICommand = New SimpleDelegateCommand(
                                                       Sub(param As Object)
                                                           CreateNewProject(CType(param, vw_codechallenge_secure))
                                                       End Sub)
        Public ReadOnly Property CreateNewProjectCommand As ICommand
            Get
                Return _createNewProjectCommand
            End Get
        End Property

        Private Sub CreateNewProject(challenge As vw_codechallenge_secure)
            Dim formattedChallengeName As String = challenge.ChallengeName.Replace(" ", "").Replace("'", "")
            Dim defaultProjectPath As String = CType(DTE.Properties("Environment", "ProjectsAndSolution").Item("ProjectsLocation").Value, String)
            Dim solutionPath As String = String.Format("{0}\{1}", defaultProjectPath, formattedChallengeName)

            'For Each prop As EnvDTE.Property In DTE.Properties("Environment", "ProjectsAndSolution")
            '    Debug.WriteLine(String.Format("{0} = {1}", prop.Name, prop.Value))
            'Next

            'Dim sfd As New OpenFileDialog
            'sfd.InitialDirectory = defaultProjectPath
            'sfd.CheckFileExists = False
            'sfd.CheckPathExists = False
            'sfd.Multiselect = False
            'sfd.FileName = defaultProjectPath
            'If sfd.ShowDialog() = DialogResult.OK Then
            '    solutionPath = sfd.FileName
            'Else
            '    Return
            'End If

            Dim nsd As New NewSolutionDialog()
            nsd.ShowDialog()


            'Dim solutionName As String = String.Format("{0}Solution", formattedChallengeName)
            Dim projectPath As String = String.Format("{0}\{1}", solutionPath, formattedChallengeName)

            DTE.Solution.Create(solutionPath, formattedChallengeName)
            Dim solutionInst As Solution2 = CType(DTE.Solution, Solution2)

            Dim templatePath As String = solutionInst.GetProjectTemplate("ClassLibrary.zip", "CSharp")
            DTE.Solution.AddFromTemplate(templatePath, projectPath, formattedChallengeName)

            Dim project As VSProject = ActiveVsProjects(0)

            'Remove the default class1.cs file
            project.Project.ProjectItems.Item("class1.cs").Remove()

            Dim references As References = project.References

            'http://stackoverflow.com/questions/10126846/visual-studio-extensions
            project.Project.ProjectItems.AddFromTemplate(solutionInst.GetProjectItemTemplate("class", "CSharp"), formattedChallengeName & ".cs")

            Dim challengeAssemblies As List(Of CodeChallenge_DeveloperAssembly) = DeveloperAssemblies.Where(Function(d) d.codechallenge_id = challenge.id).ToList

            For Each ccAssem As CodeChallenge_DeveloperAssembly In challengeAssemblies
                Dim assemName As New AssemblyName(ccAssem.assembly_fullname)
                Dim assemPath As String = String.Format("{0}\{1}.dll", solutionPath, assemName.Name)
                My.Computer.FileSystem.WriteAllBytes(assemPath, ccAssem.assembly, False)
                references.Add(assemPath)
            Next

            Dim challengeImplementation As New ChallengeCodeGen()
            Dim newChallengeCode As String = challengeImplementation.Generate(solutionPath)

            Dim txtSel As TextSelection = CType(DTE.ActiveDocument.Selection, TextSelection)
            'Dim txtDoc As TextDocument = CType(DTE.ActiveDocument.Object, TextDocument)

            txtSel.SelectAll()
            txtSel.Delete()
            txtSel.Insert(newChallengeCode)
        End Sub
    End Class
End Namespace