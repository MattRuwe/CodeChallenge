Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.Challenges
Public Class MazeImplementation
    Implements IMazeChallenge

    Private ReadOnly _moves As Stack(Of Direction)
    Private ReadOnly _rand As Random
    Private _numberOfMoves As Integer

    Public Sub New()
        _moves = New Stack(Of Direction)
        _rand = New Random()
        _numberOfMoves = 0
    End Sub

    Public ReadOnly Property AuthorNotes As String Implements IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Public Sub StartNewMaze(retrieveVicinity As Func(Of Integer, Maze)) Implements Challenges.IMazeChallenge.StartNewMaze
        _moves.Clear()
        _numberOfMoves = 0
    End Sub

    Public Function MakeMove(cell As Cell, vicinity As Maze) As Direction Implements IMazeChallenge.MakeMove
        Dim returnValue As Direction

        Dim possibleDirections As New List(Of Direction)
        Dim tempCell As Cell
        If Not cell.NorthSolution AndAlso Not cell.NorthWall Then
            tempCell = vicinity.GetCell(New CellIndex(vicinity.CurrentIndex.Row - 1, vicinity.CurrentIndex.Column))
            If tempCell.IsEnd OrElse Not tempCell.NorthWall OrElse Not tempCell.EastWall OrElse Not tempCell.WestWall Then
                possibleDirections.Add(Direction.North)
            End If
        End If

        If Not cell.EastSolution AndAlso Not cell.EastWall Then
            tempCell = vicinity.GetCell(New CellIndex(vicinity.CurrentIndex.Row, vicinity.CurrentIndex.Column + 1))
            If tempCell.IsEnd OrElse Not tempCell.NorthWall OrElse Not tempCell.EastWall OrElse Not tempCell.SouthWall Then
                possibleDirections.Add(Direction.East)
            End If

        End If

        If Not cell.SouthSolution AndAlso Not cell.SouthWall Then
            tempCell = vicinity.GetCell(New CellIndex(vicinity.CurrentIndex.Row + 1, vicinity.CurrentIndex.Column))
            If tempCell.IsEnd OrElse Not tempCell.EastWall OrElse Not tempCell.SouthWall OrElse Not tempCell.WestWall Then
                possibleDirections.Add(Direction.South)
            End If
        End If

        If Not cell.WestSolution AndAlso Not cell.WestWall Then
            tempCell = vicinity.GetCell(New CellIndex(vicinity.CurrentIndex.Row, vicinity.CurrentIndex.Column - 1))
            If tempCell.IsEnd OrElse Not tempCell.NorthWall OrElse Not tempCell.SouthWall OrElse Not tempCell.WestWall Then
                possibleDirections.Add(Direction.West)
            End If
        End If

        If possibleDirections.Count > 0 Then
            returnValue = possibleDirections(_rand.Next(0, possibleDirections.Count))
            _moves.Push(returnValue)
        Else
            returnValue = GetOppositeMove(_moves.Pop)
        End If

        _numberOfMoves += 1

        Return returnValue
    End Function

    Private Function GetOppositeMove(direction As Direction) As Direction
        Select Case direction
            Case Challenges.Direction.North
                Return Challenges.Direction.South
            Case Challenges.Direction.East
                Return Challenges.Direction.West
            Case Challenges.Direction.South
                Return Challenges.Direction.North
            Case Challenges.Direction.West
                Return Challenges.Direction.East
            Case Else
                Return Challenges.Direction.None
        End Select
    End Function
End Class
