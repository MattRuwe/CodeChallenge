Imports System.Collections.Generic

Imports OmahaMTG.Challenge.Challenges

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.WordSearchImplementation
Imports System.Text



'''<summary>
'''This is a test class for WordSearchImplementationTest and is intended
'''to contain all WordSearchImplementationTest Unit Tests
'''</summary>
<TestClass()> _
Public Class WordSearchImplementationTest


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

    '''<summary>
    '''A test for SolveWordSearch
    '''</summary>
    <TestMethod()> _
    Public Sub SolveWordSearchTest()
        Dim target As IWordSearchChallenge = New WordSearchImplementation()
        Dim puzzle As Puzzle = puzzle.CreatePuzzle(1000, 500)
        Dim actual As IEnumerable(Of FoundWord)
        actual = target.SolveWordSearch(puzzle)

        Dim puzzleResult As PuzzleResults = puzzle.GetResults(actual)

        Assert.AreEqual(puzzle.Words.Count, puzzleResult.MatchedWords.Count)

    End Sub
End Class
