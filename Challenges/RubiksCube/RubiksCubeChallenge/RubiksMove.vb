''' <summary>
''' An enumeration that contains the different possible moves that can be made to a Rubik's cube.
''' </summary>
''' <remarks>This enumeration is used to modify the <see cref="T:OmahaMTG.Challenge.Challenges.RubiksCube" /> data structure.</remarks>
<Serializable()>
Public Enum RubiksMove
    ''' <summary>
    ''' Don't make any move
    ''' </summary>
    ''' <remarks>You should generally avoid using this value as although it will not modify the <see cref="T:OmahaMTG.Challenge.Challenges.RubiksCube" />
    ''' the move will still be taken into account while scoring your solution.</remarks>
    None = 0
    ''' <summary>
    ''' Rotate the front face clockwise
    ''' </summary>
    ''' <remarks></remarks>
    FrontClockwise = 1
    ''' <summary>
    ''' Rotate the front face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    FrontCounterClockwise = 2
    ''' <summary>
    ''' Rotate the back face clockwise
    ''' </summary>
    ''' <remarks></remarks>
    BackClockwise = 3
    ''' <summary>
    ''' Rotate the back face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    BackCounterClockwise = 4
    ''' <summary>
    ''' Rotate the left face clockwise
    ''' </summary>
    ''' <remarks></remarks>
    LeftClockwise = 5
    ''' <summary>
    ''' Rotate the left face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    LeftCounterClockwise = 6
    ''' <summary>
    ''' Rotate the right face clockwise
    ''' </summary>
    ''' <remarks></remarks>
    RightClockwise = 7
    ''' <summary>
    ''' Rotate the right face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    RightCounterClockwise = 8
    ''' <summary>
    ''' Rotate the top face clockwise
    ''' </summary>
    ''' <remarks></remarks>
    TopClockwise = 9
    ''' <summary>
    ''' Rotate the top face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    TopCounterClockwise = 10
    ''' <summary>
    ''' Rotate the down face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    BottomClockwise = 11
    ''' <summary>
    ''' Rotate the down face counterclockwise
    ''' </summary>
    ''' <remarks></remarks>
    BottomCounterClockwise = 12
    ''' <summary>
    ''' Rotate the entire cube clockwise on the axis that goes through the front and back faces, turning the front face clockwise.  
    ''' The perspective of rotation is from the front (i.e. the front will be rotated clockwise)
    ''' </summary>
    FrontBackAxisClockwise = 13
    ''' <summary>
    ''' Rotate the entire cube counter-clockwise on the axis that goes through the front and back faces.  
    ''' The perspective of rotation is from the front (i.e. the front will be rotated counter-clockwise).
    ''' </summary>
    FrontBackAxisCounterClockwise = 14
    ''' <summary>
    ''' Rotate the entire cube clockwise on the axis that goes through the top and bottom faces.  
    ''' The perspective of rotation is from the top (i.e. the top will be rotated clockwise)
    ''' </summary>
    TopBottomAxisClockwise = 15
    ''' <summary>
    ''' Rotate the entire cube counter-clockwise on the axis that goes through the top and bottom faces.  
    ''' The perspective of rotation is from the top (i.e. the top will be rotated counter-clockwise)
    ''' </summary>
    TopBottomAxisCounterClockwise = 16
    ''' <summary>
    ''' Rotate the entire cube clockwise on the axis that goes through the left and right faces.  
    ''' The perspective of rotation is from the left (i.e. the left will be rotated clockwise)
    ''' </summary>
    LeftRightAxisClockwise = 17
    ''' <summary>
    ''' Rotate the entire cube clockwise on the axis that goes through the left and right faces.  
    ''' The perspective of rotation is from the left (i.e. the left will be rotated counter-clockwise)
    ''' </summary>
    LeftRightAxisCounterClockwise = 18


End Enum
