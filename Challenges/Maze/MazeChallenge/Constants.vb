Friend Module Constants

    Public Const NORTH_SHIFT As Integer = 0
    Public Const EAST_SHIFT As Integer = 1
    Public Const SOUTH_SHIFT As Integer = 2
    Public Const WEST_SHIFT As Integer = 3

    Public Const WALLS As Integer = 15
    Public Const NORTH_WALL As Integer = 1
    Public Const EAST_WALL As Integer = 2
    Public Const SOUTH_WALL As Integer = 4
    Public Const WEST_WALL As Integer = 8

    Public Const BORDERS As Integer = 15 >> 4
    Public Const NORTH_BORDER As Integer = 16
    Public Const EAST_BORDER As Integer = 32
    Public Const SOUTH_BORDER As Integer = 64
    Public Const WEST_BORDER As Integer = 128

    Public Const SOLUTIONS As Integer = 15 >> 8
    Public Const NORTH_SOLUTION As Integer = 256
    Public Const EAST_SOLUTION As Integer = 512
    Public Const SOUTH_SOLUTION As Integer = 1024
    Public Const WEST_SOLUTION As Integer = 2048

    Public Const BACKTRACKS As Integer = 15 >> 12
    Public Const NORTH_BACKTRACK As Integer = 4096
    Public Const EAST_BACKTRACK As Integer = 8192
    Public Const SOUTH_BACKTRACK As Integer = 16384
    Public Const WEST_BACKTRACK As Integer = 32768

    Public Const START As Integer = 65536
    Public Const [END] As Integer = 131072
End Module
