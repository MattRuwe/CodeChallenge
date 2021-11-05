Imports System.Text
Imports OmahaMTG.Challenge.Challenges
Imports OmahaMTG.Sample.MazeImplementation

<TestClass()>
Public Class SolverTest

    <TestMethod()>
    Public Sub MakeMoveTest()
        Dim moves As Integer = 0
        Dim maze As New Maze(100)
        maze.GenerateMaze(100)
        Dim solver As New Solver
        Dim playerMove As Direction
        Dim madeAnIllegalMove As Boolean = False
        Dim maxMoves As Long = (Math.Pow(maze.GetSize, 2) * 2)

        solver.StartNewMaze(Function(size As Integer) As Maze
                                Return maze.GetSubMaze(maze.CurrentIndex, size)
                            End Function)

        While Not maze.CurrentCell.IsEnd AndAlso moves <= maxMoves
            playerMove = solver.MakeMove(maze.CurrentCell, maze.GetSubMaze(maze.CurrentIndex, 3))

            Try
                maze.MoveCurrentCell(playerMove)
                moves += 1
            Catch ex As IllegalMoveException
                madeAnIllegalMove = True
                Exit While
            End Try
        End While

        Assert.IsTrue(maze.CurrentCell.IsEnd)
        Assert.IsFalse(madeAnIllegalMove)
    End Sub

End Class
