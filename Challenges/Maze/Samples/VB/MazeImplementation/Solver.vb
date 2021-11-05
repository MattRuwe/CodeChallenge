Imports OmahaMTG.Challenge.Challenges

Public Class Solver
    Implements IMazeChallenge

    Public ReadOnly Property AuthorNotes As String Implements OmahaMTG.Challenge.ChallengeCommon.IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Private _lastMove As Direction

    Public Sub StartNewMaze(revealVicinity As System.Func(Of Integer, Maze)) Implements IMazeChallenge.StartNewMaze
        _lastMove = Direction.None
    End Sub

    Public Function MakeMove(cell As Cell, vicinity As Maze) As Direction Implements IMazeChallenge.MakeMove
        Dim returnValue As Direction = Direction.None

        'Determine the opposite move from the last so we know not to go back the way we came
        Dim oppositeMove As Direction = GetOppositeMove(_lastMove)

        'Generate a randomly ordered list of moves to try from the current cell
        Dim possibleMoves As New List(Of Direction) From {Direction.North, Direction.South, Direction.East, Direction.West}
        possibleMoves = possibleMoves.OrderBy(Function(val) Guid.NewGuid()).ToList()

        'For each of our randomly generated moves, see if we can go that direction
        For Each move As Direction In possibleMoves
            If cell.IsValidMove(move) AndAlso move <> oppositeMove Then
                'We have a winner!
                'We haven't been this way, and it is a valid move
                returnValue = move
                Exit For
            End If
        Next

        'If we didn't find a move that works, we need to go back the direction that we came
        If returnValue = Direction.None Then
            returnValue = oppositeMove
        End If

        'Record the last move so it can be used in the next iteration
        _lastMove = returnValue

        Return returnValue
    End Function

    ''' <summary>
    ''' Get the direction opposite of the one passed in
    ''' </summary>
    ''' <param name="d">The direction to get the opposite of</param>
    ''' <returns>The opposite direction of parameter d</returns>
    Private Function GetOppositeMove(d As Direction) As Direction
        Select Case d
            Case Direction.North
                Return Direction.South
            Case Direction.East
                Return Direction.West
            Case Direction.South
                Return Direction.North
            Case Direction.West
                Return Direction.East
            Case Else
                Return Direction.None
        End Select
    End Function
End Class
