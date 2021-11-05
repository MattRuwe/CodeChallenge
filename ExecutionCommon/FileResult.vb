<Serializable()>
Public Class FileResult

    Public Property Filename As String

    Private _contents As Byte()
    Public Property Contents As Byte()
        Get
            Return _contents
        End Get
        Set(value As Byte())
            _contents = value
        End Set
    End Property

End Class
