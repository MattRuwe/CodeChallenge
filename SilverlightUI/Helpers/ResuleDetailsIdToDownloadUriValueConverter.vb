Imports System.Text.RegularExpressions
Imports System.Windows.Data
Imports System.Windows.Browser

Public Class ResuleDetailsIdToDownloadUriValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing AndAlso value.GetType Is GetType(Integer) Then
            Dim encodedName As String = HttpUtility.UrlEncode(value & ".results")
            Dim match As Match = Regex.Match(Application.Current.Host.Source.AbsoluteUri, "(?i)^.*?/(?=ClientBin)")

            Dim url As String
            If match.Success Then
                url = match.Value & encodedName
            Else
                Throw New InvalidOperationException(String.Format("The URI {0} could not be converted to an absolute path", Application.Current.Host.Source.AbsoluteUri))
            End If

            Return New Uri(url, UriKind.Absolute)
        End If
        Return value
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
