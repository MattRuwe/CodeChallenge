''' <summary>
''' A representation of one of the six side of a Rubik's cube.  
''' </summary>
''' <remarks>This class does not contain information about which side of the cube it represents.  To determine that, you should refer
''' to the RubiksCube class.</remarks>
<Serializable()>
Public Class CubeFace
    Private _tiles(2, 2) As TileColor

    Friend Sub New(ByVal tileColor As TileColor)
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                _tiles(i, j) = tileColor
            Next
        Next
    End Sub


    Friend Sub New()

    End Sub

    ''' <summary>
    ''' The tile located in the top row and left column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TopLeft() As TileColor
        Get
            Return _tiles(0, 0)
        End Get
        Set(ByVal value As TileColor)
            _tiles(0, 0) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the top row and middle column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TopMiddle() As TileColor
        Get
            Return _tiles(1, 0)
        End Get
        Set(ByVal value As TileColor)
            _tiles(1, 0) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the top row and right column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TopRight() As TileColor
        Get
            Return _tiles(2, 0)
        End Get
        Set(ByVal value As TileColor)
            _tiles(2, 0) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the middle row and left column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MiddleLeft() As TileColor
        Get
            Return _tiles(0, 1)
        End Get
        Set(ByVal value As TileColor)
            _tiles(0, 1) = value
        End Set
    End Property
    ''' <summary>
    ''' The tile located in the middle row and middle column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>This tile is special because it never changes positions when the cube is rotated.</remarks>
    Public Property MiddleMiddle() As TileColor
        Get
            Return _tiles(1, 1)
        End Get
        Set(ByVal value As TileColor)
            _tiles(1, 1) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the middle row and right column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MiddleRight() As TileColor
        Get
            Return _tiles(2, 1)
        End Get
        Set(ByVal value As TileColor)
            _tiles(2, 1) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the bottom row and left column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BottomLeft() As TileColor
        Get
            Return _tiles(0, 2)
        End Get
        Set(ByVal value As TileColor)
            _tiles(0, 2) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the bottom row and middle column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BottomMiddle() As TileColor
        Get
            Return _tiles(1, 2)
        End Get
        Set(ByVal value As TileColor)
            _tiles(1, 2) = value
        End Set
    End Property

    ''' <summary>
    ''' The tile located in the bottom row and right column of the represented cube face
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BottomRight() As TileColor
        Get
            Return _tiles(2, 2)
        End Get
        Set(ByVal value As TileColor)
            _tiles(2, 2) = value
        End Set
    End Property

    ' ''' <summary>
    ' ''' Rotate the cube face 90 degrees counter clockwise
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Friend Sub RotateCounterClockwise()
    '    Dim tmpCubeFace As CubeFace = Me.Clone()
    '    With tmpCubeFace
    '        TopLeft = .TopRight
    '        TopMiddle = .MiddleRight
    '        TopRight = .BottomRight
    '        MiddleLeft = .TopMiddle
    '        MiddleMiddle = .MiddleMiddle
    '        MiddleRight = .BottomMiddle
    '        BottomLeft = .TopLeft
    '        BottomMiddle = .MiddleLeft
    '        BottomRight = .BottomLeft
    '    End With
    'End Sub

    ' ''' <summary>
    ' ''' Rotate the cube face 90 degrees clockwise
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Friend Sub RotateClockwise()
    '    Dim tmpCubeFace As CubeFace = Me.Clone()
    '    With tmpCubeFace
    '        TopLeft = .BottomLeft
    '        TopMiddle = .MiddleLeft
    '        TopRight = .TopLeft
    '        MiddleLeft = .BottomMiddle
    '        MiddleMiddle = .MiddleMiddle
    '        MiddleRight = .TopMiddle
    '        BottomLeft = .BottomRight
    '        BottomMiddle = .MiddleRight
    '        BottomRight = .TopRight
    '    End With
    'End Sub

    ' ''' <summary>
    ' ''' Rotate the cube face 180 degrees
    ' ''' </summary>
    ' ''' <remarks>The direction does not matter when rotating 180 degrees because the end result is the same whether you go clockwise or counter-clockwise.</remarks>
    'Friend Sub Rotate180Degrees()
    '    Dim tmpCubeFace As CubeFace = Me.Clone()
    '    With tmpCubeFace
    '        TopLeft = .BottomRight
    '        TopMiddle = .BottomMiddle
    '        TopRight = .BottomLeft
    '        MiddleLeft = .MiddleRight
    '        MiddleMiddle = .MiddleMiddle
    '        MiddleRight = .MiddleLeft
    '        BottomLeft = .TopRight
    '        BottomMiddle = .TopMiddle
    '        BottomRight = .TopLeft
    '    End With
    'End Sub

    ' ''' <summary>
    ' ''' Makes a copy of the cube face that can be midified independetly from the original
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Friend Function Clone() As CubeFace
    '    Dim returnValue As New CubeFace(TileColor.None)

    '    With returnValue
    '        .TopLeft = TopLeft
    '        .TopMiddle = TopMiddle
    '        .TopRight = TopRight
    '        .MiddleLeft = MiddleLeft
    '        .MiddleMiddle = MiddleMiddle
    '        .MiddleRight = MiddleRight
    '        .BottomLeft = BottomLeft
    '        .BottomMiddle = BottomMiddle
    '        .BottomRight = BottomRight
    '    End With

    '    Return returnValue
    'End Function

    ' ''' <summary>
    ' ''' Checks the cube face to see if it is all one color
    ' ''' </summary>
    ' ''' <param name="color"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Friend Function GetIsAllOneColor(ByVal color As TileColor) As Boolean
    '    Return TopLeft = color AndAlso
    '        TopMiddle = color AndAlso
    '        TopRight = color AndAlso
    '        MiddleLeft = color AndAlso
    '        MiddleMiddle = color AndAlso
    '        MiddleRight = color AndAlso
    '        BottomLeft = color AndAlso
    '        BottomMiddle = color AndAlso
    '        BottomRight = color
    'End Function
End Class
