Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.Challenges



'''<summary>
'''This is a test class for BoardTest and is intended
'''to contain all BoardTest Unit Tests
'''</summary>
<TestClass()> _
Public Class BoardTest


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
            testContextInstance = Value
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
    '''A test for Board Constructor
    '''</summary>
    '<TestMethod()> _
    'Public Sub BoardConstructorTest()
    '    Dim boardSize As Integer = 3
    '    Dim target As Board = New Board(boardSize)
    '    target.Board = {Player.X, Player.X, Player.O, Player.O, Player.O, Player.X, Player.X, Player.X, Player.O}

    '    Dim status As GameStatus = target.GetGameStatus()
    '    Assert.AreEqual(GameStatus.XWins, status)
    'End Sub

    '<TestMethod()>
    'Public Sub PlayerTest()
    '    Dim board As Board
    '    Dim status As GameStatus
    '    While status <> GameStatus.Draw
    '        board = New Board(3)
    '        status = board.GetGameStatus()
    '        While status = GameStatus.InProgress
    '            Dim move As BoardIndex = board.GetRandomMove()
    '            board.MakeMove(GetBestMove(board.Clone))
    '            status = board.GetGameStatus()
    '        End While
    '        Debug.WriteLine(board)
    '        Debug.WriteLine(status)
    '    End While

    'End Sub
End Class