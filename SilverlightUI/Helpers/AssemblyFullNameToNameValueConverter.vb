Imports System.Windows.Data
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class AssemblyFullNameToNameValueConverter
    Implements IValueConverter


    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing AndAlso value.GetType Is GetType(String) Then
            Try
                Dim name As New AssemblyName(value)
                Return name.Name
            Catch ex As Exception
                Dim match As Match = Regex.Match(value, "^[^,]+(?=,)")
                If match.Success Then
                    Return match.Value
                Else
                    Return value
                End If
            End Try
        End If
        Return value
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class
