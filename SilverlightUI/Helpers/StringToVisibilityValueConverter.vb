Imports System.Windows.Data

Public Class StringToVisibilityValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim returnValue As Visibility
        Dim negateValue As Boolean = False

        Boolean.TryParse(parameter, negateValue)
        If Not negateValue Then
            returnValue = If(String.IsNullOrWhiteSpace(value), Visibility.Collapsed, Visibility.Visible)
        Else
            returnValue = If(Not String.IsNullOrWhiteSpace(value), Visibility.Collapsed, Visibility.Visible)
        End If

        Return returnValue
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
