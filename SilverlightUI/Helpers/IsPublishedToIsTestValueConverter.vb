Imports System.Windows.Data

Public Class IsPublishedToIsTestValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value.GetType Is GetType(Boolean) AndAlso parameter.GetType Is GetType(Boolean) AndAlso CType(parameter, Boolean) = True Then
            Return False
        Else
            Return value
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack

    End Function
End Class
