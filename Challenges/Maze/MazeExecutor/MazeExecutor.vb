Imports System.IO
Imports OmahaMTG.Challenge.ExecutionCommon
Imports OmahaMTG.Challenge.Challenges
Imports System.Reflection
Imports System.Text

Public Class MazeExecutor
    Inherits ChallengeExecutorBase(Of IMazeChallenge)

    Protected Overrides Sub RunChallengeOverride()
        If IsTest Then
            Console.WriteLine("Running Test Maze #1")
            Dim currentMaze As New Maze(50)
            currentMaze.GenerateMaze(100)
            RunSingleChallenge(currentMaze)

            Console.WriteLine("Running Test Maze #2")
            currentMaze = New Maze(100)
            currentMaze.GenerateMaze(100)
            RunSingleChallenge(currentMaze)

            Console.WriteLine("Running Test Maze #3")
            currentMaze = New Maze(500)
            currentMaze.GenerateMaze(100)
            RunSingleChallenge(currentMaze)
        Else
            Dim currentMaze As Maze
            Dim mazeChoice = Enumerable.Range(1, 7)
            mazeChoice = mazeChoice.OrderBy(Function(val) Guid.NewGuid)

            For Each val As Integer In mazeChoice
                currentMaze = Nothing
                Select Case val
                    Case 1
                        currentMaze = GetMaze1()
                    Case 2
                        currentMaze = GetMaze2()
                    Case 3
                        currentMaze = GetMaze3()
                    Case 4
                        currentMaze = GetMaze4()
                    Case 5
                        currentMaze = GetMaze5()
                    Case 6
                        currentMaze = GetMaze6()
                    Case 7
                        currentMaze = GetMaze7()
                    Case Else
                        currentMaze = Nothing
                End Select

                If currentMaze IsNot Nothing Then
                    Console.WriteLine("Using Maze #{0} - {1}x{1}", val.ToString(), currentMaze.GetSize())

                    RunSingleChallenge(currentMaze)
                End If
            Next
        End If
    End Sub

    Private Sub RunSingleChallenge(maze As Maze)
        Dim moves As Integer = 0
        Dim maxMoves As Long = (Math.Pow(maze.GetSize, 2) * 2)
        Dim playerMove As Direction
        Dim madeAnIllegalMove As Boolean = False
        Dim entryException As Exception = Nothing

        Dim stopWatch As Stopwatch = stopWatch.StartNew
        Dim revealPointsLost As Integer = 0
        Dim detailedOutput As New StringBuilder

        detailedOutput.AppendLine("|Move Number | Current Cell Index | Player Move | Elapsed Time |")
        detailedOutput.AppendLine("|------------|--------------------|-------------|--------------|")

        Dim totalNorthMoves, totalEastMoves, totalSouthMoves, totalWestMoves, totalRevealVicinityCalls As Integer

        Challenge.StartNewMaze(Function(size As Integer) As Maze
                                   'This function is used when the player needs a hint
                                   If size > maze.GetSize() OrElse size < 3 Then
                                       Return Nothing
                                   End If
                                   If size Mod 2 <> 1 Then
                                       'The size must be an odd number
                                       size -= 1
                                   End If

                                   'If the user reveals the entire maze all at once, they lose all of their potential points.
                                   revealPointsLost += Math.Max(Math.Pow(10, size / maze.GetSize() * 8), 500)
                                   totalRevealVicinityCalls += 1
                                   Return maze.GetSubMaze(maze.CurrentIndex, size)
                               End Function)

        While Not maze.CurrentCell.IsEnd AndAlso moves <= maxMoves
            Try
                playerMove = Challenge.MakeMove(maze.CurrentCell,
                                                maze.GetSubMaze(maze.CurrentIndex, 3))
            Catch ex As Exception
                entryException = ex
                Exit While
            End Try

            Select Case playerMove
                Case Direction.North
                    totalNorthMoves += 1
                Case Direction.East
                    totalEastMoves += 1
                Case Direction.South
                    totalSouthMoves += 1
                Case Direction.West
                    totalWestMoves += 1
            End Select

            detailedOutput.AppendFormat("|{0} |{1} |{2} |{3} |", moves.ToString("N0").PadLeft(11, " "c), maze.CurrentIndex.ToString.PadLeft(19, " "c), playerMove.ToString().PadLeft(12, " "c), stopWatch.ElapsedMilliseconds.ToString("N0").PadLeft(13, " "c))
            detailedOutput.AppendLine()

            Try
                maze.MoveCurrentCell(playerMove)
                moves += 1
            Catch ex As IllegalMoveException
                madeAnIllegalMove = True
                Exit While
            End Try
        End While

        stopWatch.Stop()

        Dim result As New ChallengeResult()
        result.DurationTicks = stopWatch.Elapsed.Ticks
        If madeAnIllegalMove Then
            Console.WriteLine("An illegal move was attempted")
            result.ResultMessage = "The entry made an illegal move (i.e. it tried to move through a wall or made an unknown move)"
            result.Score = 0
            result.Successful = False
            detailedOutput.AppendLine("|------------|--------------------|-------------|--------------|")
            detailedOutput.AppendLine("| *ILLEGAL MOVE ATTEMPTED* (i.e. tried to move through a wall) |")
        ElseIf moves > maxMoves Then
            Console.WriteLine("*** The entry exceeded the maximum number of moves {0}", maxMoves.ToString("N0"))
            result.ResultMessage = "Your entry exceeded the maximum number of moves."
            result.Score = 0
            result.Successful = False
            detailedOutput.AppendLine("|------------|--------------------|-------------|--------------|")
            detailedOutput.AppendLine(String.Format("|***  The entry exceeded the maximum number of moves ({0}) *** |", maxMoves.ToString("N0")))
        ElseIf entryException IsNot Nothing Then
            Console.WriteLine("The entry threw an exception: {0}", entryException.ToString())
            result.ResultMessage = "The entry threw an exception during processing."
            result.Score = 0
            result.Successful = False
            detailedOutput.AppendLine("|------------|--------------------|-------------|--------------|")
            detailedOutput.AppendLine("|       *** Exception occurred during entry execution ***      |")
            detailedOutput.AppendLine(entryException.ToString())
        Else
            detailedOutput.AppendLine("|------------|--------------------|-------------|--------------|")
            detailedOutput.AppendLine("|                                                              |")
            If maze.CurrentCell.IsEnd Then
                result.ResultMessage = String.Format("The maze was successfully solved in {0} moves", moves.ToString("N0"))
                result.Score = Math.Max(100000 - (100000 * (moves / (Math.Pow(maze.GetSize, 2) * 2))) - (stopWatch.ElapsedMilliseconds * 2) - revealPointsLost, 0) + If(maze.CurrentCell.IsEnd, 5000, 0)
                result.Successful = True

                detailedOutput.AppendLine("|           *** Maze was successfully completed ***            |")
            Else
                result.ResultMessage = String.Format("The maze was not successfully completed")
                result.Score = 0
                result.Successful = False
                detailedOutput.AppendLine("|         *** Maze was NOT successfully completed ***          |")
            End If

        End If

        detailedOutput.AppendLine("|                                                              |")
        detailedOutput.AppendLine("|------------|--------------------|-------------|--------------|")
        detailedOutput.AppendLine("|Summary:                                                      |")
        detailedOutput.AppendFormat("|--Max Moves:                     {0}|{1}", maxMoves.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Total Moves:                   {0}|{1}", moves.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Total Elapsed Millisecond:     {0}|{1}", stopWatch.ElapsedMilliseconds.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Reveal Vicinity Calls:         {0}|{1}", totalRevealVicinityCalls.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Reveal Vicinity Points Lost:   {0}|{1}", revealPointsLost.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Total North Moves:             {0}|{1}", totalNorthMoves.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Total East Moves:              {0}|{1}", totalEastMoves.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Total South Moves:             {0}|{1}", totalSouthMoves.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Total West Moves:              {0}|{1}", totalWestMoves.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendFormat("|--Final Score:                   {0}|{1}", result.Score.ToString("N0").PadRight(29, " "c), vbCrLf)
        detailedOutput.AppendLine("|--------------------------------------------------------------|")

        If IsTest Then
            Using imageStream As New MemoryStream
                maze.SaveMazeImage(imageStream)
                imageStream.Position = 0
                result.TestResults.Add(New FileResult With {
                                       .Contents = imageStream.ToArray(),
                                       .Filename = "Maze.png"
                                   })
            End Using
            result.TestResults.Add(New FileResult With
                                   {
                                       .Contents = Encoding.UTF8.GetBytes(detailedOutput.ToString()),
                                       .Filename = "log.txt"
                                   })

        End If

        ResultsAvailable(result)
    End Sub

    Protected Overrides ReadOnly Property MaxAuthorNotesLength() As Integer
        Get
            Return 10
        End Get
    End Property

    Private Function GetMaze1() As Maze
        Dim serializedMaze As String = ReadResource("Maze1")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GetMaze2() As Maze
        Dim serializedMaze As String = ReadResource("Maze2")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GetMaze3() As Maze
        Dim serializedMaze As String = ReadResource("Maze3")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GetMaze4() As Maze
        Dim serializedMaze As String = ReadResource("Maze4")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GetMaze5() As Maze
        Dim serializedMaze As String = ReadResource("Maze5")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GetMaze6() As Maze
        Dim serializedMaze As String = ReadResource("Maze6")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GetMaze7() As Maze
        Dim serializedMaze As String = ReadResource("Maze7")
        Return GenerateMazeFromSerializedData(serializedMaze)
    End Function

    Private Function GenerateMazeFromSerializedData(serializedMaze As String) As Maze
        Dim returnValue As New Maze(serializedMaze)

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
