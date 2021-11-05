Public Class Portfolio
    Public Property Cash As Decimal
    Public Property SharesOwns As Long

    Public Function Clone() As Portfolio
        Dim returnValue As New Portfolio With
            {
                .Cash = Cash,
                .SharesOwns = SharesOwns
            }

        Return returnValue
    End Function
End Class
