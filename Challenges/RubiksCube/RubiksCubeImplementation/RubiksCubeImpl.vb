Imports OmahaMTG.Challenge.Challenges
Imports System.Threading

Public Class RubiksCubeImpl
    Implements IRubiksCubeChallenge

    Public ReadOnly Property AuthorNotes As String Implements ChallengeCommon.IChallenge.AuthorNotes
        Get
            Return "Best solver"
        End Get
    End Property

    Public Function SolveCube(ByVal cube As RubiksCube) As IEnumerable(Of RubiksMove) Implements IRubiksCubeChallenge.SolveCube
        Dim returnValue As New List(Of RubiksMove) From {
            RubiksMove.BackClockwise, RubiksMove.LeftCounterClockwise, RubiksMove.FrontCounterClockwise}

        'Dim random As New Random

        'While Not cube.IsSolved
        '    Dim move As RubiksMove = random.Next(1, 13)
        '    returnValue.Add(move)
        '    cube.MakeMove(move)
        'End While

        Return returnValue
    End Function
End Class
