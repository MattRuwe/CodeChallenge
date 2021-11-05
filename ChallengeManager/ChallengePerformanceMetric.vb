Public Class ChallengePerformanceMetric
    Public Sub New()
        MeasurementTime = DateTime.Now
    End Sub

    Public Property MeasurementTime As DateTime
    Public Property MemoryUsed As Long
End Class
