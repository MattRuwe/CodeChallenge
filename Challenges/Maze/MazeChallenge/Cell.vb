''' <summary>
''' Represents a single position within a maze from which has walls, borders, and other properties
''' </summary>
''' <remarks></remarks>
Public Class Cell
    Private ReadOnly _cellValue As Integer

    ''' <summary>
    ''' Constructs a new cell from a bitwise integer value
    ''' </summary>
    ''' <param name="cellValue"></param>
    ''' <remarks>See <see cref="P:OmahaMTG.Challenge.Challenges.Cell.CellValue" /> for details on the bitwise definition.</remarks>
    Friend Sub New(cellValue As Integer)
        _cellValue = cellValue
    End Sub

    ''' <summary>
    ''' Determines if a given move is valid from the current cell (i.e. is the desired direction free of a wall)
    ''' </summary>
    ''' <param name="direction">The direction from the current cell to check for a wall</param>
    ''' <returns>boolean indicating that there is no wall blocking the given direction</returns>
    ''' <remarks></remarks>
    Public Function IsValidMove(direction As Direction) As Boolean
        If direction = Challenges.Direction.None Then
            Return False
        ElseIf direction = Challenges.Direction.North AndAlso NorthWall Then
            Return False
        ElseIf direction = Challenges.Direction.East AndAlso EastWall Then
            Return False
        ElseIf direction = Challenges.Direction.South AndAlso SouthWall Then
            Return False
        ElseIf direction = Challenges.Direction.West AndAlso WestWall Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' The value of a cell
    ''' </summary>
    ''' <returns>The bitwise integer value used to store the details of the cell.</returns>
    ''' <remarks>
    ''' This value is generally not needed except when performance might be a consideration.  The bitwise definition of this property is as follows:<br />
    ''' Walls<br />
    ''' 1 - N<br />
    ''' 2 - E<br />
    ''' 3 - S<br />
    ''' 4 - W<br />
    ''' Borders (A border surrounds the outside of the maze)<br />
    ''' 5 - N<br />
    ''' 6 - E<br />
    ''' 7 - S<br />
    ''' 8 - W<br />
    ''' Solution (The entry has travelled this direction at least once)<br />
    ''' 9 - N<br />
    ''' 10 - E<br />
    ''' 11 - S<br />
    ''' 12 - W<br />
    ''' Backtrack (The entry has travelled this direction 2 or more times)<br />
    ''' 13 - N<br />
    ''' 14 - E<br />
    ''' 15 - S<br />
    ''' 16 - W<br />
    ''' Location<br />
    ''' 17 - Is Starting Location<br />
    ''' 18 - Is Ending Location<br />
    ''' All further bits are ignored
    ''' </remarks>
    Public ReadOnly Property CellValue As Integer
        Get
            Return _cellValue
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the NorthWall
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the North wall in the current cell</returns>
    ReadOnly Property NorthWall As Boolean
        Get
            Return (_cellValue And NORTH_WALL) = NORTH_WALL
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the EastWall
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the East wall in the current cell</returns>
    ReadOnly Property EastWall As Boolean
        Get
            Return (_cellValue And EAST_WALL) = EAST_WALL
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the SouthWall
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the South wall in the current cell</returns>
    ReadOnly Property SouthWall As Boolean
        Get
            Return (_cellValue And SOUTH_WALL) = SOUTH_WALL
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the WestWall
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the West wall in the current cell</returns>
    ReadOnly Property WestWall As Boolean
        Get
            Return (_cellValue And WEST_WALL) = WEST_WALL
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the NorthBorder
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the North border in the current cell</returns>
    ReadOnly Property NorthBorder As Boolean
        Get
            Return (_cellValue And NORTH_BORDER) = NORTH_BORDER
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the EastBorder
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the East border in the current cell</returns>
    ReadOnly Property EastBorder As Boolean
        Get
            Return (_cellValue And EAST_BORDER) = EAST_BORDER
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the SouthBorder
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the South border in the current cell</returns>
    ReadOnly Property SouthBorder As Boolean
        Get
            Return (_cellValue And SOUTH_BORDER) = SOUTH_BORDER
        End Get
    End Property

    ''' <summary>
    ''' Gets the value of the WestBorder
    ''' </summary>
    ''' <returns>A boolean indicating the presence of the West border in the current cell</returns>
    ReadOnly Property WestBorder As Boolean
        Get
            Return (_cellValue And WEST_BORDER) = WEST_BORDER
        End Get
    End Property

    ''' <summary>
    ''' Gets the North solution value
    ''' </summary>
    ''' <returns>A boolean indicating that the maze solution travels through this cell (i.e. at least one move was made North through this cell).</returns>
    ''' <remarks>Note that simply because the solution flag is set does not guarantee that this direction is the actual solution.  This only means that
    ''' this direction was travelled once during the solving of the maze.
    ''' <br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property NorthSolution As Boolean
        Get
            Return (_cellValue And NORTH_SOLUTION) = NORTH_SOLUTION

        End Get
    End Property

    ''' <summary>
    ''' Gets the East solution value
    ''' </summary>
    ''' <returns>A boolean indicating that the maze solution travels through this cell (i.e. at least one move was made East through this cell).</returns>
    ''' <remarks>Note that simply because the solution flag is set does not guarantee that this direction is the actual solution.  This only means that
    ''' this direction was travelled once during the solving of the maze.
    ''' <br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property EastSolution As Boolean
        Get
            Return (_cellValue And EAST_SOLUTION) = EAST_SOLUTION
        End Get
    End Property

    ''' <summary>
    ''' Gets the South solution value
    ''' </summary>
    ''' <returns>A boolean indicating that the maze solution travels through this cell (i.e. at least one move was made South through this cell).</returns>
    ''' <remarks>Note that simply because the solution flag is set does not guarantee that this direction is the actual solution.  This only means that
    ''' this direction was travelled once during the solving of the maze.
    ''' <br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property SouthSolution As Boolean
        Get
            Return (_cellValue And SOUTH_SOLUTION) = SOUTH_SOLUTION
        End Get
    End Property

    ''' <summary>
    ''' Gets the West solution value
    ''' </summary>
    ''' <returns>A boolean indicating that the maze solution travels through this cell (i.e. at least one move was made West through this cell).</returns>
    ''' <remarks>Note that simply because the solution flag is set does not guarantee that this direction is the actual solution.  This only means that
    ''' this direction was travelled once during the solving of the maze.
    ''' <br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property WestSolution As Boolean
        Get
            Return (_cellValue And WEST_SOLUTION) = WEST_SOLUTION
        End Get
    End Property

    ''' <summary>
    ''' Gets the North backtrack value
    ''' </summary>
    ''' <returns>A boolean indicating that this direction was tavelled 2 or more times during the solution of the maze.</returns>
    ''' <remarks><br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property NorthBacktrack As Boolean
        Get
            Return (_cellValue And NORTH_BACKTRACK) = NORTH_BACKTRACK

        End Get
    End Property

    ''' <summary>
    ''' Gets the East backtrack value
    ''' </summary>
    ''' <returns>A boolean indicating that this direction was tavelled 2 or more times during the solution of the maze.</returns>
    ''' <remarks><br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property EastBacktrack As Boolean
        Get
            Return (_cellValue And EAST_BACKTRACK) = EAST_BACKTRACK
        End Get
    End Property

    ''' <summary>
    ''' Gets the South backtrack value
    ''' </summary>
    ''' <returns>A boolean indicating that this direction was tavelled 2 or more times during the solution of the maze.</returns>
    ''' <remarks><br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property SouthBacktrack As Boolean
        Get
            Return (_cellValue And SOUTH_BACKTRACK) = SOUTH_BACKTRACK
        End Get
    End Property

    ''' <summary>
    ''' Gets the West backtrack value
    ''' </summary>
    ''' <returns>A boolean indicating that this direction was tavelled 2 or more times during the solution of the maze.</returns>
    ''' <remarks><br />Hint:  You should not need to use this property.</remarks>
    ReadOnly Property WestBacktrack As Boolean
        Get
            Return (_cellValue And WEST_BACKTRACK) = WEST_BACKTRACK
        End Get
    End Property

    ''' <summary>
    ''' Gets a boolean indicating that this is the start cell for the maze
    ''' </summary>
    ''' <returns>A boolean indicating that this is the start cell for the maze</returns>
    ''' <remarks></remarks>
    ReadOnly Property IsStart As Boolean
        Get
            Return (_cellValue And START) = START
        End Get
    End Property

    ''' <summary>
    ''' Gets a boolean indicating that this is the end cell for the maze
    ''' </summary>
    ''' <returns>A boolean indicating that this is the end cell for the maze</returns>
    ''' <remarks></remarks>
    ReadOnly Property IsEnd As Boolean
        Get
            Return (_cellValue And [END]) = [END]
        End Get
    End Property

End Class
