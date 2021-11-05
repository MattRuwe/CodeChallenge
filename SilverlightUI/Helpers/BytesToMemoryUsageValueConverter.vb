Imports System.Windows.Data

Public Class BytesToMemoryUsageValueConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value.GetType Is GetType(Decimal) OrElse value.GetType Is GetType(Int64) OrElse value.GetType Is GetType(Int32) Then
            Dim newValue As Long = value
            If newValue < 1024 Then
                Return String.Format("{0} bytes", value.ToString)
            ElseIf value < Math.Pow(1024, 2) Then
                Return String.Format("{0} KB", value / 1024)
            ElseIf value < Math.Pow(1024, 3) Then
                Return String.Format("{0} MB", value / Math.Pow(1024, 2))
            ElseIf value < Math.Pow(1024, 4) Then
                Return String.Format("{0} GB", value / Math.Pow(1024, 3))
            Else
                Return String.Format("{0} bytes", newValue)
            End If
        Else
            Return value
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class
