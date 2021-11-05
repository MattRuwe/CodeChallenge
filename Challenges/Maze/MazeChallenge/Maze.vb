Imports System.Drawing
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security
Imports System.Security.Permissions
Imports System.Text
Imports System.IO.Compression
Imports System.Drawing.Imaging

''' <summary>
''' Represents a 2D maze as a square of cells with walls and borders
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class Maze

    'Maze Data Structure - (Row, Column)
    Private _mazeBoardData As Integer(,)
    Private _startIndex As CellIndex
    Private _endIndex As CellIndex

    'Bit mapping
    'Walls
    '1 - N
    '2 - E
    '3 - S
    '4 - W
    'Borders
    '5 - N
    '6 - E
    '7 - S
    '8 - W
    'Solution
    '9 - N
    '10 - E
    '11 - S
    '12 - W
    'Backtrack
    '13 - N
    '14 - E
    '15 - S
    '16 - W
    'Location
    '17 - Is Starting Location
    '18 - Is Ending Location

    ''' <summary>
    ''' Constructs a new square maze with a size specified
    ''' </summary>
    ''' <param name="size">The height and width of the maze</param>
    ''' <remarks></remarks>
    Public Sub New(size As Integer)
        ReDim _mazeBoardData(size, size)
    End Sub

    ''' <summary>
    ''' Constructs a new maze from a serialized maze.
    ''' </summary>
    ''' <param name="serializedMaze">The serialized maze</param>
    ''' <remarks>This method is used to support the maze challenge executor.  You should not use it in your code.</remarks>
    <SecuritySafeCritical()>
    Public Sub New(serializedMaze As String)
        DeserializeMaze(serializedMaze)
    End Sub

    Private Sub DeserializeMaze(serializedMaze As String)

        Dim gZipBuffer As Byte() = Convert.FromBase64String(serializedMaze)
        Using memoryStream As New MemoryStream()

            Dim dataLength As Integer = BitConverter.ToInt32(gZipBuffer, 0)
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4)

            Dim buffer(dataLength) As Byte

            memoryStream.Position = 0
            Using gZipStream = New GZipStream(memoryStream, CompressionMode.Decompress)

                gZipStream.Read(buffer, 0, buffer.Length)
            End Using

            serializedMaze = Encoding.UTF8.GetString(buffer)
        End Using

        Dim lines As String() = serializedMaze.Split(vbCrLf)
        Dim line As String() = lines(0).Split(" ")

        ReDim _mazeBoardData(line(0), line(1))

        For row As Integer = 0 To _mazeBoardData.GetUpperBound(0)
            line = lines(row + 1).Split(" ")
            For col As Integer = 0 To _mazeBoardData.GetUpperBound(1)
                _mazeBoardData(row, col) = Integer.Parse(line(col))
            Next
        Next
    End Sub

    Private Sub New(mazeBoardData As Integer(,))
        _mazeBoardData = mazeBoardData
    End Sub

    ''' <summary>
    ''' Creates a copy of the maze
    ''' </summary>
    ''' <returns>A new instance that is equivalent to the original instance</returns>
    ''' <remarks>This method is used to support the maze challenge executor.  You should not use it in your code.</remarks>
    Public Function Clone() As Maze
        Dim newMazeData(_mazeBoardData.GetUpperBound(0), _mazeBoardData.GetUpperBound(1)) As Integer

        For row As Integer = 0 To _mazeBoardData.GetUpperBound(0)
            For col As Integer = 0 To _mazeBoardData.GetUpperBound(1)
                newMazeData(row, col) = _mazeBoardData(row, col)
            Next
        Next

        Dim returnValue As New Maze(newMazeData)

        Return returnValue
    End Function

    ''' <summary>
    ''' Serializes the current maze object into a string
    ''' </summary>
    ''' <returns>A base64 encoded string containing the serialized version of the maze</returns>
    ''' <remarks>This method is used to support the maze challenge executor.  You should not use it in your code.</remarks>
    Public Function GetSerializedMaze() As String
        Dim mazeString As New StringBuilder

        mazeString.AppendLine(_mazeBoardData.GetUpperBound(0) & " " & _mazeBoardData.GetUpperBound(1))
        For row As Integer = 0 To _mazeBoardData.GetUpperBound(0)
            For col As Integer = 0 To _mazeBoardData.GetUpperBound(1)
                mazeString.Append(_mazeBoardData(row, col).ToString())
                mazeString.Append(" ")
            Next
            mazeString.AppendLine()
        Next

        Dim mazeBytes As Byte() = Encoding.UTF8.GetBytes(mazeString.ToString())

        Dim memoryStream = New MemoryStream()
        Using gZipStream = New GZipStream(memoryStream, CompressionMode.Compress, True)
            gZipStream.Write(mazeBytes, 0, mazeBytes.Length)
        End Using

        memoryStream.Position = 0

        Dim compressedData = New Byte(memoryStream.Length - 1) {}
        memoryStream.Read(compressedData, 0, compressedData.Length)

        Dim gZipBuffer = New Byte(compressedData.Length + 3) {}
        Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length)
        Buffer.BlockCopy(BitConverter.GetBytes(mazeBytes.Length), 0, gZipBuffer, 0, 4)
        Return Convert.ToBase64String(gZipBuffer)
    End Function

    Private Sub CheckCurrentIndex()
        If _currentCellIndex Is Nothing Then
            _currentCellIndex = GetStartIndex()
            If _currentCellIndex Is Nothing Then
                _currentCellIndex = New CellIndex(0, 0)
            End If
        End If
    End Sub

    Private _currentCellIndex As CellIndex
    ''' <summary>
    ''' A CellIndex indicating the cell that is currently selected.
    ''' </summary>
    ''' <returns>The CellIndex of the currently selected cell.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CurrentIndex As CellIndex
        Get
            CheckCurrentIndex()
            Return _currentCellIndex
        End Get
    End Property

    ''' <summary>
    ''' The cell that is currently selected
    ''' </summary>
    ''' <returns>The cell that is currently selected</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CurrentCell As Cell
        Get
            CheckCurrentIndex()
            Return GetCell(_currentCellIndex)
        End Get
    End Property

    ''' <summary>
    ''' Gets a Cell giving a cellindex
    ''' </summary>
    ''' <param name="cellIndex">The index of the cell to retrieve</param>
    ''' <returns>A cell corresponding to the cellIndex property.</returns>
    Public Function GetCell(cellIndex As CellIndex) As Cell
        If cellIndex Is Nothing Then
            Throw New ArgumentException("Parameter cannot be null", "cellIndex")
        End If
        Dim cellValue As Integer = _mazeBoardData(cellIndex.Row, cellIndex.Column)

        Dim returnValue As New Cell(cellValue)

        Return returnValue
    End Function

    ''' <summary>
    ''' Changes the current cell in the direction requested
    ''' </summary>
    ''' <param name="direction">The direction of the cell to move in</param>
    ''' <remarks>When moving the current cell, the corresponding solution and backtrack values are automatically set.  In essense, if the cell has not yet been visited, the solution 
    ''' flag for the specific direction is turned on.  If the cell has already been visited for the direction specific, then the backtrack value for that direction is also set.</remarks>
    Public Sub MoveCurrentCell(direction As Direction)
        If (direction = Challenges.Direction.North And Me.CurrentCell.NorthWall) OrElse
            (direction = Challenges.Direction.East And Me.CurrentCell.EastWall) OrElse
            (direction = Challenges.Direction.South And Me.CurrentCell.SouthWall) OrElse
            (direction = Challenges.Direction.West And Me.CurrentCell.WestWall) Then
            Throw New IllegalMoveException()
        End If

        Dim currentCell As Cell = GetCell(_currentCellIndex)
        Dim nextCellIndex As CellIndex
        Dim nextCell As Cell

        If direction = Challenges.Direction.North Then
            nextCellIndex = New CellIndex(_currentCellIndex.Row - 1, _currentCellIndex.Column)
            nextCell = GetCell(nextCellIndex)
            If currentCell.NorthSolution Then
                'Already visited this cell - set the north backtrack as on
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or NORTH_BACKTRACK
            Else
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or NORTH_SOLUTION
            End If
            If nextCell.SouthSolution Then
                'Already visited this cell - set the south backtrack as on
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or SOUTH_BACKTRACK
            Else
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or SOUTH_SOLUTION
            End If
        ElseIf direction = Challenges.Direction.East Then
            nextCellIndex = New CellIndex(_currentCellIndex.Row, _currentCellIndex.Column + 1)
            nextCell = GetCell(nextCellIndex)
            If currentCell.EastSolution Then
                'Already visited this cell - set the north backtrack as on
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or EAST_BACKTRACK
            Else
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or EAST_SOLUTION
            End If
            If nextCell.WestSolution Then
                'Already visited this cell - set the south backtrack as on
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or WEST_BACKTRACK
            Else
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or WEST_SOLUTION
            End If
        ElseIf direction = Challenges.Direction.South Then
            nextCellIndex = New CellIndex(_currentCellIndex.Row + 1, _currentCellIndex.Column)
            nextCell = GetCell(nextCellIndex)
            If currentCell.SouthSolution Then
                'Already visited this cell - set the north backtrack as on
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or SOUTH_BACKTRACK
            Else
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or SOUTH_SOLUTION
            End If
            If nextCell.NorthSolution Then
                'Already visited this cell - set the south backtrack as on
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or NORTH_BACKTRACK
            Else
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or NORTH_SOLUTION
            End If
        ElseIf direction = Challenges.Direction.West Then
            nextCellIndex = New CellIndex(_currentCellIndex.Row, _currentCellIndex.Column - 1)
            nextCell = GetCell(nextCellIndex)
            If currentCell.WestSolution Then
                'Already visited this cell - set the north backtrack as on
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or WEST_BACKTRACK
            Else
                _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) = _mazeBoardData(_currentCellIndex.Row, _currentCellIndex.Column) Or WEST_SOLUTION
            End If
            If nextCell.EastSolution Then
                'Already visited this cell - set the south backtrack as on
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or EAST_BACKTRACK
            Else
                _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) = _mazeBoardData(nextCellIndex.Row, nextCellIndex.Column) Or EAST_SOLUTION
            End If
        Else
            Throw New InvalidOperationException("An unreachable code block was reached.")
        End If

        _currentCellIndex = nextCellIndex

    End Sub

    Private Function GetDirection(cellIndex As CellIndex, previousCellIndex As CellIndex) As Direction
        Dim returnValue As Direction

        If (Not (cellIndex.Row <> previousCellIndex.Row Xor cellIndex.Column <> previousCellIndex.Column)) Then
            Throw New InvalidOperationException("The cells do not appear to be next to one another.")
        End If

        If cellIndex.Row - previousCellIndex.Row = -1 Then
            'Moved North
            returnValue = Direction.North
        ElseIf cellIndex.Row - previousCellIndex.Row = 1 Then
            'Moved South
            returnValue = Direction.South
        ElseIf cellIndex.Column - previousCellIndex.Column = -1 Then
            'Moved West
            returnValue = Direction.West
        ElseIf cellIndex.Column - previousCellIndex.Column = 1 Then
            'Moved East
            returnValue = Direction.East
        Else
            Throw New InvalidOperationException("Tried to determine the direction of a move between two cells that were not next to one another.")
        End If

        Return returnValue
    End Function

    ''' <summary>
    ''' Builds a random maze
    ''' </summary>
    ''' <param name="difficulty">A value between 0 and 100 indicating how probable a straight line is.  The longer a straight line, the less difficult a maze is to solve.</param>
    ''' <remarks>This method is used to support the maze challenge executor.  You should not use it in your entry code, however it may be useful during testing.</remarks>
    Public Sub GenerateMaze(difficulty As Integer)
        If difficulty < 0 Or difficulty > 100 Then
            Throw New ArgumentException("Value must be between 0 and 100 inclusive", "difficulty")
        End If
        InitializeMaze()

        Dim rand As New Random()
        Dim currentIndex As New CellIndex With {.Row = rand.Next(0, _mazeBoardData.GetUpperBound(0) + 1), .Column = rand.Next(0, _mazeBoardData.GetUpperBound(0) + 1)}
        Dim totalCells = (_mazeBoardData.GetUpperBound(0) + 1) * (_mazeBoardData.GetUpperBound(1) + 1)
        Dim visitedCells = 1
        Dim indexStack As New Stack(Of CellIndex)
        Dim lastMove As Direction

        While visitedCells < totalCells

            'Find all neighbors of current
            Dim neighborsWithAllWallsIntact As List(Of CellIndex) = GetInTactNeighbors(currentIndex)
            Dim directionsWithIntactWalls As New List(Of Direction)
            neighborsWithAllWallsIntact.ForEach(Sub(cellIndex As CellIndex) directionsWithIntactWalls.Add(GetDirection(cellIndex, currentIndex)))

            If neighborsWithAllWallsIntact.Count > 0 Then
                indexStack.Push(currentIndex)
                Dim previousMoveIndex As Integer = directionsWithIntactWalls.FindIndex(Function(direction) direction = lastMove)
                Dim newIndex As CellIndex
                If previousMoveIndex > -1 AndAlso rand.Next(0, 101) > difficulty Then
                    newIndex = neighborsWithAllWallsIntact(previousMoveIndex)
                Else
                    newIndex = neighborsWithAllWallsIntact(rand.Next(0, neighborsWithAllWallsIntact.Count))
                End If

                lastMove = GetDirection(newIndex, currentIndex)
                RemoveWallsBetweenIndexes(currentIndex, newIndex, WallType.Standard)
                currentIndex = newIndex
                visitedCells += 1
            Else
                currentIndex = indexStack.Pop
            End If

        End While

        'Dim startRandomRow As Integer = rand.Next(0, _mazeBoardData.GetUpperBound(0) + 1)
        'Dim endRandomRow As Integer = rand.Next(0, _mazeBoardData.GetUpperBound(0) + 1)

        'While (_mazeBoardData(endRandomRow, _mazeBoardData.GetUpperBound(1)) And SOUTH_WALL) <> SOUTH_WALL AndAlso (_mazeBoardData(endRandomRow, _mazeBoardData.GetUpperBound(1)) And SOUTH_BORDER) <> SOUTH_BORDER
        '    endRandomRow += 1
        'End While

        'While (_mazeBoardData(startRandomRow, 0) And NORTH_WALL) <> NORTH_WALL AndAlso (_mazeBoardData(startRandomRow, 0) And NORTH_BORDER) <> NORTH_BORDER
        '    startRandomRow -= 1
        'End While

        '_mazeBoardData(startRandomRow, 0) = _mazeBoardData(startRandomRow, 0) Or START
        '_mazeBoardData(endRandomRow, _mazeBoardData.GetUpperBound(1)) = _mazeBoardData(endRandomRow, _mazeBoardData.GetUpperBound(1)) Or [END]

        _mazeBoardData(0, 0) = _mazeBoardData(0, 0) Or START
        _mazeBoardData(_mazeBoardData.GetUpperBound(0), _mazeBoardData.GetUpperBound(1)) = _mazeBoardData(_mazeBoardData.GetUpperBound(0), _mazeBoardData.GetUpperBound(1)) Or [END]
    End Sub

    Private Sub InitializeMaze()
        Dim cell As Integer
        'put up all of the walls and borders as necessary
        For row As Integer = 0 To _mazeBoardData.GetUpperBound(0)
            For column As Integer = 0 To _mazeBoardData.GetUpperBound(1)

                cell = cell Or WALLS
                If row = 0 Then
                    'Set the north border
                    cell = cell Or NORTH_BORDER
                End If
                If column = 0 Then
                    'set the west border
                    cell = cell Or WEST_BORDER
                End If
                If row = _mazeBoardData.GetUpperBound(0) Then
                    'Set the south border
                    cell = cell Or SOUTH_BORDER
                End If
                If column = _mazeBoardData.GetUpperBound(1) Then
                    cell = cell Or EAST_BORDER
                End If

                _mazeBoardData(row, column) = cell
                cell = 0
            Next
        Next
    End Sub

    Private Function GetStartIndex() As CellIndex
        For row As Integer = 0 To _mazeBoardData.GetUpperBound(0)
            If (_mazeBoardData(row, 0) And START) = START Then
                Return New CellIndex(row, 0)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the size of the maze
    ''' </summary>
    ''' <returns>An integer value indiciating the size of the maze.</returns>
    Public Function GetSize() As Integer
        Return _mazeBoardData.GetLength(0)
    End Function

    ''' <summary>
    ''' Gets a sub set of the larger maze
    ''' </summary>
    ''' <param name="centerCellIndex">The cell that exists at the center of the sub maze</param>
    ''' <param name="size">The size of the sub maze to retrieve.  This number is required to be odd.</param>
    ''' <returns>A maze containing the sub maze of the size specified.</returns>
    ''' <remarks></remarks>
    Public Function GetSubMaze(centerCellIndex As CellIndex, size As Integer) As Maze
        If size Mod 2 <> 1 Then
            Throw New ArgumentException("The size must be an odd value (i.e. not evenly divisible by two).")
        End If

        If size < 3 Then
            Throw New ArgumentException(String.Format("The requested submaze size ({0}) is invalid.  The size must be greater than or equal to 3", size))
        End If

        If size > GetSize() Then
            Throw New ArgumentException(String.Format("The requested size ({0}) must be less than or equal to the size of the maze ({1}).", size, GetSize()))
        End If



        Dim startingRow As Integer
        startingRow = Math.Min(Math.Max(centerCellIndex.Row - ((size - 1) / 2), 0), _mazeBoardData.GetUpperBound(0) - (size - 1))

        Dim startingColumn As Integer
        startingColumn = Math.Min(Math.Max(centerCellIndex.Column - ((size - 1) / 2), 0), _mazeBoardData.GetUpperBound(1) - (size - 1))

        Dim newMazeGrid(size - 1, size - 1) As Integer

        Dim newRowIndex As Integer = 0
        Dim newColumnIndex As Integer = 0
        Dim newCurrentIndex As CellIndex = Nothing

        For row As Integer = startingRow To startingRow + (size - 1)
            For col As Integer = startingColumn To startingColumn + (size - 1)
                If row = centerCellIndex.Row AndAlso col = centerCellIndex.Column Then
                    newCurrentIndex = New CellIndex(newRowIndex, newColumnIndex)
                End If
                newMazeGrid(newRowIndex, newColumnIndex) = _mazeBoardData(row, col)
                newColumnIndex += 1
            Next
            newRowIndex += 1
            newColumnIndex = 0
        Next

        Dim returnValue As New Maze(newMazeGrid)
        If newCurrentIndex Is Nothing Then
            Throw New InvalidOperationException("The current index was not set")
        End If
        returnValue._currentCellIndex = newCurrentIndex

        Return returnValue
    End Function

    Private Sub RemoveWallsBetweenIndexes(firstIndex As CellIndex, secondIndex As CellIndex, wallType As WallType)
        If firstIndex.Row <> secondIndex.Row AndAlso firstIndex.Column <> secondIndex.Column Then
            Throw New InvalidOperationException("Wall must be N, E, S or W.")
        End If

        Dim shiftAmount As Byte = 0
        Select Case wallType
            Case Challenges.WallType.Standard
                shiftAmount = 0
            Case Challenges.WallType.Border
                shiftAmount = 4
            Case Challenges.WallType.Solution
                shiftAmount = 8
            Case Challenges.WallType.Backtrack
                shiftAmount = 12
        End Select

        If firstIndex.Row <> secondIndex.Row Then
            'The two indexes differ by row
            If firstIndex.Row > secondIndex.Row Then
                'The first index needs its North wall removed, the second index needs it's South wall removed
                _mazeBoardData(firstIndex.Row, firstIndex.Column) = _mazeBoardData(firstIndex.Row, firstIndex.Column) Xor (1 << (NORTH_SHIFT + shiftAmount))
                _mazeBoardData(secondIndex.Row, secondIndex.Column) = _mazeBoardData(secondIndex.Row, secondIndex.Column) Xor (1 << (SOUTH_SHIFT + shiftAmount))
            Else
                'The first index needs its South wall removed, the second index needs it's North wall removed
                _mazeBoardData(firstIndex.Row, firstIndex.Column) = _mazeBoardData(firstIndex.Row, firstIndex.Column) Xor (1 << (SOUTH_SHIFT + shiftAmount))
                _mazeBoardData(secondIndex.Row, secondIndex.Column) = _mazeBoardData(secondIndex.Row, secondIndex.Column) Xor (1 << (NORTH_SHIFT + shiftAmount))
            End If
        Else
            'The two indexes differ by column
            If firstIndex.Column > secondIndex.Column Then
                'The first index needs its West wall removed, the second index needs it's East wall removed
                _mazeBoardData(firstIndex.Row, firstIndex.Column) = _mazeBoardData(firstIndex.Row, firstIndex.Column) Xor (1 << (WEST_SHIFT + shiftAmount))
                _mazeBoardData(secondIndex.Row, secondIndex.Column) = _mazeBoardData(secondIndex.Row, secondIndex.Column) Xor (1 << (EAST_SHIFT + shiftAmount))
            Else
                'The first index needs its East wall removed, the second index needs it's West wall removed
                _mazeBoardData(firstIndex.Row, firstIndex.Column) = _mazeBoardData(firstIndex.Row, firstIndex.Column) Xor (1 << (EAST_SHIFT + shiftAmount))
                _mazeBoardData(secondIndex.Row, secondIndex.Column) = _mazeBoardData(secondIndex.Row, secondIndex.Column) Xor (1 << (WEST_SHIFT + shiftAmount))
            End If
        End If
    End Sub

    Private Function GetInTactNeighbors(currentIndex As CellIndex) As List(Of CellIndex)
        Dim returnValue As New List(Of CellIndex)

        'Check the North cell
        If currentIndex.Row > 0 AndAlso
            (_mazeBoardData(currentIndex.Row - 1, currentIndex.Column) And WALLS) = WALLS AndAlso
            (_mazeBoardData(currentIndex.Row, currentIndex.Column) And NORTH_BORDER) = 0 Then
            'Add the North cell
            returnValue.Add(New CellIndex(currentIndex.Row - 1, currentIndex.Column))
        End If

        'Check the East cell
        If currentIndex.Column < _mazeBoardData.GetUpperBound(1) AndAlso
            (_mazeBoardData(currentIndex.Row, currentIndex.Column + 1) And WALLS) = WALLS AndAlso
            (_mazeBoardData(currentIndex.Row, currentIndex.Column) And EAST_BORDER) = 0 Then
            'Add the East cell
            returnValue.Add(New CellIndex(currentIndex.Row, currentIndex.Column + 1))
        End If

        'Check the South cell
        If currentIndex.Row < _mazeBoardData.GetUpperBound(0) AndAlso
            (_mazeBoardData(currentIndex.Row + 1, currentIndex.Column) And WALLS) = WALLS AndAlso
            (_mazeBoardData(currentIndex.Row, currentIndex.Column) And SOUTH_BORDER) = 0 Then
            'Add the South cell
            returnValue.Add(New CellIndex(currentIndex.Row + 1, currentIndex.Column))
        End If

        'Check the West cell
        If currentIndex.Column > 0 AndAlso
            (_mazeBoardData(currentIndex.Row, currentIndex.Column - 1) And WALLS) = WALLS AndAlso
            (_mazeBoardData(currentIndex.Row, currentIndex.Column) And WEST_BORDER) = 0 Then
            'Add the West cell
            returnValue.Add(New CellIndex(currentIndex.Row, currentIndex.Column - 1))
        End If

        Return returnValue
    End Function

    Private Const CEll_SIZE As Integer = 10

    ''' <summary>
    ''' Saves a graphical representation of the maze to the specificed file location
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <remarks>The file is saved in PNG format.</remarks>
    Public Sub SaveMazeImage(filename As String)
        Using FileStream As New FileStream(filename, FileMode.Create)
            SaveMazeImage(FileStream)
        End Using
    End Sub

    ''' <summary>
    ''' Saves a graphical represenation of the maze to the given stream.
    ''' </summary>
    ''' <param name="stream">The stream used to store the generated image.</param>
    ''' <remarks></remarks>
    Public Sub SaveMazeImage(stream As Stream)
        Dim bitMap As New Bitmap((_mazeBoardData.GetUpperBound(1) + 1) * CEll_SIZE + 5, (_mazeBoardData.GetUpperBound(0) + 1) * CEll_SIZE + 5)
        Using pen As New Pen(Brushes.Black)
            pen.Width = 1
            Using graphics As Graphics = graphics.FromImage(bitMap)
                graphics.Clear(Color.White)
                Dim cellTopLeftPoint As Point = Nothing
                Dim startingPoint As Point = Nothing
                Dim endingPoint As Point = Nothing

                For row As Integer = 0 To _mazeBoardData.GetUpperBound(0)
                    For col As Integer = 0 To _mazeBoardData.GetUpperBound(1)
                        cellTopLeftPoint = New Point(col * CEll_SIZE, row * CEll_SIZE)
                        startingPoint = Nothing
                        endingPoint = Nothing

                        If (_mazeBoardData(row, col) And NORTH_WALL) = NORTH_WALL Then
                            startingPoint = New Point(cellTopLeftPoint.X, cellTopLeftPoint.Y)
                            endingPoint = New Point(cellTopLeftPoint.X + CEll_SIZE, cellTopLeftPoint.Y)
                            graphics.DrawLine(pen, startingPoint, endingPoint)
                        End If

                        If (_mazeBoardData(row, col) And EAST_WALL) = EAST_WALL Then
                            startingPoint = New Point(cellTopLeftPoint.X + CEll_SIZE, cellTopLeftPoint.Y)
                            endingPoint = New Point(cellTopLeftPoint.X + CEll_SIZE, cellTopLeftPoint.Y + CEll_SIZE)
                            graphics.DrawLine(pen, startingPoint, endingPoint)
                        End If

                        If (_mazeBoardData(row, col) And SOUTH_WALL) = SOUTH_WALL Then
                            startingPoint = New Point(cellTopLeftPoint.X, cellTopLeftPoint.Y + CEll_SIZE)
                            endingPoint = New Point(cellTopLeftPoint.X + CEll_SIZE, cellTopLeftPoint.Y + CEll_SIZE)
                            graphics.DrawLine(pen, startingPoint, endingPoint)
                        End If

                        If (_mazeBoardData(row, col) And WEST_WALL) = WEST_WALL Then
                            startingPoint = New Point(cellTopLeftPoint.X, cellTopLeftPoint.Y)
                            endingPoint = New Point(cellTopLeftPoint.X, cellTopLeftPoint.Y + CEll_SIZE)
                            graphics.DrawLine(pen, startingPoint, endingPoint)
                        End If

                        'Start cell
                        If (_mazeBoardData(row, col) And START) = START Then
                            'Draw the starting indicator
                            startingPoint = New Point(cellTopLeftPoint.X + 2, cellTopLeftPoint.Y + 2)
                            endingPoint = New Point(CEll_SIZE - 4, CEll_SIZE - 4)
                            graphics.FillRectangle(Brushes.Green, New Rectangle(startingPoint, endingPoint))
                        End If

                        'End cell
                        If (_mazeBoardData(row, col) And [END]) = [END] Then
                            'Draw the ending indicator
                            startingPoint = New Point(cellTopLeftPoint.X + 2, cellTopLeftPoint.Y + 2)
                            endingPoint = New Point(CEll_SIZE - 4, CEll_SIZE - 4)
                            graphics.FillRectangle(Brushes.Red, New Rectangle(startingPoint, endingPoint))
                        End If

                        Using solutionPen As New Pen(Brushes.Blue, 1)
                            DrawLine(cellTopLeftPoint, row, col, solutionPen, graphics, startingPoint, endingPoint, NORTH_SOLUTION, EAST_SOLUTION, SOUTH_SOLUTION, WEST_SOLUTION)
                        End Using

                        Using backtrackPen As New Pen(Brushes.Gray, 1)
                            DrawLine(cellTopLeftPoint, row, col, backtrackPen, graphics, startingPoint, endingPoint, NORTH_BACKTRACK, EAST_BACKTRACK, SOUTH_BACKTRACK, WEST_BACKTRACK)
                        End Using

                    Next
                Next

                'pen.Color = Color.Red
                'graphics.DrawRectangle(pen, New Rectangle(0, 0, (_mazeBoardData.GetUpperBound(1) + 1) * CEll_SIZE, (_mazeBoardData.GetUpperBound(0) + 1) * CEll_SIZE))
            End Using
        End Using

        bitMap.Save(stream, ImageFormat.Png)
    End Sub

    Private Sub DrawLine(cellTopLeftPoint As Point, row As Integer, col As Integer, solutionPen As Pen, graphics As Graphics, ByRef startingPoint As Point, ByRef endingPoint As Point, northMask As Integer, eastMask As Integer, southMask As Integer, westMask As Integer)
        startingPoint = New Point(cellTopLeftPoint.X + (CEll_SIZE / 2), cellTopLeftPoint.Y + (CEll_SIZE / 2))
        If (_mazeBoardData(row, col) And northMask) = northMask Then
            endingPoint = New Point(cellTopLeftPoint.X + (CEll_SIZE / 2), cellTopLeftPoint.Y)

            graphics.DrawLine(solutionPen, startingPoint, endingPoint)
        End If

        If (_mazeBoardData(row, col) And eastMask) = eastMask Then
            endingPoint = New Point(cellTopLeftPoint.X + CEll_SIZE, cellTopLeftPoint.Y + (CEll_SIZE / 2))

            graphics.DrawLine(solutionPen, startingPoint, endingPoint)
        End If

        If (_mazeBoardData(row, col) And southMask) = southMask Then
            endingPoint = New Point(cellTopLeftPoint.X + (CEll_SIZE / 2), cellTopLeftPoint.Y + CEll_SIZE)

            graphics.DrawLine(solutionPen, startingPoint, endingPoint)
        End If

        If (_mazeBoardData(row, col) And westMask) = westMask Then
            endingPoint = New Point(cellTopLeftPoint.X, cellTopLeftPoint.Y + (CEll_SIZE / 2))

            graphics.DrawLine(solutionPen, startingPoint, endingPoint)
        End If
    End Sub
End Class


