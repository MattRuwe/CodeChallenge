Public Class SelectedTabToActiveTabConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim returnValue As Integer

        If value.GetType Is GetType(ActiveTab) Then
            Select Case CType(value, ActiveTab)
                Case ActiveTab.Challenges
                    returnValue = 0
                Case ActiveTab.Leadboard
                    returnValue = 1
                Case ActiveTab.Announcements
                    returnValue = 2
                Case ActiveTab.MyEntries
                    returnValue = 3
                Case ActiveTab.LatestEntries
                    returnValue = 4
            End Select
        End If

        Return returnValue
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim returnValue As ActiveTab

        If value.GetType Is GetType(Integer) Then
            Select Case CType(value, Integer)
                Case 0
                    returnValue = ActiveTab.Challenges
                Case 1
                    returnValue = ActiveTab.Leadboard
                Case 2
                    returnValue = ActiveTab.Announcements
                Case 3
                    returnValue = ActiveTab.MyEntries
                Case 4
                    returnValue = ActiveTab.LatestEntries
            End Select
        End If

        Return returnValue
    End Function
End Class
