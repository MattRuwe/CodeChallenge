Imports System.Collections.Generic
Imports System.Linq
Imports System.Text


Friend Class FastRubiksCube
    'Colors
    Private Const W As Integer = TileColor.White
    Private Const R As Integer = TileColor.Red
    Private Const B As Integer = TileColor.Blue
    Private Const G As Integer = TileColor.Green
    Private Const O As Integer = TileColor.Orange
    Private Const Y As Integer = TileColor.Yellow

    'Number of bits to shift to move a tile
    Private Const TS1 As Integer = 3
    Private Const TS2 As Integer = 6
    Private Const TS3 As Integer = 9
    Private Const TS4 As Integer = 12
    Private Const TS5 As Integer = 15
    Private Const TS6 As Integer = 18
    Private Const TS7 As Integer = 21
    Private Const TS8 As Integer = 24

    'Tile mask to use with bitwise ands
    Private Const MM As Integer = &H7
    Private Const TL As Integer = (MM << TS1)
    Private Const TM As Integer = (MM << TS2)
    Private Const TR As Integer = (MM << TS3)
    Private Const MR As Integer = (MM << TS4)
    Private Const BR As Integer = (MM << TS5)
    Private Const BM As Integer = (MM << TS6)
    Private Const BL As Integer = (MM << TS7)
    Private Const ML As Integer = (MM << TS8)

    'Row/Column masks
    Private Const TOPROW As Integer = TL Or TM Or TR
    Private Const RIGHTCOLUMN As Integer = TR Or MR Or BR
    Private Const BOTTOMROW As Integer = BR Or BM Or BL
    Private Const LEFTCOLUMN As Integer = BL Or ML Or TL

    'Inverse Row/Column masks
    Private Const NOTTOPROW As Integer = Not TOPROW
    Private Const NOTRIGHTCOLUMN As Integer = Not RIGHTCOLUMN
    Private Const NOTBOTTOMROW As Integer = Not BOTTOMROW
    Private Const NOTLEFTCOLUMN As Integer = Not LEFTCOLUMN

    'Clockwise Shift
    Private CC As Integer = TOPROW Or RIGHTCOLUMN Or BM

    'Counter-clockwise shift
    Private CCC As Integer = ML Or BOTTOMROW Or RIGHTCOLUMN

    'Cube faces
    Private _front As Integer
    Private _right As Integer
    Private _top As Integer
    Private _bottom As Integer
    Private _left As Integer
    Private _back As Integer

    Public Sub New()
        'Assign white to the front face
        _front = W Or (W << TS1) Or (W << TS2) Or (W << TS3) Or (W << TS4) Or (W << TS5) Or (W << TS6) Or (W << TS7) Or (W << TS8)

        'Assign red to the right face
        _right = R Or (R << TS1) Or (R << TS2) Or (R << TS3) Or (R << TS4) Or (R << TS5) Or (R << TS6) Or (R << TS7) Or (R << TS8)

        'Assign blue to the top face
        _top = B Or (B << TS1) Or (B << TS2) Or (B << TS3) Or (B << TS4) Or (B << TS5) Or (B << TS6) Or (B << TS7) Or (B << TS8)

        'Assign green to the bottom face
        _bottom = G Or (G << TS1) Or (G << TS2) Or (G << TS3) Or (G << TS4) Or (G << TS5) Or (G << TS6) Or (G << TS7) Or (G << TS8)

        'Assign orange to the left face
        _left = O Or (O << TS1) Or (O << TS2) Or (O << TS3) Or (O << TS4) Or (O << TS5) Or (O << TS6) Or (O << TS7) Or (O << TS8)

        'Assign yellow to the back face
        _back = Y Or (Y << TS1) Or (Y << TS2) Or (Y << TS3) Or (Y << TS4) Or (Y << TS5) Or (Y << TS6) Or (Y << TS7) Or (Y << TS8)
    End Sub

    Private Sub New(front As Integer, right As Integer, top As Integer, bottom As Integer, left As Integer, back As Integer)
        _front = front
        _right = right
        _top = top
        _bottom = bottom
        _left = left
        _back = back
    End Sub

    Friend Function Clone() As FastRubiksCube
        Return New FastRubiksCube(_front, _right, _top, _bottom, _left, _back)
    End Function

    Public Sub makeMove(move As RubiksMove)
        Dim tmp As Integer
        Select Case move
            Case RubiksMove.FrontClockwise
                _front = (_front And MM) Or (((_front And BL) Or (_front And ML)) >> TS6) Or ((_front And CC) << TS2)
                'rotate all cubes except for MM, 2 cube lengths clockwise
                tmp = _bottom
                _bottom = (_bottom And NOTTOPROW) Or (((_right And BL) Or (_right And ML)) >> TS6) Or ((_right And TL) << TS2)
                'clear bottom row (CB) | BL >6 TL | ML >6 TM | TL <2 TR
                _right = (_right And NOTLEFTCOLUMN) Or (((_top And BR) Or (_top And BM)) << TS2) Or ((_top And BL) >> TS6)
                'clear left col  (CL) | BR <2 BL | BM <2 ML | BL >6 TL
                _top = (_top And NOTBOTTOMROW) Or ((_left And RIGHTCOLUMN) << TS2)
                'clear bottom row (CB) | TR <2 BR | MR <2 BM | BR <2 BL
                _left = (_left And NOTRIGHTCOLUMN) Or ((tmp And TOPROW) << TS2)
                'clear right col (CR) | TL <2 TR | TM <2 MR | TR <2 BR

            Case RubiksMove.FrontCounterClockwise
                _front = (_front And MM) Or (((_front And TL) Or (_front And TM)) << TS6) Or ((_front And CCC) >> TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTTOPROW) Or ((_left And RIGHTCOLUMN) >> TS2)
                'TR >2 TL | MR >2 TM | BR >2 TR
                _left = (_left And NOTRIGHTCOLUMN) Or ((_top And BOTTOMROW) >> TS2)
                'BR >2 TR | BM >2 MR | BL >2 BR
                _top = (_top And NOTBOTTOMROW) Or (((_right And BL) Or (_right And ML)) >> TS2) Or ((_right And TL) << TS6)
                'BL >2 BR | ML >2 BM | TL <6 BL
                _right = (_right And NOTLEFTCOLUMN) Or (((tmp And TL) Or (tmp And TM)) << TS6) Or ((tmp And TR) >> TS2)
                'TL <6 BL | TM <6 ML | TR >2 TL

            Case RubiksMove.BackClockwise
                _back = (_back And MM) Or (((_back And BL) Or (_back And ML)) >> TS6) Or ((_back And CC) << TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTBOTTOMROW) Or ((_left And TL) << TS6) Or (((_left And ML) Or (_left And BL)) >> TS2)
                'TL <6 BL | ML >2 BM | BL >2 BR
                _left = (_left And NOTLEFTCOLUMN) Or ((_top And TR) >> TS2) Or (((_top And TM) Or (_top And TL)) << TS6)
                'TR >2 TL | TM <6 ML | TL <6 BL
                _top = (_top And NOTTOPROW) Or ((_right And RIGHTCOLUMN) >> TS2)
                'BR >2 TR | MR >2 TM | TR >2 TL
                _right = (_right And NOTRIGHTCOLUMN) Or ((tmp And BOTTOMROW) >> TS2)
                'BL >2 BR | BM >2 MR | BR >2 TR

            Case RubiksMove.BackCounterClockwise
                _back = (_back And MM) Or (((_back And TL) Or (_back And TM)) << TS6) Or ((_back And CCC) >> TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTBOTTOMROW) Or ((_right And RIGHTCOLUMN) << TS2)
                'BR <2 BL | MR <2 BM | TR <2 BR
                _right = (_right And NOTRIGHTCOLUMN) Or ((_top And TOPROW) << TS2)
                'TR <2 BR | TM <2 MR | TL <2 TR
                _top = (_top And NOTTOPROW) Or ((_left And TL) << TS2) Or (((_left And ML) Or (_left And BL)) >> TS6)
                'TL <2 TR | ML >6 TM | BL >6 TL
                _left = (_left And NOTLEFTCOLUMN) Or ((tmp And BL) >> TS6) Or (((tmp And BM) Or (tmp And BR)) << TS2)
                'BL >6 TL | BM <2 ML | BR <2 BL

            Case RubiksMove.LeftClockwise
                _left = (_left And MM) Or (((_left And BL) Or (_left And ML)) >> TS6) Or ((_left And CC) << TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTLEFTCOLUMN) Or (_front And LEFTCOLUMN)
                'TL =0 TL | ML =0 ML | BL =0 BL
                _front = (_front And NOTLEFTCOLUMN) Or (_top And LEFTCOLUMN)
                'TL =0 TL | ML =0 ML | BL =0 BL
                _top = (_top And NOTLEFTCOLUMN) Or ((_back And BR) >> TS4) Or (((_back And MR) Or (_back And TR)) << TS4)
                'BR >4 TL | MR <4 ML | TR <4 BL
                _back = (_back And NOTRIGHTCOLUMN) Or ((tmp And TL) << TS4) Or (((tmp And ML) Or (tmp And BL)) >> TS4)
                'TL <4 BR | ML >4 MR | BL >4 TR

            Case RubiksMove.LeftCounterClockwise
                _left = (_left And MM) Or (((_left And TL) Or (_left And TM)) << TS6) Or ((_left And CCC) >> TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTLEFTCOLUMN) Or ((_back And BR) >> TS4) Or (((_back And MR) Or (_back And TR)) << TS4)
                'BR >4 TL | MR <4 ML | TR <4 BL
                _back = (_back And NOTRIGHTCOLUMN) Or ((_top And TL) << TS4) Or (((_top And ML) Or (_top And BL)) >> TS4)
                'TL <4 BR | ML >4 MR | BL >4 TR
                _top = (_top And NOTLEFTCOLUMN) Or (_front And LEFTCOLUMN)
                'TL =0 TL | ML =0 ML | BL =0 BL
                _front = (_front And NOTLEFTCOLUMN) Or (tmp And LEFTCOLUMN)
                'TL =0 TL | ML =0 ML | BL =0 BL

            Case RubiksMove.RightClockwise
                _right = (_right And MM) Or (((_right And BL) Or (_right And ML)) >> TS6) Or ((_right And CC) << TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTRIGHTCOLUMN) Or (((_back And BL) Or (_back And ML)) >> TS4) Or ((_back And TL) << TS4)
                'BL >4 TR | ML >4 MR | TL <4 BR
                _back = (_back And NOTLEFTCOLUMN) Or (((_top And TR) Or (_top And MR)) << TS4) Or ((_top And BR) >> TS4)
                'TR <4 BL | MR <4 ML | BR >4 TL
                _top = (_top And NOTRIGHTCOLUMN) Or (_front And RIGHTCOLUMN)
                'BR =0 BR | MR =0 MR | TR =0 TR
                _front = (_front And NOTRIGHTCOLUMN) Or (tmp And RIGHTCOLUMN)
                'BR =0 BR | MR =0 MR | TR =0 TR

            Case RubiksMove.RightCounterClockwise
                _right = (_right And MM) Or (((_right And TL) Or (_right And TM)) << TS6) Or ((_right And CCC) >> TS2)
                tmp = _bottom
                _bottom = (_bottom And NOTRIGHTCOLUMN) Or (_front And RIGHTCOLUMN)
                'BR =0 BR | MR =0 MR | TR =0 TR
                _front = (_front And NOTRIGHTCOLUMN) Or (_top And RIGHTCOLUMN)
                'BR =0 BR | MR =0 MR | TR =0 TR
                _top = (_top And NOTRIGHTCOLUMN) Or (((_back And BL) Or (_back And ML)) >> TS4) Or ((_back And TL) << TS4)
                'BL >4 TR | ML >4 MR | TL <4 BR
                _back = (_back And NOTLEFTCOLUMN) Or (((tmp And TR) Or (tmp And MR)) << TS4) Or ((tmp And BR) >> TS4)
                'TR <4 BL | MR <4 ML | BR >4 TL

            Case RubiksMove.TopClockwise
                _top = (_top And MM) Or (((_top And BL) Or (_top And ML)) >> TS6) Or ((_top And CC) << TS2)
                tmp = _right
                _right = (_right And NOTTOPROW) Or (_back And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR
                _back = (_back And NOTTOPROW) Or (_left And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR
                _left = (_left And NOTTOPROW) Or (_front And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR
                _front = (_front And NOTTOPROW) Or (tmp And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR

            Case RubiksMove.TopCounterClockwise
                _top = (_top And MM) Or (((_top And TL) Or (_top And TM)) << TS6) Or ((_top And CCC) >> TS2)
                tmp = _right
                _right = (_right And NOTTOPROW) Or (_front And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR
                _front = (_front And NOTTOPROW) Or (_left And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR
                _left = (_left And NOTTOPROW) Or (_back And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR
                _back = (_back And NOTTOPROW) Or (tmp And TOPROW)
                'TL =0 TL | TM =0 TM | TR =0 TR

            Case RubiksMove.BottomClockwise
                _bottom = (_bottom And MM) Or (((_bottom And BL) Or (_bottom And ML)) >> TS6) Or ((_bottom And CC) << TS2)
                tmp = _right
                _right = (_right And NOTBOTTOMROW) Or (_front And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR
                _front = (_front And NOTBOTTOMROW) Or (_left And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR
                _left = (_left And NOTBOTTOMROW) Or (_back And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR
                _back = (_back And NOTBOTTOMROW) Or (tmp And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR

            Case RubiksMove.BottomCounterClockwise
                _bottom = (_bottom And MM) Or (((_bottom And TL) Or (_bottom And TM)) << TS6) Or ((_bottom And CCC) >> TS2)
                tmp = _right
                _right = (_right And NOTBOTTOMROW) Or (_back And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR
                _back = (_back And NOTBOTTOMROW) Or (_left And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR
                _left = (_left And NOTBOTTOMROW) Or (_front And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR
                _front = (_front And NOTBOTTOMROW) Or (tmp And BOTTOMROW)
                'BL =0 BL | BM =0 BM | BR =0 BR

            Case RubiksMove.FrontBackAxisClockwise
                _back = (_back And MM) Or (((_back And TL) Or (_back And TM)) << TS6) Or ((_back And CCC) >> TS2)
                _front = (_front And MM) Or (((_front And BL) Or (_front And ML)) >> TS6) Or ((_front And CC) << TS2)
                tmp = _left
                _left = (_bottom And MM) Or ((_bottom And CC) << TS2) Or (((_bottom And BL) Or (_bottom And ML)) >> TS6)
                _bottom = (_right And MM) Or ((_right And CC) << TS2) Or (((_right And BL) Or (_right And ML)) >> TS6)
                _right = (_top And MM) Or ((_top And CC) << TS2) Or (((_top And BL) Or (_top And ML)) >> TS6)
                _top = (tmp And MM) Or ((tmp And CC) << TS2) Or (((tmp And BL) Or (tmp And ML)) >> TS6)

            Case RubiksMove.FrontBackAxisCounterClockwise
                _back = (_back And MM) Or (((_back And BL) Or (_back And ML)) >> TS6) Or ((_back And CC) << TS2)
                _front = (_front And MM) Or (((_front And TL) Or (_front And TM)) << TS6) Or ((_front And CCC) >> TS2)
                tmp = _left
                _left = (_top And MM) Or (((_top And TL) Or (_top And TM)) << TS6) Or ((_top And CCC) >> TS2)
                _top = (_right And MM) Or (((_right And TL) Or (_right And TM)) << TS6) Or ((_right And CCC) >> TS2)
                _right = (_bottom And MM) Or (((_bottom And TL) Or (_bottom And TM)) << TS6) Or ((_bottom And CCC) >> TS2)
                _bottom = (tmp And MM) Or (((tmp And TL) Or (tmp And TM)) << TS6) Or ((tmp And CCC) >> TS2)

            Case RubiksMove.TopBottomAxisClockwise
                _top = (_top And MM) Or (((_top And BL) Or (_top And ML)) >> TS6) Or ((_top And CC) << TS2)
                _bottom = (_bottom And MM) Or (((_bottom And TL) Or (_bottom And TM)) << TS6) Or ((_bottom And CCC) >> TS2)

                tmp = _left
                _left = _front
                _front = _right
                _right = _back
                _back = tmp
            Case RubiksMove.TopBottomAxisCounterClockwise
                _top = (_top And MM) Or (((_top And TL) Or (_top And TM)) << TS6) Or ((_top And CCC) >> TS2)
                _bottom = (_bottom And MM) Or (((_bottom And BL) Or (_bottom And ML)) >> TS6) Or ((_bottom And CC) << TS2)

                tmp = _left
                _left = _back
                _back = _right
                _right = _front
                _front = tmp

            Case RubiksMove.LeftRightAxisClockwise
                _left = (_left And MM) Or (((_left And BL) Or (_left And ML)) >> TS6) Or ((_left And CC) << TS2)
                _right = (_right And MM) Or (((_right And TL) Or (_right And TM)) << TS6) Or ((_right And CCC) >> TS2)

                tmp = _front
                _front = _top
                _top = (_back And MM) Or ((_back And (TL Or TM Or TR Or MR)) << TS4) Or ((_back And (BR Or BM Or BL Or ML)) >> TS4)
                _back = (_bottom And MM) Or ((_bottom And (TL Or TM Or TR Or MR)) << TS4) Or ((_bottom And (BR Or BM Or BL Or ML)) >> TS4)
                _bottom = tmp

            Case RubiksMove.LeftRightAxisCounterClockwise
                _left = (_left And MM) Or (((_left And TL) Or (_left And TM)) << TS6) Or ((_left And CCC) >> TS2)
                _right = (_right And MM) Or (((_right And BL) Or (_right And ML)) >> TS6) Or ((_right And CC) << TS2)

                tmp = _front
                _front = _bottom
                _bottom = (_back And MM) Or ((_back And (TL Or TM Or TR Or MR)) << TS4) Or ((_back And (BR Or BM Or BL Or ML)) >> TS4)
                _back = (_top And MM) Or ((_top And (TL Or TM Or TR Or MR)) << TS4) Or ((_top And (BR Or BM Or BL Or ML)) >> TS4)
                _top = tmp

            Case Else
                Throw New InvalidOperationException("You chose a bad move")
        End Select
    End Sub

    Public Function GetFace(face As FaceIndicator) As CubeFace
        Dim returnValue As New CubeFace()

        Dim faceValue As Integer
        Select Case face
            Case FaceIndicator.Front
                faceValue = _front
            Case FaceIndicator.Back
                faceValue = _back
            Case FaceIndicator.Top
                faceValue = _top
            Case FaceIndicator.Bottom
                faceValue = _bottom
            Case FaceIndicator.Left
                faceValue = _left
            Case FaceIndicator.Right
                faceValue = _right
        End Select

        returnValue.MiddleMiddle = faceValue And MM
        returnValue.TopLeft = (faceValue And TL) >> TS1
        returnValue.TopMiddle = (faceValue And TM) >> TS2
        returnValue.TopRight = (faceValue And TR) >> TS3
        returnValue.MiddleRight = (faceValue And MR) >> TS4
        returnValue.BottomRight = (faceValue And BR) >> TS5
        returnValue.BottomMiddle = (faceValue And BM) >> TS6
        returnValue.BottomLeft = (faceValue And BL) >> TS7
        returnValue.MiddleLeft = (faceValue And ML) >> TS8

        Return returnValue
    End Function

    Public Function isSolved() As Boolean
        Return IsFaceSolid(_front) AndAlso IsFaceSolid(_back) AndAlso IsFaceSolid(_left) AndAlso IsFaceSolid(_right) AndAlso IsFaceSolid(_bottom) AndAlso IsFaceSolid(_top)
    End Function

    Private Function IsFaceSolid(face As Integer)
        Dim middleTile As Integer = (face And MM)

        For i As Integer = TS1 To TS8 Step TS1
            If middleTile <> ((face >> i) And MM) Then
                Return False
            End If
        Next

        Return True
    End Function
End Class
