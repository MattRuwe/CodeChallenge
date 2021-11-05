Imports OmahaMTG.Challenge.ChallengeCommon
Public Interface IGuessTheNumberChallenge
    Inherits IChallenge
    Function GuessNumber(sequence As IEnumerable(Of Integer), checkAnswer As Func(Of Integer, Boolean)) As Integer
End Interface
