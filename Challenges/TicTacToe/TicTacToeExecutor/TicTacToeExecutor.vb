Imports OmahaMTG.Challenge.ExecutionCommon

Public Class TicTacToeExecutor
    Inherits TurnBasedChallengeExecutorBase(Of ITicTacToeChallenge, Board, BoardIndex)

    Protected Overrides Sub RunFullTurnOverride()
        Dim players As New List(Of ITicTacToeChallenge)(CType(MyBase.Players.ToArray.Clone, IEnumerable(Of ITicTacToeChallenge)))
        Dim losers As New List(Of ITicTacToeChallenge)()

        While players.Count > 1
            For i As Integer = 0 To players.Count - 1
                If (i + 1) < players.Count Then
                    Select Case RunSingleGame(players(i), players(i + 1))
                        Case Player.X
                            losers.Add(players(i))
                        Case Player.O
                            losers.Add(players(i + 1))
                    End Select
                End If
            Next

            For Each player As ITicTacToeChallenge In losers
                players.Remove(player)
            Next

        End While
    End Sub

    Private Function RunSingleGame(playerX As ITicTacToeChallenge, playerO As ITicTacToeChallenge) As Player
        Dim board As New Board(3)

        Dim playerXTime As New Stopwatch()
        Dim playerOTime As New Stopwatch()

        While board.GetGameStatus = GameStatus.InProgress
            If board.CurrentPlayer = Player.X Then
                playerXTime.Start()
                board.MakeMove(playerX.MakeMove(board.Clone))
                playerXTime.Stop()
            Else
                playerOTime.Start()
                board.MakeMove(playerO.MakeMove(board.Clone))
                playerOTime.Stop()
            End If
        End While

        Dim returnValue As Player
        Dim status As GameStatus = board.GetGameStatus()
        If status = GameStatus.XWins Then
            returnValue = Player.X
        ElseIf status = GameStatus.OWins Then
            returnValue = Player.O
        ElseIf status = GameStatus.Draw Then
            If playerXTime.ElapsedTicks > playerOTime.ElapsedTicks Then
                returnValue = Player.O
            Else
                returnValue = Player.X
            End If
        End If

        Return returnValue
    End Function


End Class

'Public Enum Winner
'    Player1
'    Player2
'End Enum