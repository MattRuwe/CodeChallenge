Public Class ChallengeEventLog

    Private Const MAX_ENTRY_LENGTH As Integer = 30000

    Public Shared Sub WriteError(ByVal message As String)
        CheckEventSource()
        EventLog.WriteEntry("OmahaMTG", If(message.Length >= MAX_ENTRY_LENGTH, message.Substring(0, MAX_ENTRY_LENGTH), message), EventLogEntryType.Error)
    End Sub

    Public Shared Sub WriteInformation(ByVal message As String)
        Try
            CheckEventSource()
            EventLog.WriteEntry("OmahaMTG", If(message.Length >= MAX_ENTRY_LENGTH, message.Substring(0, MAX_ENTRY_LENGTH), message), EventLogEntryType.Information)
        Catch
        End Try
    End Sub

    Private Shared Sub CheckEventSource()
        If Not EventLog.SourceExists("OmahaMTG") Then
            EventLog.CreateEventSource(New EventSourceCreationData("OmahaMTG", "OmahaMTG"))
        End If
    End Sub
End Class
