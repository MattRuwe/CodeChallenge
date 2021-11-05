Imports System.Reflection
Imports System.Windows.Data
Imports System.Windows.Browser
Imports System.Text.RegularExpressions

Public Class ChallengeIdToSampleDownloadUriValueConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value IsNot Nothing AndAlso value.GetType Is GetType(Integer) Then
            Dim encodedName As String = HttpUtility.UrlEncode(value & ".sample")
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

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class
