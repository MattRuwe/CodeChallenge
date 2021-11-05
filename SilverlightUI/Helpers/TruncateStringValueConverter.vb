Imports System.Windows.Data

Public Class TruncateStringValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim desiredLength As Integer

        If value IsNot Nothing AndAlso value.GetType Is GetType(String) AndAlso Integer.TryParse(parameter, desiredLength) Then
            Dim stringValue As String = value

            If stringValue.Length > desiredLength Then
                Return stringValue.Substring(0, desiredLength) & "..."
            End If
        End If

        Return value
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
