''' <summary>
''' An object used to store a row and a column value
''' </summary>
''' <remarks></remarks>
Public Class CellIndex
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Creates a new CellIndex object with the specified row and column values
    ''' </summary>
    ''' <param name="row">The value to be assigned to the Row property</param>
    ''' <param name="column">The value to be assigned to the Column property</param>
    ''' <remarks></remarks>
    Public Sub New(row As Integer, column As Integer)
        Me.Row = row
        Me.Column = column
    End Sub

    ''' <summary>
    ''' Gets or sets the index of the row (0 based)
    ''' </summary>
    ''' <value>An integer value indiciating the position of the row</value>
    ''' <returns>An integer value indiciating the position of the row</returns>
    ''' <remarks></remarks>
    Public Property Row As Integer

    ''' <summary>
    ''' Gets or sets the index of the column (0 based)
    ''' </summary>
    ''' <value>An integer value indiciating the position of the column</value>
    ''' <returns>An integer value indiciating the position of the column</returns>
    ''' <remarks></remarks>
    Public Property Column As Integer

    ''' <summary>
    ''' Converts this object instance to a string (e.g. &quot;(1, 5)&quot; where the row is 1 and the column is 5)
    ''' </summary>
    ''' <returns>A string representation of this CellIndex instance</returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return String.Format("({0},{1})", Row, Column)
    End Function
End Class
