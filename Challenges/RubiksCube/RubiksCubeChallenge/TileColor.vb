''' <summary>
''' Represents the color of a given tile on the <see cref="T:OmahaMTG.Challenge.Challenges.RubiksCube" />
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Enum TileColor
    ''' <summary>
    ''' No color has been assigned to this tile (generally an error condition)
    ''' </summary>
    ''' <remarks></remarks>
    None = 0
    ''' <summary>
    ''' The tile color is white
    ''' </summary>
    ''' <remarks></remarks>
    White = 1
    ''' <summary>
    ''' The tile color is red
    ''' </summary>
    ''' <remarks></remarks>
    Red = 2
    ''' <summary>
    ''' The tile color is blue
    ''' </summary>
    ''' <remarks></remarks>
    Blue = 3
    ''' <summary>
    ''' The tile color is orange
    ''' </summary>
    ''' <remarks></remarks>
    Orange = 4
    ''' <summary>
    ''' The tile color is green
    ''' </summary>
    ''' <remarks></remarks>
    Green = 5
    ''' <summary>
    ''' The tile color is yellow
    ''' </summary>
    ''' <remarks></remarks>
    Yellow = 6
End Enum
