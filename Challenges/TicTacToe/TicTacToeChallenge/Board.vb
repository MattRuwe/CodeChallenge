Imports System.Text
Imports OmahaMTG.Challenge.ChallengeCommon

Public Class Board
    Implements ITurnEnvironment
    Private _boardSize As Integer
    Private _currentPlayer As Player = Player.None
    Private _board As Player()
    Private _random As New Random(10)
    Private _history As New Stack(Of Integer)

    Private Sub New()

    End Sub

    Public Sub New(boardSize As Integer)
        _boardSize = boardSize
        ReDim _board(Math.Pow(boardSize, 2) - 1)
    End Sub

    Public Function Clone() As Board
        Return New Board With
               {
                   ._currentPlayer = _currentPlayer,
                   ._board = _board.Clone(),
                   ._boardSize = _boardSize,
                   ._random = _random,
                   ._history = New Stack(Of Integer)(_history.ToArray())
               }
    End Function

    Public Property Board() As Player()
        Get
            Return _board
        End Get
        Set(value As Player())
            _board = value
        End Set
    End Property

    Public ReadOnly Property CurrentPlayer As Player

        Get
            If _currentPlayer = Player.None Then
                _currentPlayer = Player.X
            End If

            Return _currentPlayer
        End Get
    End Property

    Public Sub MakeMove(index As BoardIndex)
        If GetPlayerAtIndex(index) <> Player.None Then
            Throw New InvalidOperationException(String.Format("Invalid move, that space ({0}x{1}) is already occupied", index.Row, index.Column))
        End If

        Dim bi As Integer = GetIndexFromBoardIndex(index)
        _board(bi) = CurrentPlayer
        _history.Push(bi)
        If CurrentPlayer = Player.X Then
            _currentPlayer = Player.O
        Else
            _currentPlayer = Player.X
        End If
    End Sub

    Public Sub UndoLastMove(removeFromHistory As Boolean)
        If _history.Count > 0 Then
            Dim lastMove As Integer = If(removeFromHistory, _history.Pop(), _history.Peek)
            _board(lastMove) = Player.None
        End If
    End Sub

    Public ReadOnly Property UndoMoveCount() As Integer
        Get
            Return _history.Count
        End Get
    End Property

    Public Function GetPlayerAtIndex(boardIndex As BoardIndex) As Player
        Dim arrayIndex As Integer = GetIndexFromBoardIndex(boardIndex)


        Return _board(arrayIndex)

    End Function

    Public Function GetRandomMove() As BoardIndex
        Dim availableIndexes() As BoardIndex = GetAllAvailableBoardIndexes()

        If availableIndexes.Count > 0 Then
            Return availableIndexes.OrderBy(Function(val) Guid.NewGuid)(0)
        Else
            Return New BoardIndex With {.Row = -1, .Column = -1}
        End If
    End Function

    Public Function GetAllAvailableBoardIndexes() As BoardIndex()
        Dim returnValue As New List(Of BoardIndex)
        For i As Integer = 0 To _board.GetUpperBound(0)
            If _board(i) = Player.None AndAlso Not _history.Contains(i) Then
                returnValue.Add(GetBoardIndexFromIndex(i))
            End If
        Next

        Return returnValue.OrderBy(Function(val) Guid.NewGuid).ToArray()
    End Function

    Public Function GetAvailableBoardIndex(startingBoardIndex As BoardIndex) As BoardIndex
        Dim returnValue As New BoardIndex With {.Row = -1, .Column = -1}
        Dim startingIndex As Integer = GetIndexFromBoardIndex(startingBoardIndex)

        For i As Integer = startingIndex To _board.GetUpperBound(0)
            If Board(i) = Player.None Then
                returnValue = GetBoardIndexFromIndex(i)
                Exit For
            End If
        Next

        Return returnValue
    End Function

    Public Function IncrementBoardIndex(startingBoardIndex As BoardIndex) As BoardIndex
        Return GetBoardIndexFromIndex(GetIndexFromBoardIndex(startingBoardIndex) + 1)
    End Function

    Private Function GetIndexFromBoardIndex(boardIndex As BoardIndex) As Integer
        Return (_boardSize * boardIndex.Row) + boardIndex.Column
    End Function

    Private Function GetBoardIndexFromIndex(index As Integer) As BoardIndex
        Dim returnValue As New BoardIndex With
            {
                .Row = index \ _boardSize,
                .Column = index Mod _boardSize
            }

        Return returnValue
    End Function

    Public Function GetGameStatus() As GameStatus
        Dim returnValue As GameStatus = GameStatus.InProgress

        For Each p In {Player.X, Player.O}
            Dim slice(_boardSize - 1) As Player

            For i As Integer = 0 To _boardSize - 1
                'Check the rows
                Array.Copy(_board, i * _boardSize, slice, 0, _boardSize)
                If CheckWinner(slice, p, returnValue) Then
                    Return returnValue
                End If

                'Check the columns
                For j As Integer = 0 To _boardSize - 1
                    slice(j) = _board(j * _boardSize + i)
                Next
                If CheckWinner(slice, p, returnValue) Then
                    Return returnValue
                End If
            Next

            'Check the left to right diagonal
            For i As Integer = 0 To _boardSize - 1
                slice(i) = _board(i * _boardSize + i)
            Next
            If CheckWinner(slice, p, returnValue) Then
                Return returnValue
            End If

            'Check the right to left diagonal
            For i As Integer = 0 To _boardSize - 1
                slice(i) = _board(i * _boardSize + (_boardSize - (i + 1)))
            Next
            If CheckWinner(slice, p, returnValue) Then
                Return returnValue
            End If
        Next

        If _board.Any(Function(p) p = Player.None) Then
            Return GameStatus.InProgress
        Else
            Return GameStatus.Draw
        End If
    End Function

    Private Function CheckWinner(slice As Player(), p As Player, ByRef returnValue As GameStatus) As Boolean
        returnValue = CType(Nothing, GameStatus)
        If slice.Sum(Function(v) CType(v, Integer)) = _boardSize * CType(p, Integer) Then
            If p = Player.X Then
                returnValue = GameStatus.XWins
                Return True
            ElseIf p = Player.O Then
                returnValue = GameStatus.OWins
                Return True
            End If
        End If
        Return False
    End Function

    Public Overrides Function ToString() As String
        Dim output As New StringBuilder
        For i As Integer = 0 To _board.GetUpperBound(0)
            If i > 0 And (i Mod _boardSize) = 0 Then
                output.AppendLine()
                output.AppendLine(New String("---", _boardSize * 4 - 1))
            End If

            Select Case _board(i)
                Case Player.None
                    output.Append(" - ")
                Case Player.O
                    output.Append(" O ")
                Case Player.X
                    output.Append(" X ")
            End Select

            If ((i + 1) Mod _boardSize) > 0 Then
                output.Append("|")


            End If
        Next

        Return output.ToString()
    End Function
End Class