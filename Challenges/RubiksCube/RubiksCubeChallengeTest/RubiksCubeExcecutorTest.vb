Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.Challenges
Imports OmahaMTG.Challenge.RubiksCubeImplementation



'''<summary>
'''This is a test class for RubiksCubeExcecutorTest and is intended
'''to contain all RubiksCubeExcecutorTest Unit Tests
'''</summary>
<TestClass()> _
Public Class RubiksCubeExcecutorTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for RunChallengeOverride
    '''</summary>
    '<TestMethod(), _
    ' DeploymentItem("OmahaMTG.Challenge.RubiksCubeExecutor.dll")> _
    'Public Sub RunChallengeOverrideTest()
    '    Dim target As New RubiksCubeExcecutor
    '    target.RunChallenge(New RubiksCubeImpl)


    'End Sub
End Class
