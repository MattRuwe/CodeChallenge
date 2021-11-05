Imports System.Globalization
Imports System.Windows.Data

Public Class NullToVisbilityValueConverter
Implements IValueConverter
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim returnValue As Visibility = Visibility.Visible

        returnValue = If(value Is Nothing, Visibility.Collapsed, Visibility.Visible)

        Return returnValue
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        ' TODO: Implement this method
        Throw New NotImplementedException()
    End Function

End Class
