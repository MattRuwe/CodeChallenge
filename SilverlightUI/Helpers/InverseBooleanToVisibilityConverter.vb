Imports System.Globalization
Imports System.Windows.Data

Public Class InverseBooleanToVisibilityConverter
    Implements IValueConverter
    ' Methods
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim flag As Boolean = False
        If TypeOf value Is Boolean Then
            flag = CBool(value)
        ElseIf TypeOf value Is Nullable(Of Boolean) Then
            Dim nullable As Nullable(Of Boolean) = DirectCast(value, Nullable(Of Boolean))
            flag = IIf(nullable.HasValue, nullable.Value, False)
        End If
        Return IIf(flag, Visibility.Collapsed, Visibility.Visible)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return (TypeOf value Is Visibility AndAlso (DirectCast(value, Visibility) <> Visibility.Visible))
    End Function


End Class
