Imports System.Text
Imports OmahaMTG.Challenge.Challenges
Imports System.Xml.Serialization
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports OmahaMTG.Challenge.MazeImplementation

<TestClass()>
Public Class MazeTest

    <TestMethod()>
    Public Sub TestMethod1()
        Dim maze As Maze

        Dim serializedFilePath As String = "c:\temp\Maze.txt"
        'Dim serializedFilePath As String = "E:\Development\OmahaMTG\Development\Tools\CodeChallenge\Challenges\Maze\MazeExecutor\Maze1.txt"

        File.Delete(serializedFilePath)

        If File.Exists(serializedFilePath) Then
            Using fs As New StreamReader(serializedFilePath)
                maze = New Maze(fs.ReadToEnd())
            End Using
        Else
            maze = New Maze(500)
            maze.GenerateMaze(100)
            Using fs As New StreamWriter(serializedFilePath, False)
                fs.Write(maze.GetSerializedMaze())
            End Using
        End If

        maze.SaveMazeImage("c:\temp\test.bmp")

    End Sub

    <TestMethod()>
    Public Sub SerializationTest()
        Dim boardSize As Integer = 1000

        Dim originalMaze As New Maze(boardSize)
        originalMaze.GenerateMaze(100)


        Dim serializedMaze As String = originalMaze.GetSerializedMaze()

        Dim newMaze = New Maze(serializedMaze)
        For row As Integer = 0 To boardSize
            For col As Integer = 0 To boardSize
                Dim originalCell As Cell = originalMaze.GetCell(New CellIndex(row, col))
                Dim newCell As Cell = newMaze.GetCell(New CellIndex(row, col))

                Assert.AreEqual(originalCell.CellValue, newCell.CellValue)
            Next
        Next
    End Sub

    <TestMethod()>
    Public Sub FogOfWarTest()
        Dim maze As New Maze(100)
        maze.GenerateMaze(100)

        Dim foggedMaze As Maze = maze.GetSubMaze(New CellIndex(0, 0), 5)

        Assert.AreEqual(maze.GetCell(New CellIndex(2, 2)).CellValue, foggedMaze.GetCell(New CellIndex(2, 2)).CellValue)

    End Sub

    <TestMethod()>
    Public Sub GenerateExecutorMazes()
        Dim sizes() As Integer = New Integer(6) {5, 50, 100, 250, 500, 1000, 1500}
        Dim count As Integer = 0
        Dim mazeFolder As String = "E:\Development\OmahaMTG\Development\Tools\CodeChallenge\Challenges\Maze\MazeExecutor\"
        For Each size As Integer In sizes
            count += 1
            Dim mazeFilname = Path.Combine(mazeFolder, "maze" & count & ".txt")
            Dim maze As New Maze(size)
            maze.GenerateMaze(100)
            Using tw As New StreamWriter(mazeFilname)
                tw.Write(maze.GetSerializedMaze())
            End Using
        Next

    End Sub

    <TestMethod()>
    Public Sub SolveMaze()
        For i As Integer = 0 To 100


            Dim maze As New Maze(100)

            maze.GenerateMaze(100)

            Dim solver As New MazeImplementation

            Try
                While Not maze.CurrentCell.IsEnd
                    maze.MoveCurrentCell(solver.MakeMove(maze.CurrentCell, maze.GetSubMaze(maze.CurrentIndex, 5)))
                End While

                maze.SaveMazeImage(String.Format("c:\temp\solution{0}.bmp", i))
            Catch
                maze.SaveMazeImage(String.Format("c:\temp\error{0}.bmp", i))
            End Try


        Next
    End Sub

End Class
