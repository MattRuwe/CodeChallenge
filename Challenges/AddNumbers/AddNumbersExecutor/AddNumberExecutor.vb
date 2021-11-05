Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.Challenges
Imports OmahaMTG.Challenge.ExecutionCommon
Imports System.Text

Public Class AddNumberExecutor
    Inherits ChallengeExecutorBase(Of IAddNumberChallenge)

    ''' <summary>
    ''' Entry point for the execution of user entries
    ''' </summary>
    ''' <remarks>
    ''' This method is called inside of the security sandbox.  It is executed only once.
    ''' </remarks>
    Protected Overrides Sub RunChallengeOverride()
        'Setup the parameters to pass into the entry
        Dim x, y As Integer

        'Setup the timer that will keep track of how long the entry takes to run
        Dim sw As Stopwatch

        'Random number generator for creating new values to pass to the entry
        Dim rnd As New Random()

        For i As Integer = 0 To 10
            'Create a new challenge result
            Dim challengeResult As New ChallengeResult

            'Setup the parameters for the entry
            If IsTest Then
                'If the entry is running in test mode, then the data passed to the entry should be different than standard mode
                'Ideally, the data will be random
                x = rnd.Next(0, 10000)
                y = rnd.Next(0, 10000)
            Else
                'If the entry is running in standard mode, the data should always be the same to ensure fairness between all entries
                x = i
                y = i + 25
            End If

            'Start the timer
            sw = Stopwatch.StartNew()

            'Run the entry
            Dim result As Integer = MyBase.Challenge.Add(x, y)

            'Stop the timer
            sw.Stop()

            'Create a challenge result, some of which will be displayed on the web site

            'Store the number of ticks that elapsed during the execution
            challengeResult.DurationTicks = sw.Elapsed.Ticks
            Dim detailedOutput As String = String.Empty
            If result <> (x + y) Then
                'The entry failed to correctly calculate the result
                challengeResult.Successful = False
                challengeResult.ResultMessage = "The two numbers did not add together correctly."
                challengeResult.Score = 50000
                detailedOutput = String.Format("Failed to successfully add {0} and {1}.  Entry calculated result as {2} instead of {3}", x, y, result, x + y)
            Else
                'The entry successfully calculated the correct value
                challengeResult.Successful = True
                challengeResult.ResultMessage = "Successfully added two numbers together."
                challengeResult.Score = 100000
                detailedOutput = String.Format("Successfully added {0} and {1} to equal {2}", x, y, result)
            End If

            If IsTest Then
                'The entry is running in test mode, so add the detailed test data to the challenge result
                Dim fileResult As New FileResult With
                    {
                        .Contents = Encoding.UTF8.GetBytes(detailedOutput),
                        .Filename = "AddNumberOutput.txt"
                    }
                challengeResult.TestResults.Add(fileResult)
            End If
            challengeResult.Score -= sw.ElapsedMilliseconds

            'When the challenge result is ready, send it to the execution base for storage.
            ResultsAvailable(challengeResult)
        Next
    End Sub

    Protected Overrides ReadOnly Property MaxAuthorNotesLength As Integer
        Get
            Return 0
        End Get
    End Property
End Class
