Imports OmahaMTG.Challenge.ChallengeCommon

Public MustInherit Class TurnBasedChallengeExecutorBase(Of TEntry As ITurnChallenge(Of TEnvironment, TMove), TEnvironment As ITurnEnvironment, TMove As ITurnMove)
    Protected MustOverride Sub RunFullTurnOverride()

    Private _players As List(Of TEntry)
    Protected ReadOnly Property Players As IEnumerable(Of TEntry)
        Get
            If _players Is Nothing Then
                _players = New List(Of TEntry)
            End If

            Return _players
        End Get
    End Property
End Class
