Imports System.Windows.Data

Public Class UtcDateTimeToLocalValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing AndAlso value.GetType Is GetType(DateTime) Then
            Dim newValue As DateTime = CType(value, DateTime)
            newValue = DateTime.SpecifyKind(newValue, DateTimeKind.Utc)

            Return newValue.ToLocalTime()
        Else
            Return value
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
