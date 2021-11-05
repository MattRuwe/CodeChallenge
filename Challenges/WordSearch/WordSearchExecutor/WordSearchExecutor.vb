Imports OmahaMTG.Challenge.ExecutionCommon
Imports System.Reflection
Imports System.IO
Imports System.Text.RegularExpressions

Public Class WordSearchExecutor
    Inherits ChallengeExecutorBase(Of IWordSearchChallenge)

    Protected Overrides ReadOnly Property MaxAuthorNotesLength As Integer
        Get
            Return 0
        End Get
    End Property

    Protected Overrides Sub RunChallengeOverride()
        Dim currentPuzzle As Puzzle
        Dim puzzleChoice = Enumerable.Range(1, 4)
        puzzleChoice = puzzleChoice.OrderBy(Function(val) Guid.NewGuid)

        For Each val As Integer In puzzleChoice
            Select Case val
                Case 1
                    currentPuzzle = GetPuzzle1()
                Case 2
                    currentPuzzle = GetPuzzle2()
                Case 3
                    currentPuzzle = GetPuzzle3()
                Case 4
                    currentPuzzle = GetPuzzle4()
                Case Else
                    currentPuzzle = Nothing
            End Select

            RunSingleChallenge(currentPuzzle)
        Next
    End Sub

    Private Sub RunSingleChallenge(puzzle As Puzzle)
        Dim stopWatch As Stopwatch = stopWatch.StartNew
        Dim foundWords As IEnumerable(Of FoundWord) = Challenge.SolveWordSearch(puzzle.Clone())
        Dim foundWordList As List(Of FoundWord) = foundWords.ToList()
        stopWatch.Stop()

        Dim puzzleResult As PuzzleResults = puzzle.GetResults(foundWordList)

        ResultsAvailable(New ChallengeResult With {
                         .ResultMessage = String.Format("{0} matches, {1} false matches, {2} unmatched words", puzzleResult.MatchedWords.Count, puzzleResult.UnmatchedWords.Count, puzzle.Words.Count - puzzleResult.MatchedWords.Count),
                         .Score = GetScore(puzzleResult.MatchedWords.Count, stopWatch.ElapsedMilliseconds, puzzleResult.UnmatchedWords.Count),
                         .DurationTicks = stopWatch.Elapsed.Ticks})
    End Sub

    Private Function GetScore(matchedWords As Integer, millisecondsElapsed As Integer, unmatchedWords As Integer) As Integer
        Dim returnValue As Integer = 0
        If millisecondsElapsed <= 30000 Then
            returnValue = matchedWords * 500 + (30000 - millisecondsElapsed) - (unmatchedWords * 500)
        Else
            returnValue = matchedWords * 500 - (unmatchedWords * 500)
        End If

        If returnValue < 0 Then
            returnValue = 0
        End If

        Return returnValue
    End Function

    Private Function GetPuzzle1() As Puzzle
        Dim rawPuzzle As String = ReadResource("Puzzle1")
        Dim returnValue As Puzzle = GetPuzzleFromRaw(rawPuzzle)

        Return returnValue
    End Function

    Private Function GetPuzzle2() As Puzzle
        Dim rawPuzzle As String = ReadResource("Puzzle2")
        Dim returnValue As Puzzle = GetPuzzleFromRaw(rawPuzzle)

        Return returnValue
    End Function

    Private Function GetPuzzle3() As Puzzle
        Dim rawPuzzle As String = ReadResource("Puzzle3")
        Dim returnValue As Puzzle = GetPuzzleFromRaw(rawPuzzle)

        Return returnValue
    End Function

    Private Function GetPuzzle4() As Puzzle
        Dim rawPuzzle As String = ReadResource("Puzzle4")
        Dim returnValue As Puzzle = GetPuzzleFromRaw(rawPuzzle)

        Return returnValue
    End Function

    Private Function GetPuzzleFromRaw(raw As String) As Puzzle
        Dim returnValue As Puzzle = Nothing

        Dim boardAndWords As MatchCollection = Regex.Matches(raw, "(?m)[^-]+(?=\-)?")
        Dim rawBoard As String
        Dim rawWords As String
        Dim grid(,) As Char = Nothing
        Dim words As New List(Of String)
        If boardAndWords.Count = 2 Then
            rawBoard = boardAndWords(0).Value
            rawWords = boardAndWords(1).Value

            Dim lines As MatchCollection = Regex.Matches(rawBoard, "\S+")
            If lines.Count > 0 Then
                Dim lineLength As Integer = lines(0).Value.Length
                Dim numberOfLines As Integer = lines.Count
                ReDim grid(lineLength - 1, numberOfLines - 1)
                Dim currentRow As Integer = 0
                Dim currentColumn As Integer = 0
                For Each line As Match In lines
                    If line.Value.Length <> lineLength Then
                        Throw New InvalidOperationException("The line lengths are not consistent.")
                    End If
                    For Each character As Char In line.Value.ToCharArray()
                        grid(currentRow, currentColumn) = character
                        currentColumn += 1
                    Next
                    currentColumn = 0
                    currentRow += 1
                Next
            End If

            Dim wordCollection As MatchCollection = Regex.Matches(rawWords, "\S+")
            For Each word As Match In wordCollection
                words.Add(word.Value)
            Next
            If grid IsNot Nothing AndAlso words.Count > 0 Then
                returnValue = New Puzzle(grid, words)
            End If
        End If

        Return returnValue
    End Function

    Private Function ReadResource(name As String) As String
        Dim thisAssembly As Assembly = Assembly.GetExecutingAssembly()
        Dim returnValue As String = String.Empty

        Using stream As Stream = thisAssembly.GetManifestResourceStream("OmahaMTG.Challenge.Challenges." & name & ".txt")
            Using sr As New StreamReader(stream)
                returnValue = sr.ReadToEnd
            End Using
        End Using

        Return returnValue
    End Function
End Class
