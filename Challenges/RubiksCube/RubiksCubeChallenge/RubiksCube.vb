Imports System.Diagnostics.Contracts

' http://agoln.net/archives/70
' http://www.amzi.com/ExpertSystemsInProlog/12rubikscube.htm
' http://upload.wikimedia.org/wikipedia/commons/thumb/b/b9/Western_color_scheme_of_a_Rubik%27s_Cube.svg/280px-Western_color_scheme_of_a_Rubik%27s_Cube.svg.png


''' <summary>
''' A data structure that is representative of a Rubik's Cube.
''' </summary>
''' <remarks>
''' To manipulate the cube, you should use the <see cref="M:OmahaMTG.Challenge.Challenges.RubiksCube.MakeMove(OmahaMTG.Challenge.Challenges.RubiksMove)" /> method.
''' This method will allow you to make any of the standard cube moves (e.g. front, right, back inverse, left inverse, etc.) 
''' in addition to the special cube moves (rotate on the top bottom axis clockwise, rotate on the left right axis clockwise, and 
''' rotate on the front back axis clockwise).  See the <see cref="T:OmahaMTG.Challenge.Challenges.RubiksMove" /> object for more details
''' on each of these moves.
''' 
''' The RubiksCube object contains 6 properties referencing the 6 different sides of a Rubik's Cube.  These properties are:<br />
''' <ul>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.Front" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.Right" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.Left" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.Back" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.Top" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.Bottom" /></li>
''' </ul>
''' Each one of these cube faces has 9 "tiles" that indicate the position of the tile within the cube face.  These tiles are:
''' 
''' <ul>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.TopLeft" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.TopMiddle" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.TopRight" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.MiddleLeft" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.MiddleMiddle" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.MiddleRight" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.BottomLeft" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.BottomMiddle" /></li>
'''   <li><see cref="P:OmahaMTG.Challenge.Challenges.CubeFace.BottomRight" /></li>
''' 
''' </ul>
''' 
''' <b>NOTE:</b>  Each cube face is oriented as you would expect if you were looking at that side of the cube facing you.  For instance,
''' imagine that you're holding a cube in your hand with the front facing you.  The top left is in the top left corner.  Now if you 
''' want to know how the bottom face is oriented, you need to imagine rotating the entire cube on the left and right axis 90 degrees.
''' The bottom's top left face is where you would expect it to be, the top left corner.  The back is a little different, again, imagine 
''' that you're holding the cube with the front facing you.  To know how the back is oriented, you need to rotate the entire cube on the
''' top bottom axis 180 degrees.  Now the back is the way you would expect with the top left being in the top left and the bottom right 
''' being in the bottom right.<br /><br />
''' 
''' This can be confusing so the following pictoral representation of the cube may be helpful.<br /><br />
''' <img src="../Images/RubiksCube.png" />
''' 
''' </remarks>
<Serializable()>
Public Class RubiksCube

    Dim _fastCube As FastRubiksCube

    Public Sub New()
        _fastCube = New FastRubiksCube()
    End Sub

    Private Sub New(fastCube As FastRubiksCube)
        _fastCube = fastCube
    End Sub

    ''' <summary>
    ''' A cube face that represents the current front side of the cube
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Front() As CubeFace
        Get
            Return _fastCube.GetFace(FaceIndicator.Front)
        End Get
    End Property

    ''' <summary>
    ''' A cube face that represents the current back side of the cube
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Back() As CubeFace
        Get
            Return _fastCube.GetFace(FaceIndicator.Back)
        End Get
    End Property

    ''' <summary>
    ''' A cube face that represents the current right side of the cube
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Right() As CubeFace
        Get
            Return _fastCube.GetFace(FaceIndicator.Right)
        End Get
    End Property

    ''' <summary>
    ''' A cube face that represents the current left side of the cube
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Left() As CubeFace
        Get
            Return _fastCube.GetFace(FaceIndicator.Left)
        End Get
    End Property

    ''' <summary>
    ''' A cube face that represents the current top side of the cube
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Top() As CubeFace
        Get
            Return _fastCube.GetFace(FaceIndicator.Top)
        End Get
    End Property

    ''' <summary>
    ''' A cube face that represents the current bottom side of the cube
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Bottom() As CubeFace
        Get
            Return _fastCube.GetFace(FaceIndicator.Bottom)
        End Get
    End Property

#Region " Traditional moves"
    ''' <summary>
    ''' Transforms the cube in the way specified by the move parameter
    ''' </summary>
    ''' <param name="move">A move that alters the cube</param>
    ''' <remarks></remarks>
    Public Sub MakeMove(ByVal move As RubiksMove)
        _fastCube.makeMove(move)

        If RecordHistory Then
            MoveHistory.Push(move)
        End If
    End Sub

    ''' <summary>
    ''' Reverses a RubiksMove
    ''' </summary>
    ''' <param name="move">The move that is to be reversed</param>
    ''' <returns>A RubiksMove value that is opposite from the value passed into the function.</returns>
    ''' <remarks>This method is useful for "playing back" moves from the <see cref="P:OmahaMTG.Challenge.Challenges.RubiksCube.MoveHistory" /> property</remarks>
    Public Shared Function GetReversedMove(move As RubiksMove) As RubiksMove
        Dim moveInt As Integer = move
        Dim returnValue As RubiksMove = RubiksMove.None

        If moveInt Mod 2 = 0 Then
            'counterclockwise move, subtract one to get the opposite move
            returnValue = CType(moveInt - 1, RubiksMove)
        Else
            'clockwise move, add one to get the opposite move
            returnValue = CType(moveInt + 1, RubiksMove)
        End If

        Return returnValue
    End Function
#End Region

    Private _recordHistory As Boolean = True
    ''' <summary>
    ''' A flag indicating whether or not the cube should record the history of it's moves
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RecordHistory() As Boolean
        Get
            Return _recordHistory
        End Get
        Set(ByVal value As Boolean)
            _recordHistory = value
        End Set
    End Property

    Private _moveHistory As Stack(Of RubiksMove)
    ''' <summary>
    ''' Contains the moves that have been made to this cube
    ''' </summary>
    ''' <value></value>
    ''' <returns>A stack of RubiksMove values that indicate what moves have been performed on this cube.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MoveHistory As Stack(Of RubiksMove)
        Get
            If _moveHistory Is Nothing Then
                _moveHistory = New Stack(Of RubiksMove)
            End If

            Return _moveHistory
        End Get
    End Property

    ''' <summary>
    ''' Determines if the Rubik's cube is successfully solved
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indicating whether or not the cube is solved</returns>
    ''' <remarks>The cube is only considered completely solve when each side contains only tiles that match the MiddleMiddle tile's color.</remarks>
    Public ReadOnly Property IsSolved() As Boolean
        Get
            Return _fastCube.isSolved()
        End Get
    End Property

    Public Function Clone() As RubiksCube
        Return New RubiksCube(_fastCube.Clone)
    End Function
End Class
