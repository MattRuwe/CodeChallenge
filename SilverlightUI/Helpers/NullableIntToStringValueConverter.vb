Imports System.Globalization
Imports System.Windows.Data
Public Class NullableIntToStringValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing AndAlso value.GetType Is GetType(Integer?) Then
            Dim convValue As Integer? = CType(value, Integer?)

            If convValue.HasValue Then
                Return convValue.Value.ToString()
            Else
                Return "0"
            End If
        ElseIf value IsNot Nothing AndAlso value.GetType() Is GetType(Integer) Then
            Return value.ToString()
        Else
            Return "0"
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function

End Class
