<Serializable()>
Public Class ChallengeResult

    Private _authorNotes As String
    Public Property AuthorNotes() As String
        Get
            Return _authorNotes
        End Get
        Set(ByVal value As String)
            _authorNotes = value
        End Set
    End Property

    Private _durationTicks As Long
    Public Property DurationTicks() As Long
        Get
            Return _durationTicks
        End Get
        Set(ByVal value As Long)
            _durationTicks = value
        End Set
    End Property

    Private _error As String
    Public Property DisplayError() As String
        Get
            Return _error
        End Get
        Set(ByVal value As String)
            _error = value
        End Set
    End Property

    Private _detailedError As String
    Public Property DetailedError() As String
        Get
            Return _detailedError
        End Get
        Set(ByVal value As String)
            _detailedError = value
        End Set
    End Property

    Private _successful As Boolean
    Public Property Successful() As Boolean
        Get
            Return _successful
        End Get
        Set(ByVal value As Boolean)
            _successful = value
        End Set
    End Property

    Private _resultMessage As String
    Public Property ResultMessage() As String
        Get
            Return _resultMessage
        End Get
        Set(ByVal value As String)
            _resultMessage = value
        End Set
    End Property

    Private _score As Long
    Public Property Score() As Long
        Get
            Return _score
        End Get
        Set(ByVal value As Long)
            _score = value
        End Set
    End Property

    Private _cpuCyclesUsed As ULong
    Public Property CpuCyclesUsed() As ULong
        Get
            Return _cpuCyclesUsed
        End Get
        Set(ByVal value As ULong)
            _cpuCyclesUsed = value
        End Set
    End Property

    Private _testResults As List(Of FileResult)
    Public Property TestResults() As List(Of FileResult)
        Get
            If _testResults Is Nothing Then
                _testResults = New List(Of FileResult)
            End If
            Return _testResults
        End Get
        Set(value As List(Of FileResult))
            _testResults = value
        End Set
    End Property

End Class
