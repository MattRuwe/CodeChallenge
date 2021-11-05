Imports OmahaMTG.Challenge.Manager
Imports System.Configuration

Public Class ExecutionService

    Private _host As ChallengeHost
    Private _serviceTrace As TraceSource

    Protected Overrides Sub OnStart(ByVal args() As String)
        Try
            _serviceTrace = New TraceSource("ChallengeHost")
            _serviceTrace.TraceInformation("Attempting to start the Windows service")
            _serviceTrace.Flush()

            _serviceTrace.TraceInformation("Attempting to retrieve the connection string")
            Dim connString As String = ConfigurationManager.ConnectionStrings("CodeChallengeModelChallengeManager").ConnectionString
            If connString IsNot Nothing Then
                _serviceTrace.TraceInformation("Connection string retrieved as {0}", connString)
            Else
                Throw New InvalidOperationException("The connection string could not be found")
            End If

            _serviceTrace.TraceInformation("Attempting to start challenge host")
            _host = New ChallengeHost(
                connString,
                My.Settings.ChallengeExecutionWorkingDirectory,
                My.Settings.NewChallangeCheckIntervalSeconds,
                My.Settings.ChallengeConsolePath,
                My.Settings.ArchivePath)
            _host.Start()
            _serviceTrace.TraceInformation("Service started")
        Catch ex As Exception
            EventLog.WriteEntry(ex.ToString)
            _serviceTrace.TraceInformation(ex.ToString)
        Finally
            _serviceTrace.Flush()
        End Try

    End Sub

    Protected Overrides Sub OnStop()
        _serviceTrace.TraceInformation("Attempting to stop host")
        If _host IsNot Nothing Then
            _host.Stop()
        Else
            _serviceTrace.TraceInformation("The host was null and cannot be stopped.")
        End If
        _serviceTrace.TraceInformation("Host was successfully stopped")
        _serviceTrace.TraceInformation("Windows service is exiting")
    End Sub

End Class
