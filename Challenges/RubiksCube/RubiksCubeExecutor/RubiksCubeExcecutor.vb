Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.ExecutionCommon

<Serializable()>
Public Class RubiksCubeExcecutor
    Inherits ChallengeExecutorBase(Of IRubiksCubeChallenge)

    Protected Overrides Sub RunChallengeoverride()
        Dim scrambleMoves As IEnumerable(Of RubiksMove)

        Dim result As ChallengeResult

        Dim cubeChoice = Enumerable.Range(1, 5)
        cubeChoice = cubeChoice.OrderBy(Function(val) Guid.NewGuid())

        For Each val As Integer In cubeChoice
            Select Case val
                Case 1
                    Console.WriteLine("Retrieving cube #1")
                    scrambleMoves = GetScrambleMoves1()
                Case 2
                    Console.WriteLine("Retrieving cube #2")
                    scrambleMoves = GetScrambleMoves2()
                Case 3
                    Console.WriteLine("Retrieving cube #3")
                    scrambleMoves = GetScrambleMoves3()
                Case 4
                    Console.WriteLine("Retrieving cube #4")
                    scrambleMoves = GetScrambleMoves4()
                Case 5
                    Console.WriteLine("Retrieving cube #5")
                    scrambleMoves = GetScrambleMoves5()
                Case Else
                    Console.WriteLine("Unknown cube!")
                    scrambleMoves = Nothing
            End Select

            If scrambleMoves IsNot Nothing Then
                Console.WriteLine("Running entry")
                result = RunSingleChallenge(scrambleMoves)
                Console.WriteLine("Finished running cube, the result was {0}", If(result.Successful, "successful", "unsuccessful"))
                ResultsAvailable(result)
            Else
                Console.WriteLine("The scamble moves were not available")
            End If
        Next
    End Sub

    Private Function GetScore(ByVal movesMade As Integer, ByVal millisecondsElapsed As Integer, ByVal solved As Boolean)
        Dim returnValue As Integer = 100000

        If solved Then
            returnValue -= movesMade * 1500
            returnValue -= millisecondsElapsed
        Else
            returnValue = 0
        End If

        If returnValue < 0 Then
            returnValue = 0
        End If

        Return returnValue
    End Function

    Private Function RunSingleChallenge(ByVal scrambleMoves As IEnumerable(Of RubiksMove)) As ChallengeResult
        Dim returnValue As New ChallengeResult
        Dim cube As New RubiksCube

        Console.WriteLine("Applying moves")
        cube.RecordHistory = False
        For Each move As RubiksMove In scrambleMoves
            cube.MakeMove(move)
        Next
        cube.RecordHistory = True
        Console.WriteLine("Finished applying moves")

        Dim stopWatch As Stopwatch = stopWatch.StartNew()
        Console.WriteLine("Running entry")
        Dim solveSteps As IEnumerable(Of RubiksMove) = Challenge.SolveCube(cube.Clone)
        Console.WriteLine("Finished running entry")
        If solveSteps IsNot Nothing Then
            solveSteps = solveSteps.ToList()
        End If
        stopWatch.Stop()
        returnValue.DurationTicks = stopWatch.Elapsed.Ticks

        Console.WriteLine("Playing entry moves")
        If solveSteps IsNot Nothing Then
            For Each move As RubiksMove In solveSteps
                If Not IsValidMove(move) Then
                    returnValue.DisplayError = "An invalid move was specified."
                    Exit For
                End If

                cube.MakeMove(move)
            Next
        Else
            solveSteps = New List(Of RubiksMove)
        End If
        Console.WriteLine("Finished playing entry moves")

        If cube.IsSolved() Then
            returnValue.Successful = True
            'returnValue.ResultMessage = String.Format("The cube was successfully solved in {0} moves and {1} milliseconds", solveSteps.Count, TimeSpan.FromTicks(returnValue.DurationTicks).TotalMilliseconds)
            returnValue.ResultMessage = "The cube was successfully solved"
            Console.WriteLine("The cube was successfully solved in {0} moves and {1} milliseconds", solveSteps.Count, TimeSpan.FromTicks(returnValue.DurationTicks).TotalMilliseconds)
        Else
            returnValue.Successful = False
            'returnValue.ResultMessage = String.Format("The cube was NOT solved in {0} move(s) and {1} milliseconds", solveSteps.Count, TimeSpan.FromTicks(returnValue.DurationTicks).TotalMilliseconds)
            returnValue.ResultMessage = "The cube was NOT solved."
            Console.WriteLine("The cube was NOT solved in {0} move(s) and {1} milliseconds", solveSteps.Count, TimeSpan.FromTicks(returnValue.DurationTicks).TotalMilliseconds)
        End If

        returnValue.Score = GetScore(solveSteps.Count, TimeSpan.FromTicks(returnValue.DurationTicks).TotalMilliseconds, cube.IsSolved)

        Console.WriteLine("Final score for cube was {0}", returnValue.Score)

        Return returnValue
    End Function

    Private Function IsValidMove(ByVal move As RubiksMove)
        Dim returnValue As Boolean

        returnValue = [Enum].GetNames(GetType(RubiksMove)).Contains([Enum].GetName(GetType(RubiksMove), move))

        Return returnValue
    End Function

    Protected Overrides ReadOnly Property MaxAuthorNotesLength As Integer
        Get
            Return 0
        End Get
    End Property

    Private Function GetScrambleMoves1() As IEnumerable(Of RubiksMove)
        Dim returnValue = New List(Of RubiksMove) From {
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.RightCounterClockwise}

        Return returnValue
    End Function

    Private Function GetScrambleMoves2() As IEnumerable(Of RubiksMove)
        Dim returnValue = New List(Of RubiksMove) From {RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BackClockwise}

        Return returnValue
    End Function

    Private Function GetScrambleMoves3() As IEnumerable(Of RubiksMove)
        Dim returnValue = New List(Of RubiksMove) From {
            RubiksMove.BackClockwise, RubiksMove.BottomCounterClockwise, RubiksMove.LeftClockwise, RubiksMove.FrontClockwise}
        Return returnValue

    End Function

    Private Function GetScrambleMoves4() As IEnumerable(Of RubiksMove)
        Dim returnValue As New List(Of RubiksMove) From {RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.FrontClockwise}

        Return returnValue
    End Function

    Private Function GetScrambleMoves5() As IEnumerable(Of RubiksMove)
        Dim returnValue As New List(Of RubiksMove) From {
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.TopBottomAxisCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.LeftClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.FrontClockwise,
                            RubiksMove.BottomCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.BackCounterClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.RightClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.TopCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.FrontBackAxisClockwise,
                            RubiksMove.FrontBackAxisCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.FrontCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.TopClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.LeftCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.TopBottomAxisClockwise,
                            RubiksMove.BottomClockwise,
                            RubiksMove.BackClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftRightAxisClockwise,
                            RubiksMove.RightCounterClockwise,
                            RubiksMove.LeftRightAxisCounterClockwise,
                            RubiksMove.BackClockwise}

        Return returnValue
    End Function
End Class
