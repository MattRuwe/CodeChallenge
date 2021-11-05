Public Class DayStock
    Public Property DayNumber As Integer
    Public Property Open As Decimal
    Public Property High As Decimal
    Public Property Low As Decimal
    Public Property Close As Decimal
    Public Property Volume As Long

    Public Function Clone() As DayStock
        Dim returnValue As New DayStock With
            {
                .DayNumber = DayNumber,
                .Open = Open,
                .High = High,
                .Low = Low,
                .Close = Close,
                .Volume = Volume
            }

        Return returnValue
    End Function

End Class
