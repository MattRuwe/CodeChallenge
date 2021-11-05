Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' The data structure containing the puzzle to be solved
''' </summary>
''' <remarks></remarks>
Public Class Puzzle
    Private ReadOnly _board As Char(,)
    Private ReadOnly _words As List(Of String)

    ''' <summary>
    ''' Creates a new puzzle instance
    ''' </summary>
    ''' <param name="board">A 2 dimensional array of characters containing the letters on the board</param>
    ''' <param name="words">A list of words that the entry is asked to find</param>
    ''' <remarks>The parameters for the contructor are assigned to <see cref="P:OmahaMTG.Challenge.Challenges.Puzzle.Board" /> and 
    ''' <see cref="P:OmahaMTG.Challenge.Challenges.Puzzle.Words respectively." /></remarks>
    Public Sub New(board(,) As Char, words As List(Of String))
        _board = board
        _words = words
    End Sub

    ''' <summary>
    ''' A 2 dimensional array of characters containing the letters A-Z on the board
    ''' </summary>
    ''' <returns>A 2 dimensional array of characters containing the letters A-Z (all uppercase).  This is the array that needs to be searched.  The first dimension represents the rows within the word search and the second
    ''' dimension represents the columns.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Board As Char(,)
        Get
            Return _board
        End Get
    End Property

    ''' <summary>
    ''' A list of words that exist within the board verticially, horizontally, and diagonally
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Words As List(Of String)
        Get
            Return _words
        End Get
    End Property

    ''' <summary>
    ''' Use this method to determine how well you solved the word search)
    ''' </summary>
    ''' <param name="foundWords">An enumerable list of FoundWord objects containing the location of all of the words that you found</param>
    ''' <returns>A PuzzleResults object containing the number of words successfully found and the number of words that were unmatched.</returns>
    ''' <remarks>This is the same method used by the execution engine to determine how well you solved the puzzle.  For an example, 
    ''' you should check the test harness included in the sample project.</remarks>
    Public Function GetResults(foundWords As IEnumerable(Of FoundWord)) As PuzzleResults
        Dim returnValue As New PuzzleResults
        Dim rowDirection As Short
        Dim columnDirection As Short

        Dim matchedWords As New List(Of FoundWord)
        Dim unmatchedWords As New List(Of FoundWord)

        Dim wordsProcessed As New List(Of String)

        'Parallel.ForEach(Of FoundWord)(foundWords,
        'Sub(foundWord As FoundWord)
        For Each foundWord As FoundWord In foundWords
            If Words.Contains(foundWord.Word) And Not wordsProcessed.Contains(foundWord.Word) Then
                If foundWord.StartingRow = foundWord.EndingRow AndAlso foundWord.StartingColumn < foundWord.EndingColumn Then
                    'Horizontal, left to right
                    rowDirection = 0
                    columnDirection = 1
                ElseIf foundWord.StartingRow < foundWord.EndingRow AndAlso foundWord.StartingColumn = foundWord.EndingColumn Then
                    ' vertical top to bottom
                    rowDirection = 1
                    columnDirection = 0
                ElseIf foundWord.StartingRow = foundWord.EndingRow AndAlso foundWord.StartingColumn > foundWord.EndingColumn Then
                    'Horizontal, Right to left
                    rowDirection = 0
                    columnDirection = -1
                ElseIf foundWord.StartingRow > foundWord.EndingRow AndAlso foundWord.StartingColumn = foundWord.EndingColumn Then
                    ' vertical bottom to top
                    rowDirection = -1
                    columnDirection = 0
                ElseIf foundWord.StartingRow > foundWord.EndingRow AndAlso foundWord.StartingColumn < foundWord.EndingColumn Then
                    'diagonal going up left to right
                    rowDirection = -1
                    columnDirection = 1
                ElseIf foundWord.StartingRow < foundWord.EndingRow AndAlso foundWord.StartingColumn < foundWord.EndingColumn Then
                    'diagonal going down left to right
                    rowDirection = 1
                    columnDirection = 1
                ElseIf foundWord.StartingRow > foundWord.EndingRow AndAlso foundWord.StartingColumn > foundWord.EndingColumn Then
                    ' diagonal going up right to left
                    rowDirection = -1
                    columnDirection = -1
                ElseIf foundWord.StartingRow < foundWord.EndingRow AndAlso foundWord.StartingColumn > foundWord.EndingColumn Then
                    ' diagonal going down right to left
                    rowDirection = 1
                    columnDirection = -1
                End If

                Dim currentRow As Integer = foundWord.StartingRow
                Dim currentColumn As Integer = foundWord.StartingColumn
                Dim wordChar() As Char = foundWord.Word.ToCharArray
                Dim wordMatched As Boolean = True
                'Loop for ever character in the word
                For i As Integer = 0 To foundWord.Word.Length - 1
                    If Board(currentRow, currentColumn) <> wordChar(i) Then
                        'The current character didn't match so there's no sense in continuing to look
                        wordMatched = False
                        Exit For
                    Else
                        'The current character matched, continue looking at the next character
                        currentColumn += columnDirection
                        currentRow += rowDirection
                    End If
                Next

                If wordMatched Then
                    matchedWords.Add(foundWord)
                    wordsProcessed.Add(foundWord.Word)
                Else
                    unmatchedWords.Add(foundWord)
                End If
            End If
        Next
        'End Sub)

        returnValue.MatchedWords = matchedWords
        returnValue.UnmatchedWords = unmatchedWords


        Return returnValue
    End Function

    Public Function Clone() As Puzzle
        Dim newBoard(_board.GetUpperBound(0), _board.GetUpperBound(1)) As Char
        Dim newWords As New List(Of String)

        For i As Integer = 0 To _board.GetUpperBound(0)
            For j As Integer = 0 To _board.GetUpperBound(1)
                newBoard(i, j) = _board(i, j)
            Next
        Next

        For Each word As String In _words
            newWords.Add(word)
        Next

        Dim returnValue As New Puzzle(newBoard, newWords)


        Return returnValue
    End Function

    ''' <summary>
    ''' Method used to create a new puzzle object, this should only be used for testing purposes
    ''' </summary>
    ''' <param name="size">The size of the word grid to generate.  The grid is always square (i.e. same height and width)</param>
    ''' <param name="wordCount">The number of words to randomly extract from the grid</param>
    ''' <returns>A new instance of a puzzle object</returns>
    ''' <remarks>This method should be used to generate a new puzzle that can be used to test your solver.  For an example, 
    ''' you should check the test harness included in the sample project</remarks>
    Public Shared Function CreatePuzzle(size As Integer, wordCount As Integer) As Puzzle
        Dim grid(size, size) As Char
        Dim rand As New Random
        Dim output As New StringBuilder

        For row As Integer = 0 To grid.GetUpperBound(0)
            For column As Integer = 0 To grid.GetUpperBound(1)
                grid(row, column) = ChrW(rand.Next(65, 91))
            Next
        Next

        For i As Integer = 0 To grid.GetUpperBound(0)
            For j As Integer = 0 To grid.GetUpperBound(1)
                output.Append(grid(i, j).ToString())
            Next
            output.AppendLine()
        Next
        output.AppendLine()

        Dim words As New List(Of String)

        'Loop once for every word
        For i As Integer = 0 To wordCount
            Dim row As Integer = rand.Next(0, grid.GetUpperBound(0) + 1)
            Dim column As Integer = rand.Next(0, grid.GetUpperBound(1) + 1)
            Dim direction As Integer = rand.Next(0, 8)
            Dim minLength As Integer = grid.GetUpperBound(0) * 0.1
            Dim maxLength As Integer = grid.GetUpperBound(0) * 0.25
            Dim length As Integer = rand.Next(minLength, maxLength + 1)
            Dim word() As Char
            ReDim word(length - 1)

            Select Case direction
                Case 0 ' horizontal left to right
                    If length + column > grid.GetUpperBound(1) Then
                        column = grid.GetUpperBound(1) - (length + 1)
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row, column + j)
                    Next
                Case 1 ' vertical top to bottom
                    If length + row > grid.GetUpperBound(0) Then
                        row = grid.GetUpperBound(0) - (length + 1)
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row + j, column)
                    Next
                Case 2 ' horizontal right to left
                    If column - length < 0 Then
                        column = length + 1
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row, column - j)
                    Next
                Case 3 ' vertical bottom to top
                    If row - length < 0 Then
                        row = length + 1
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row - j, column)
                    Next
                Case 4 ' diagonal going up left to right
                    If row - length < 0 Then
                        row = length + 1
                    End If

                    If column + length > grid.GetUpperBound(1) Then
                        column = grid.GetUpperBound(1) - (length + 1)
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row - j, column + j)
                    Next
                Case 5 ' diagonal going down left to right
                    If row + length > grid.GetUpperBound(0) Then
                        row = grid.GetUpperBound(0) - (length + 1)
                    End If

                    If column + length > grid.GetUpperBound(1) Then
                        column = grid.GetUpperBound(1) - (length + 1)
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row + j, column + j)
                    Next
                Case 6 ' diagonal going up right to left
                    If row - length < 0 Then
                        row = length + 1
                    End If

                    If column - length < 0 Then
                        column = (length + 1)
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row - j, column - j)
                    Next
                Case 7 ' diagonal going down right to left
                    If row + length > grid.GetUpperBound(0) Then
                        row = grid.GetUpperBound(0) - (length + 1)
                    End If

                    If column - length < 0 Then
                        column = (length + 1)
                    End If

                    For j As Integer = 0 To length - 1
                        word(j) = grid(row + j, column - j)
                    Next
            End Select

            words.Add(New String(word))
        Next

        For Each word As String In words
            output.AppendLine(word)
        Next

        Debug.WriteLine(output.ToString())

        Return New Puzzle(grid, words)

    End Function
End Class

