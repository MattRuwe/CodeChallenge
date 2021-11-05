Imports OmahaMTG.Challenge.ExecutionCommon
Imports OmahaMTG.Challenge.GuessTheNumberChallenge
Imports System.Text

Public Class GuessNumberExecutor
    Inherits ChallengeExecutorBase(Of IGuessTheNumberChallenge)

    Protected Overrides Sub RunChallengeOverride()
        Dim currentSequence As List(Of Integer)
        Dim sequenceChoice
        If Not IsTest Then
            sequenceChoice = Enumerable.Range(1, 25).OrderBy(Function(val) Guid.NewGuid)
        Else
            sequenceChoice = New List(Of Integer) From {1, 5, 17}
        End If

        For Each val As Integer In sequenceChoice
            Select Case val
                Case 1
                    currentSequence = Sequence1
                Case 2
                    currentSequence = Sequence2
                Case 3
                    currentSequence = Sequence3
                Case 4
                    currentSequence = Sequence4
                Case 5
                    currentSequence = Sequence5
                Case 6
                    currentSequence = Sequence6
                Case 7
                    currentSequence = Sequence7
                Case 8
                    currentSequence = Sequence8
                Case 9
                    currentSequence = Sequence9
                Case 10
                    currentSequence = Sequence10
                Case 11
                    currentSequence = Sequence11
                Case 12
                    currentSequence = Sequence12
                Case 13
                    currentSequence = Sequence13
                Case 14
                    currentSequence = Sequence14
                Case 15
                    currentSequence = Sequence15
                Case 16
                    currentSequence = Sequence16
                Case 17
                    currentSequence = Sequence17
                Case 18
                    currentSequence = Sequence18
                Case 19
                    currentSequence = Sequence19
                Case 20
                    currentSequence = Sequence20
                Case 21
                    currentSequence = Sequence21
                Case 22
                    currentSequence = Sequence22
                Case 23
                    currentSequence = Sequence23
                Case 24
                    currentSequence = Sequence24
                Case 25
                    currentSequence = Sequence25
                Case Else
                    currentSequence = Nothing
            End Select

            If currentSequence IsNot Nothing AndAlso currentSequence.Count > 0 Then
                RunSingleChallenge(currentSequence)
            End If
        Next
    End Sub

    Private Sub RunSingleChallenge(currentSequence As List(Of Integer))
        Dim userChallengeException As Exception = Nothing
        Dim nextGuess As Integer
        _nextAnswer = currentSequence(currentSequence.Count - 1)
        _timesChecked = 0
        _detailedOutput = New StringBuilder

        Dim stringSequence = currentSequence.Select(Function(i) i.ToString).Aggregate(Function(i, j) i & "," & j)
        _detailedOutput.AppendLine(String.Format("Sequence: {0}", stringSequence))
        Console.WriteLine("Running Sequence: {0}", stringSequence)

        currentSequence.RemoveAt(currentSequence.Count - 1)
        Dim stopWatch As Stopwatch = stopWatch.StartNew
        Try
            Console.WriteLine("Invoking entry code")
            nextGuess = Challenge.GuessNumber(currentSequence, AddressOf CheckFunction)
            Console.WriteLine("Entry returned {0}", nextGuess)
            Console.WriteLine("The correct answer is {0}", _nextAnswer)
            _detailedOutput.AppendLine(String.Format("Entry returned {0}", nextGuess))
            _detailedOutput.AppendLine(String.Format("The correct answer is {0}", _nextAnswer))
        Catch ex As Exception
            'The user challenge threw an exception
            userChallengeException = ex
            _detailedOutput.AppendLine("An exception occurred:")
            _detailedOutput.AppendLine(ex.ToString())
            Console.WriteLine("An error occurred:")
            Console.WriteLine(ex.ToString())
        End Try
        stopWatch.Stop()

        Dim score As Integer

        Dim result As New ChallengeResult()

        Dim timesCheckedPointsLost As Integer = 0
        If (_timesChecked < 6) Then
            timesCheckedPointsLost = If(_timesChecked > 0, Math.Pow(1000, Math.Max(_timesChecked / 6, 1)), 0)
            Console.WriteLine("The entry lost {0} points for making {1} call(s) to CheckAnswer", timesCheckedPointsLost, _timesChecked)
            _detailedOutput.AppendLine(String.Format("The entry lost {0} points for making {1} call(s) to CheckAnswer", timesCheckedPointsLost, _timesChecked))
        Else
            'The entry lost all available points
            timesCheckedPointsLost = 100000
            Console.WriteLine("The entry lost all available points for making {0} calls to CheckAnswer", _timesChecked)
            _detailedOutput.AppendLine(String.Format("The entry lost all available points for making {0} calls to CheckAnswer", _timesChecked))
        End If

        If nextGuess = _nextAnswer AndAlso userChallengeException Is Nothing Then
            Console.WriteLine("The entry guessed correctly")
            _detailedOutput.AppendLine("The entry guessed correctly")
            score = 100000
            score -= stopWatch.Elapsed.TotalMilliseconds
            score -= timesCheckedPointsLost
            If (score < 0) Then
                score = 0
            End If
            With result
                .ResultMessage = "The number was successfully found"
                .Score = score
                .DurationTicks = stopWatch.Elapsed.Ticks
                .Successful = True
            End With
        Else
            Console.WriteLine("The entry guessed incorrectly")
            _detailedOutput.AppendLine("The entry guessed incorrectly")
            If userChallengeException Is Nothing Then
                score = 10000
            Else
                score = 0
            End If
            score -= stopWatch.Elapsed.TotalMilliseconds
            score -= timesCheckedPointsLost
            If (score < 0) Then
                score = 0
            End If
            With result
                .ResultMessage = "The number was NOT successfully found"
                .Score = score
                .DurationTicks = stopWatch.Elapsed.Ticks
                .Successful = False
            End With
        End If

        If IsTest Then
            result.TestResults.Add(New FileResult With
                                   {
                                       .Contents = Encoding.UTF8.GetBytes(_detailedOutput.ToString()),
                                       .Filename = "log.txt"
                                   })
        End If

        ResultsAvailable(result)
    End Sub

    Private _nextAnswer As Integer
    Private _timesChecked As Integer
    Private _detailedOutput As StringBuilder

    Private Function CheckFunction(checkValue As Integer) As Boolean
        _timesChecked += 1
        _detailedOutput.AppendLine(String.Format("CheckFunction invoked with value {0}", checkValue.ToString("N0")))
        _detailedOutput.AppendLine(String.Format("Correct answer is: {0}", _nextAnswer))
        Console.WriteLine("CheckFunction invoked with value: {0}", checkValue.ToString())
        Console.WriteLine("Correct answer is: {0}", _nextAnswer)

        _detailedOutput.AppendLine(String.Format("CheckAnswer has been invoked {0} time(s)", _timesChecked))
        Console.WriteLine("CheckAnswer has been invoked {0} time(s)", _timesChecked)

        Dim returnValue As Boolean = checkValue = _nextAnswer

        _detailedOutput.AppendLine(String.Format("CheckFunction returning {0}", returnValue.ToString))
        Console.WriteLine("CheckFunction returning {0}", returnValue.ToString)

        Return returnValue
    End Function


    Protected Overrides ReadOnly Property MaxAuthorNotesLength() As Integer
        Get
            Return 0
        End Get
    End Property

    Private ReadOnly Property Sequence1 As List(Of Integer)
        Get
            'x
            '0,1,2,3,4,5,6,7,8,9
            Return GetArithmeticSequence(0, 10, 1)
        End Get
    End Property

    Private ReadOnly Property Sequence2 As List(Of Integer)
        Get
            '+2
            '0,2,4,6,8,10,12,14,16,18
            Return GetArithmeticSequence(0, 10, 2)
        End Get
    End Property

    Private ReadOnly Property Sequence3 As List(Of Integer)
        Get
            '+3
            '0,3,6,9,12,15,18,21,24,27
            Return GetArithmeticSequence(0, 10, 3)
        End Get
    End Property

    Private ReadOnly Property Sequence4 As List(Of Integer)
        Get
            '0,4,8,12,16,20,24,28,32,36
            Return GetArithmeticSequence(0, 10, 4)
        End Get
    End Property

    Private ReadOnly Property Sequence5 As List(Of Integer)
        Get
            '+5
            '0,5,10,15,20,25,30,35,40,45
            Return GetArithmeticSequence(0, 10, 5)
        End Get
    End Property

    Private ReadOnly Property Sequence6 As List(Of Integer)
        Get
            '+6
            '0,6,12,18,24,30,36,42,48,54
            Return GetArithmeticSequence(0, 10, 6)
        End Get
    End Property

    Private ReadOnly Property Sequence7 As List(Of Integer)
        Get
            '+2 -1
            '0,2,1,3,2,4,3,5,4,6,5,7,6
            Dim returnValue As New List(Of Integer)

            returnValue.Add(0)
            For i As Integer = 0 To 5
                returnValue.Add(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 2))
                returnValue.Add(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) - 1))
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence8 As List(Of Integer)
        Get
            '+1 -2
            '0,1,-1,0,-2,-1,-3,-2,-4,-3,-5,-4,-6
            Dim returnValue As New List(Of Integer)

            returnValue.Add(0)
            For i As Integer = 0 To 5
                returnValue.Add(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 1))
                returnValue.Add(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) - 2))
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence9 As List(Of Integer)
        Get
            '0,1,3,4,6,7,9,10,12
            Dim returnValue As New List(Of Integer)

            For i As Integer = 0 To 4
                If i < 4 Then
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 2), 2, 1))
                Else
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 2), 1, 1))
                End If
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence10 As List(Of Integer)
        Get
            '2 sequences 3 digits long 1) +1 2) +2
            '0, 1, 2, 4, 5, 6, 8, 9, 10, 12, 13, 14, 16
            Dim returnValue As New List(Of Integer)

            For i As Integer = 0 To 4
                If i < 4 Then
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 2), 3, 1))
                Else
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 2), 1, 1))
                End If
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence11 As List(Of Integer)
        Get
            '2 sequences 3 digits long 1) +2 2) +3
            '0, 2, 4, 7, 9, 11, 14, 16, 18, 21, 23, 25, 28, 30
            Dim returnValue As New List(Of Integer)

            For i As Integer = 0 To 4
                If i < 4 Then
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 3), 3, 2))
                Else
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) + 3), 2, 2))
                End If
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence12 As List(Of Integer)
        Get
            '3 digits +5 1 digit -3
            '0, 5, 10, 7, 12, 17, 14, 19, 24, 21, 26, 31, 28

            Dim returnValue As New List(Of Integer)

            For i As Integer = 0 To 4
                If i < 4 Then
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) - 3), 3, 5))
                Else
                    returnValue.AddRange(GetArithmeticSequence(If(returnValue.Count = 0, 0, returnValue(returnValue.Count - 1) - 3), 1, 5))
                End If
            Next

            Return returnValue
        End Get
    End Property

    Public ReadOnly Property Sequence13 As List(Of Integer)
        Get
            '*2
            '1, 2, 4, 8, 16, 32, 64, 128, 256, 512
            Return GetGeometricSequence(1, 10, 2)
        End Get
    End Property

    Private ReadOnly Property Sequence14 As List(Of Integer)
        Get
            '*3
            '1,3,9,27,81,243,729,2187,6561,19683
            Return GetGeometricSequence(1, 10, 3)
        End Get
    End Property

    Private ReadOnly Property Sequence15 As List(Of Integer)
        Get
            '*5
            '1,5,25,125,625,3125,15625,78125,390625,1953125
            Return GetGeometricSequence(1, 10, 5)
        End Get
    End Property

    Private ReadOnly Property Sequence16 As List(Of Integer)
        Get
            '*3 *5
            '1,3,15,45,225,675,3375,10125,50625
            Return New List(Of Integer) From {1, 3, 15, 45, 225, 675, 3375, 10125, 50625}
        End Get
    End Property

    Private ReadOnly Property Sequence17 As List(Of Integer)
        Get
            '*3 +5
            '1,3,8,24,29,87,92,276,281
            Return New List(Of Integer) From {1, 3, 8, 24, 29, 87, 92, 276, 281}
        End Get
    End Property

    Private ReadOnly Property Sequence18 As List(Of Integer)
        Get
            '2^x
            '1,2,4,8,16,32,64
            Return New List(Of Integer) From {1, 2, 4, 8, 16, 32, 64}
        End Get
    End Property

    Private ReadOnly Property Sequence19() As List(Of Integer)
        Get
            'x^2
            '0,1,4,6,8,10,12,14
            Return New List(Of Integer) From {0, 1, 4, 6, 8, 10, 12, 14}
        End Get
    End Property

    Private ReadOnly Property Sequence20() As List(Of Integer)
        Get
            'x^2
            '0,1,4,6,8,10,12,14
            Return New List(Of Integer) From {0, 1, 4, 6, 8, 10, 12, 14}
        End Get
    End Property

    Private ReadOnly Property Sequence21() As List(Of Integer)
        Get
            'x!
            '1,1,2,6,24,120,720,5040
            Return (New List(Of Integer) From {1, 1, 2, 6, 24, 120, 720, 5040})
        End Get
    End Property

    Private ReadOnly Property Sequence22() As List(Of Integer)
        Get
            'x! - 1
            '0,0,1,5,23,119,719,5039
            Return New List(Of Integer) From {0, 0, 1, 5, 23, 119, 719, 5039}
        End Get
    End Property

    Private ReadOnly Property Sequence23() As List(Of Integer)
        Get
            'Math.Floor(sin(x))
            '0,84,90,14,-76,-96,-28,65,98,41,-55,-100,-54,42,99,65,-29,-97,-76,14,91
            Dim returnValue As New List(Of Integer)
            For i As Integer = 0 To 20
                returnValue.Add(Math.Floor(Math.Sin(i) * 100))
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence24() As List(Of Integer)
        Get
            'Math.Floor(cos(x))
            '100,54,-42,-99,-66,28,96,75,-15,-92,-84,0,84,90,13,-76,-96,-28,66,98,40
            Dim returnValue As New List(Of Integer)
            For i As Integer = 0 To 20
                returnValue.Add(Math.Floor(Math.Cos(i) * 100))
            Next

            Return returnValue
        End Get
    End Property

    Private ReadOnly Property Sequence25() As List(Of Integer)
        Get
            'Math.Floor(tan(x))
            '0,155,-219,-15,115,-339,-30,87,-680,-46,64,-22596,-64,46,724,-86,30,349,-114,15,223
            Dim returnValue As New List(Of Integer)
            For i As Integer = 0 To 20
                returnValue.Add(Math.Floor(Math.Tan(i) * 100))
            Next

            Return returnValue
        End Get
    End Property

    Private Function GetArithmeticSequence(startingValue As Integer, length As Integer, incrementBy As Integer) As List(Of Integer)
        Dim returnValue As New List(Of Integer)

        For i As Integer = 0 To length - 1
            If returnValue.Count = 0 Then
                returnValue.Add(startingValue)
            Else
                returnValue.Add(returnValue(returnValue.Count - 1) + incrementBy)
            End If
        Next

        Return returnValue
    End Function

    Private Function GetGeometricSequence(startingValue As Integer, length As Integer, multipliedBy As Integer) As List(Of Integer)
        Dim returnValue As New List(Of Integer)

        For i As Integer = 0 To length - 1
            If returnValue.Count = 0 Then
                returnValue.Add(startingValue)
            Else
                returnValue.Add(returnValue(returnValue.Count - 1) * multipliedBy)
            End If
        Next
        Return returnValue
    End Function
End Class
