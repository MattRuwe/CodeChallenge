Public Class AssemblyValidationResult

    Private _isValid As Boolean
    Public Property IsValid() As Boolean
        Get
            Return _isValid
        End Get
        Set(ByVal value As Boolean)
            _isValid = value
        End Set
    End Property

    Private _validationMessage As String
    Public Property ValidationMessage() As String
        Get
            Return _validationMessage
        End Get
        Set(ByVal value As String)
            _validationMessage = value
        End Set
    End Property

    Private _assemblyFullName As String
    Public Property AssemblyFullName() As String
        Get
            Return _assemblyFullName
        End Get
        Set(ByVal value As String)
            _assemblyFullName = value
        End Set
    End Property


End Class
