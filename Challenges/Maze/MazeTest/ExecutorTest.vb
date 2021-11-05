Imports OmahaMTG.Challenge.Challenges
Imports OmahaMTG.Challenge.ExecutionCommon
Imports OmahaMTG.Challenge.MazeImplementation
<TestClass()>
Public Class ExecutorTest

    <TestMethod()>
    Public Sub TestExecutor()
        Dim executor As New MazeExecutor()
        Dim result As List(Of ChallengeResult) = executor.RunChallenge(New MazeImplementation())

    End Sub

End Class
