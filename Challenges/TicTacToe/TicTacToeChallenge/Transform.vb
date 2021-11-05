'Public Class Transform
'    Const Size As Integer = 3
'    Private Delegate Function TransformFunc(p As Point) As Point
'    Public Shared Function Rotate90Degree(p As Point) As Point
'        '012 -> x->y, y->size-x
'        '012
'        Return New Point(Size - p.y - 1, p.x)
'    End Function

'    Public Shared Function MirrorX(p As Point) As Point
'        '012 -> 210
'        Return New Point(Size - p.x - 1, p.y)
'    End Function

'    Public Shared Function MirrorY(p As Point) As Point
'        Return New Point(p.x, Size - p.y - 1)
'    End Function

'    Private actions As New List(Of TransformFunc)()
'    Public Function ActOn(p As Point) As Point
'        For Each f As TransformFunc In actions
'            If f IsNot Nothing Then
'                p = f(p)
'            End If
'        Next

'        Return p
'    End Function

'    Private Sub New(op As TransformFunc, ops As TransformFunc())
'        If op IsNot Nothing Then
'            actions.Add(op)
'        End If
'        If ops IsNot Nothing AndAlso ops.Length > 0 Then
'            actions.AddRange(ops)
'        End If
'    End Sub

'    Public Shared s_transforms As New List(Of Transform)()
'    Shared Sub New()
'        For i As Integer = 0 To 3
'            Dim ops As TransformFunc() = Enumerable.Repeat(Of TransformFunc)(AddressOf Rotate90Degree, i).ToArray()
'            s_transforms.Add(New Transform(Nothing, ops))
'            s_transforms.Add(New Transform(AddressOf MirrorX, ops))
'            s_transforms.Add(New Transform(AddressOf MirrorY, ops))
'        Next
'    End Sub
'End Class
