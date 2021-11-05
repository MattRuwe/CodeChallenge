Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.GuessTheNumberChallenge

Public Class NumberGuesser
    Implements IGuessTheNumberChallenge

    Public ReadOnly Property AuthorNotes() As String Implements IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Public Function GuessNumber(sequence As IEnumerable(Of Integer), checkAnswer As Func(Of Integer, Boolean)) As Integer Implements IGuessTheNumberChallenge.GuessNumber
        Dim answer As Integer = 0

        For i As Integer = 0 To 20
            If checkAnswer(answer) Then
                Exit For
            End If
            answer += 1
        Next

        Return answer
    End Function

End Class
