Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.GuessTheNumberChallenge

Public Class Guesser
Implements IGuessTheNumberChallenge
    Public ReadOnly Property AuthorNotes() As String Implements IChallenge.AuthorNotes
        Get
            Return String.Empty
        End Get
    End Property

    Public Function GuessNumber(sequence As IEnumerable(Of Integer), checkAnswer As Func(Of Integer, Boolean)) As Integer Implements IGuessTheNumberChallenge.GuessNumber
        Dim delta As Integer = sequence(1) - sequence(0)
        Dim listSeq As List(Of Integer) = sequence.ToList()
        Dim returnValue As Integer = listSeq(listSeq.Count - 1) + delta
        If Not checkAnswer(returnValue) Then
            delta = sequence(2) - sequence(1)
            returnValue = listSeq(listSeq.Count - 1) + delta

            For i As Integer = 0 To 10
                If Not checkAnswer(returnValue) Then
                    returnValue = returnValue + 1
                Else
                    Exit For
                End If
            Next

        End If

        

        Return returnValue

    End Function

End Class
