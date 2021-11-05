Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports OmahaMTG.Challenge.Challenges



'''<summary>
'''This is a test class for RubiksCubeTest and is intended
'''to contain all RubiksCubeTest Unit Tests
'''</summary>
<TestClass()> _
Public Class RubiksCubeTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

    <TestMethod()>
    <ExpectedException(GetType(InvalidOperationException))>
    Public Sub InvalidMoveTest()
        Dim target As New RubiksCube

        target.MakeMove(RubiksMove.LeftRightAxisCounterClockwise + 1)
    End Sub

    '''<summary>
    '''A test for RubiksCube Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub RubiksCubeConstructorManualTest()
        Dim target As RubiksCube = New RubiksCube()

        AssertCubeIsSolved(target)

    End Sub

    <TestMethod()> _
    Public Sub TestLeftRightAxisCounterClockwise()
        Dim target As New RubiksCube

        target.MakeMove(RubiksMove.LeftRightAxisCounterClockwise)
        target.MakeMove(RubiksMove.BackClockwise)
        target.MakeMove(RubiksMove.LeftRightAxisCounterClockwise)
        target.MakeMove(RubiksMove.BottomCounterClockwise)

        Assert.IsTrue(target.IsSolved)
    End Sub

    <TestMethod()> _
    Public Sub TestLeftRightAxisClockwise()
        Dim target As New RubiksCube

        target.MakeMove(RubiksMove.LeftRightAxisClockwise)
        target.MakeMove(RubiksMove.BackClockwise)
        target.MakeMove(RubiksMove.LeftRightAxisClockwise)
        target.MakeMove(RubiksMove.TopCounterClockwise)

        Assert.IsTrue(target.IsSolved)
    End Sub

    <TestMethod()> _
    Public Sub TestLeftRightRotation()
        Dim target As New RubiksCube

        target.MakeMove(RubiksMove.LeftRightAxisCounterClockwise)

        target.MakeMove(RubiksMove.LeftRightAxisClockwise)
        AssertCubeIsSolved(target)

        For i As Integer = 0 To 3
            target.MakeMove(RubiksMove.LeftRightAxisClockwise)
        Next
        AssertCubeIsSolved(target)

        For i As Integer = 0 To 3
            target.MakeMove(RubiksMove.LeftRightAxisCounterClockwise)
        Next
        AssertCubeIsSolved(target)
    End Sub

    <TestMethod()> _
    Public Sub TestTopBottomRotation()
        Dim target As New RubiksCube

        target.MakeMove(RubiksMove.TopBottomAxisClockwise)

        target.MakeMove(RubiksMove.TopBottomAxisCounterClockwise)
        AssertCubeIsSolved(target)

        For i As Integer = 0 To 3
            target.MakeMove(RubiksMove.TopBottomAxisClockwise)
        Next
        AssertCubeIsSolved(target)

        For i As Integer = 0 To 3
            target.MakeMove(RubiksMove.TopBottomAxisCounterClockwise)
        Next
        AssertCubeIsSolved(target)

    End Sub

    <TestMethod()> _
    Public Sub TestFrontBackRotation()
        Dim target As New RubiksCube

        target.MakeMove(RubiksMove.FrontBackAxisClockwise)

        target.MakeMove(RubiksMove.FrontBackAxisCounterClockwise)
        AssertCubeIsSolved(target)

        For i As Integer = 0 To 3
            target.MakeMove(RubiksMove.FrontBackAxisClockwise)
        Next
        AssertCubeIsSolved(target)

        For i As Integer = 0 To 3
            target.MakeMove(RubiksMove.FrontBackAxisCounterClockwise)
        Next
        AssertCubeIsSolved(target)

    End Sub

    <TestMethod()>
    Public Sub TestFrontFullRotation()
        TestFullRotation(RubiksMove.FrontClockwise)
        TestFullRotation(RubiksMove.FrontCounterClockwise)
    End Sub

    <TestMethod()>
    Public Sub TestBackFullRotation()
        TestFullRotation(RubiksMove.BackClockwise)
        TestFullRotation(RubiksMove.BackCounterClockwise)
    End Sub

    <TestMethod()>
    Public Sub TestLeftFullRotation()
        TestFullRotation(RubiksMove.LeftClockwise)
        TestFullRotation(RubiksMove.LeftCounterClockwise)
    End Sub

    <TestMethod()>
    Public Sub TestRightFullRotation()
        TestFullRotation(RubiksMove.RightClockwise)
        TestFullRotation(RubiksMove.RightCounterClockwise)
    End Sub

    <TestMethod()>
    Public Sub TestTopFullRotation()
        TestFullRotation(RubiksMove.TopClockwise)
        TestFullRotation(RubiksMove.TopCounterClockwise)
    End Sub

    <TestMethod()>
    Public Sub TestDownFullRotation()
        TestFullRotation(RubiksMove.BottomClockwise)
        TestFullRotation(RubiksMove.BottomCounterClockwise)
    End Sub

    Private Sub TestFullRotation(move As RubiksMove)
        Dim target As New RubiksCube

        For i As Integer = 0 To 3
            target.makeMove(move)
        Next

        Assert.IsTrue(target.isSolved)
    End Sub

    <TestMethod()>
    Public Sub RubiksTest1()
        Dim target As New RubiksCube()
        Dim random As New Random()
        Dim move As RubiksMove

        For i As Integer = 0 To 100
            move = random.Next(1, 19)
            target.MakeMove(move)
            Debug.WriteLine("                            RubiksMove." & [Enum].GetName(GetType(RubiksMove), move) & ",")
        Next

        Assert.IsFalse(target.IsSolved)

        target.RecordHistory = False
        While target.MoveHistory.Count > 0
            move = target.MoveHistory.Pop()
            'Debug.WriteLine([Enum].GetName(GetType(RubiksMove), move))

            Dim moveint As Integer = CType(move, Integer)
            If moveint Mod 2 = 0 Then
                'counterclockwise move, subtract one to get the opposite move
                target.MakeMove(CType(moveint - 1, RubiksMove))
            Else
                'clockwise move, add one to get the opposite move
                target.MakeMove(CType(moveint + 1, RubiksMove))
            End If
        End While

        AssertCubeIsSolved(target)
    End Sub

    Private Sub AssertCubeIsSolved(ByVal target As RubiksCube)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.TopLeft)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.TopMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.TopRight)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.MiddleLeft)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.MiddleMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.MiddleRight)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.BottomLeft)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.BottomMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.White, target.Front.BottomRight)

        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.TopLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.TopMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.TopRight)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.MiddleLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.MiddleMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.MiddleRight)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.BottomLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.BottomMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Red, target.Right.BottomRight)

        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.TopLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.TopMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.TopRight)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.MiddleLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.MiddleMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.MiddleRight)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.BottomLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.BottomMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Orange, target.Left.BottomRight)

        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.TopLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.TopMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.TopRight)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.MiddleLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.MiddleMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.MiddleRight)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.BottomLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.BottomMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Yellow, target.Back.BottomRight)

        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.TopLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.TopMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.TopRight)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.MiddleLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.MiddleMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.MiddleRight)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.BottomLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.BottomMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Blue, target.Top.BottomRight)

        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.TopLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.TopMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.TopRight)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.MiddleLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.MiddleMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.MiddleRight)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.BottomLeft)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.BottomMiddle)
        Assert.AreEqual(Of TileColor)(TileColor.Green, target.Bottom.BottomRight)
    End Sub

    '''<summary>
    '''A test for GetReversedMove
    '''</summary>
    <TestMethod()> _
    Public Sub GetReversedMoveTest1()
        Dim move As RubiksMove = RubiksMove.BackClockwise
        Dim expected As RubiksMove = RubiksMove.BackCounterClockwise
        Dim actual As RubiksMove
        actual = RubiksCube.GetReversedMove(move)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetReversedMove
    '''</summary>
    <TestMethod()> _
    Public Sub GetReversedMoveTest2()
        Dim move As RubiksMove = RubiksMove.BackCounterClockwise
        Dim expected As RubiksMove = RubiksMove.BackClockwise
        Dim actual As RubiksMove
        actual = RubiksCube.GetReversedMove(move)
        Assert.AreEqual(expected, actual)
    End Sub
End Class
