''' <summary>
''' A list of possible directions
''' </summary>
''' <remarks></remarks>
Public Enum Direction
    ''' <summary>
    ''' No direction is indicated
    ''' </summary>
    None = 0
    ''' <summary>
    ''' Indicates a direction toward the cell above the current cell
    ''' </summary>
    North = 1
    ''' <summary>
    ''' Indicates a direction toward the cell to the right of the current cell
    ''' </summary>
    East = 2
    ''' <summary>
    ''' Indicates a direction toward the cell below the current cell
    ''' </summary>
    South = 3
    ''' <summary>
    ''' Indicates a direction toward the cell to the left of the current cell
    ''' </summary>
    West = 4
End Enum
