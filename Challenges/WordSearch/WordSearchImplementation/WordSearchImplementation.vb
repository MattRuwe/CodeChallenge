Imports OmahaMTG.Challenge.Challenges
Imports System.Threading.Tasks

Public Class WordSearchImplementation
    Implements IWordSearchChallenge

    Public ReadOnly Property AuthorNotes As String Implements OmahaMTG.Challenge.ChallengeCommon.IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Public Function SolveWordSearch(puzzle As Puzzle) As IEnumerable(Of FoundWord) Implements IWordSearchChallenge.SolveWordSearch
        Dim returnValue As New List(Of FoundWord)

        'This is a sample implementation, which is intentionally unoptimized

        Parallel.ForEach(Of String)(puzzle.Words,
                Sub(word As String, state As ParallelLoopState)
                    Dim foundWord As Boolean = False
                    For row As Integer = 0 To puzzle.Board.GetUpperBound(0)
                        For column As Integer = 0 To puzzle.Board.GetUpperBound(1)
                            If ExtractString(puzzle.Board, 0, 1, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row,
                                                .EndingColumn = column + word.Length})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, 0, -1, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row,
                                                .EndingColumn = column - word.Length})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, 1, 0, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row + word.Length,
                                                .EndingColumn = column})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, -1, 0, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row - word.Length,
                                                .EndingColumn = column})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, 1, 1, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row + word.Length,
                                                .EndingColumn = column + word.Length})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, -1, 1, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row - word.Length,
                                                .EndingColumn = column + word.Length})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, 1, -1, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row + word.Length,
                                                .EndingColumn = column - word.Length})
                                foundWord = True
                                Exit For
                            ElseIf ExtractString(puzzle.Board, -1, -1, word, row, column, word.Length) = word Then
                                returnValue.Add(New FoundWord() With {
                                                .Word = word,
                                                .StartingRow = row,
                                                .StartingColumn = column,
                                                .EndingRow = row - word.Length,
                                                .EndingColumn = column - word.Length})
                                foundWord = True
                                Exit For
                            End If
                        Next
                        If foundWord Then
                            Exit For
                        End If
                    Next
                End Sub)
        Return returnValue
    End Function

    Private Function ExtractString(board As Char(,), rowDirection As Integer, columnDirection As Integer, word As String, startRow As Integer, startColumn As Integer, length As Integer) As String
        Dim returnValue As String = String.Empty

        Dim endingColumn As Integer = startColumn + (length * columnDirection)
        Dim endingRow As Integer = startRow + (length * rowDirection)


        If endingColumn >= 0 AndAlso endingColumn <= board.GetUpperBound(1) AndAlso endingRow >= 0 AndAlso endingRow <= board.GetUpperBound(0) Then



            For i As Integer = 0 To length - 1
                If startColumn + (i * columnDirection) > board.GetUpperBound(1) OrElse startColumn + (i * columnDirection) < 0 Then
                    Exit For
                End If
                If startRow + (i * rowDirection) > board.GetUpperBound(0) OrElse startRow + (i * rowDirection) < 0 Then
                    Exit For
                End If
                returnValue += board(startRow + (i * rowDirection), startColumn + (i * columnDirection))
                If returnValue <> word.Substring(0, returnValue.Length) Then
                    Exit For
                End If
            Next
        End If
        Return returnValue
    End Function

End Class
