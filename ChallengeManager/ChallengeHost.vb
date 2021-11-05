Imports System.Data.SqlClient
Imports System.Threading
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports OmahaMTG.Challenge.ExecutionCommon
Imports System.Reflection
Imports System.Runtime.InteropServices

''' <summary>
''' 
''' </summary>
''' <remarks>
''' This class is responsible for
''' 1) Watching for new challenges that have arrived via the database
''' 2) Starting a new process which will run the tests (i.e. the client)
''' 3) Making sure that the client runs within its limits (i.e. time and memory restrictions)
''' </remarks>
Public Class ChallengeHost
    Private _newChallengeTimer As Timer
    Private ReadOnly _connectionString As String
    Private ReadOnly _challengeWorkingDirectory As String
    Private ReadOnly _newChallangeCheckIntervalSeconds As Integer
    Private ReadOnly _challengeConsolePath As String
    Private ReadOnly _archivePath As String
    Private ReadOnly _syncLock As Object = New Object
    Private ReadOnly _trace As TraceSource = New TraceSource("ChallengeHost")

    Public Sub New(ByVal connectionString As String,
                   ByVal challengeWorkingDirectory As String,
                   ByVal newChallangeCheckIntervalSeconds As Integer,
                   ByVal challengeConsolePath As String,
                   ByVal archivePath As String)

        If connectionString Is Nothing Then
            Throw New ArgumentNullException("connectionString")
        End If

        If String.IsNullOrEmpty(challengeWorkingDirectory) Then
            Throw New ArgumentException("The challengeWorkingDirectory must be set to a valid directory")
        End If

        If Not Directory.Exists(challengeWorkingDirectory) Then
            _trace.TraceInformation("The challenge working directory ('{0}') does not exists", challengeWorkingDirectory)
            _trace.TraceInformation("Attempting to create directory.")
            Directory.CreateDirectory(challengeWorkingDirectory)
            If Not Directory.Exists(challengeWorkingDirectory) Then
                _trace.TraceInformation("The directory still does not exists, there's likely another issue involved.")
            Else
                _trace.TraceInformation("The directory was successfull created")
            End If
        End If

        If newChallangeCheckIntervalSeconds <= 0 Then
            Throw New ArgumentException("The argument 'newChallengeCheckIntervalSeconds' must be set to a value grater than 0.")
        End If

        If String.IsNullOrWhiteSpace(challengeConsolePath) OrElse Not File.Exists(challengeConsolePath) OrElse Not challengeConsolePath.EndsWith(".exe") Then
            Throw New ArgumentException("The argument 'challengeConsolePath' is either null or does not contain a valid path to the challenge console executable.")
        End If

        _trace.TraceInformation("Initializing ChallengeHost")
        _trace.TraceInformation("ConnectionString                 = {0}", connectionString)
        _trace.TraceInformation("challengeWorkingDirectory        = {0}", challengeWorkingDirectory)
        _trace.TraceInformation("newChallangeCheckIntervalSeconds = {0}", newChallangeCheckIntervalSeconds)
        _trace.TraceInformation("challengeConsolePath             = {0}", challengeConsolePath)

        _connectionString = connectionString
        _challengeWorkingDirectory = challengeWorkingDirectory
        _newChallangeCheckIntervalSeconds = newChallangeCheckIntervalSeconds
        _challengeConsolePath = challengeConsolePath
        _archivePath = archivePath
    End Sub

    Public Sub Start()
        _trace.TraceInformation("Starting the challenge host")
        InternalStart(True)
    End Sub

    Private Sub InternalStart(ByVal startImmediately As Boolean)
        SyncLock _syncLock
            _trace.TraceInformation("Creating a new timer object and initializing it with an interval of {0} milliseconds.", _newChallangeCheckIntervalSeconds * 1000)
            _newChallengeTimer = New Timer(New TimerCallback(AddressOf ChallengeTimer_Elapsed))
            _newChallengeTimer.Change(If(startImmediately, 0, _newChallangeCheckIntervalSeconds * 1000), _newChallangeCheckIntervalSeconds * 1000)
            _trace.TraceInformation("The timer has been successfully created and started")
        End SyncLock
    End Sub

    Public Sub [Stop]()
        SyncLock _syncLock
            _trace.TraceInformation("Stopping the new challenge timer.")
            _newChallengeTimer.Change(Timeout.Infinite, Timeout.Infinite)
            _newChallengeTimer = Nothing
            _trace.TraceInformation("The new challenge timer has been stopped.")
        End SyncLock

    End Sub

    Private Sub CleanupUnpublishedEntries()
        Dim db As New CodeChallengeModel(_connectionString)
        Try
            _trace.TraceInformation("Cleaning up unpublished entries")
            Dim rowsAffected As Integer
            rowsAffected = db.ExecuteStoreCommand("delete from CodeChallenge_Entry_Result where codechallenge_entry_id IN (select id from CodeChallenge_Entry where IsPublished = 0 and Datediff(""d"", DateAdded, GETDATE()) > 30)")
            _trace.TraceInformation("{0} entry result(s) removed", rowsAffected)
            rowsAffected = db.ExecuteStoreCommand("delete from CodeChallenge_Entry where IsPublished = 0 and Datediff(""d"", DateAdded, GETDATE()) > 30")
            _trace.TraceInformation("{0} entry(ies) removed", rowsAffected)
        Catch ex As Exception
            _trace.TraceInformation("Couldn't cleanup assemblies due to error:")
            _trace.TraceInformation(ex.ToString())
        End Try
    End Sub

    Private Sub UpdateServiceHeartbeat()
        Dim db As New CodeChallengeModel(_connectionString)
        Dim config = db.CodeChallenge_Config.FirstOrDefault()

        If config Is Nothing Then
            config = New CodeChallenge_Config With
                     {.LastServiceHeartbeat = DateTime.UtcNow}
            db.CodeChallenge_Config.AddObject(config)
        Else
            config.LastServiceHeartbeat = DateTime.UtcNow()
        End If

        db.SaveChanges()
    End Sub

    Private Sub ChallengeTimer_Elapsed(state As Object)
        _trace.TraceInformation("New challenge timer elapsed")

        Dim challengeClientOutput As String = Nothing
        Dim challengeResults As String = Nothing
        Dim performance As List(Of ChallengePerformanceMetric)
        Dim assemblies As List(Of String) = Nothing
        Dim currentCount As Integer = 0
        SyncLock _syncLock
            Try
                [Stop]()

                CleanupUnpublishedEntries()

                UpdateServiceHeartbeat()

                _trace.TraceInformation("Creating connection to database")
                Dim db As New CodeChallengeModel(_connectionString)
                db.CommandTimeout = 216000
                _trace.TraceInformation("Checking for entries that need to be run")

                _trace.TraceInformation("Getting entries to run")
                Dim entryIds = (From cce In db.CodeChallenge_Entry.Include("CodeChallenge")
                               Where cce.DateRan Is Nothing AndAlso cce.CodeChallenge.StartDate < DateTime.Now AndAlso cce.CodeChallenge.EndDate > DateTime.Now
                               Order By cce.DateAdded
                               Select cce.id).ToList()

                _trace.TraceInformation("Found {0} entry(ies) to run", entryIds.Count)

                If entryIds.Count > 0 Then

                    For Each entryid As Integer In entryIds
                        db.ExecuteStoreCommand("UPDATE codechallenge_entry SET status = 'Queued' WHERE id = @id", New SqlParameter With {.ParameterName = "@ID", .Value = entryid})
                    Next

                    Dim newChallengeConsolePath As String = SetupExecutionFolder()

                    For Each entryId As Integer In entryIds
                        currentCount += 1
                        _trace.TraceInformation("Entry {0} of {1}", currentCount, entryIds.Count)
                        Dim localEntryId As Integer = entryId

                        db.ExecuteStoreCommand("UPDATE codechallenge_entry SET status = 'Initializing' WHERE id = @id", New SqlParameter With {.ParameterName = "@ID", .Value = entryId})

                        _trace.TraceInformation("Retrieving entry from database.")
                        Dim entry = (From d In db.CodeChallenge_Entry.
                                     Include("CodeChallenge.CodeChallenge_DeveloperAssembly").
                                     Include("CodeChallenge_Entry_Result").
                                     Include("aspnet_Users")
                                     Where (d.id = localEntryId)
                                     Select d).FirstOrDefault()

                        entry.Status = "Running"
                        db.SaveChanges()

                        performance = New List(Of ChallengePerformanceMetric)

                        Dim assemblyFullname As String = entry.AssemblyFullName
                        _trace.TraceInformation("AssemblyFullName = '{0}'", assemblyFullname)
                        'Run the challenge
                        Try
                            assemblies = InitializeChallengeAssemblies(entry, _challengeWorkingDirectory)

                            If assemblies.Count = 0 Then
                                _trace.TraceInformation("WARNING: The entry assembly was not written to the disk.  Unable to continue processing entry.")
                                Continue For
                            End If
                            Dim overranMemory As Boolean = False
                            Dim overranTime As Boolean = False

                            challengeClientOutput = RunClientProcess(assemblies, entry, newChallengeConsolePath, overranMemory, overranTime)

                            _trace.TraceInformation("Client output: {0}", challengeClientOutput)

                            Dim resultsFilePath As String = Path.Combine(Path.GetDirectoryName(newChallengeConsolePath), "Results.xml")
                            If File.Exists(resultsFilePath) Then
                                _trace.TraceInformation("Reading the results xml file")
                                challengeResults = File.ReadAllText(resultsFilePath)
                            Else
                                _trace.TraceInformation("The results xml file does not exist (i.e. no results).")
                                challengeResults = String.Empty
                            End If

                            _trace.TraceInformation("Writing results to the database.")
                            If overranMemory Then
                                _trace.TraceInformation("Adding the fact that the memory limitation was exceeded")
                                Dim result As New CodeChallenge_Entry_Result With {
                                    .codechallenge_entry_id = entry.id,
                                    .duration = entry.TotalExecutionTime,
                                    .error = String.Format("The entry used more memory than allowed ({0:N0} bytes maximum)", entry.CodeChallenge.MaximumMemoryUsageBytes),
                                    .result_message = "The entry did not succeed due to an error",
                                    .successful = False,
                                    .score = 0}
                                entry.CodeChallenge_Entry_Result.Add(result)
                                entry.FinalScore = 0
                                ChallengeEventLog.WriteInformation(String.Format("The entry with id {0} was stopped because it used more memory than allowed ({1:N0} bytes)", entry.id, entry.CodeChallenge.MaximumMemoryUsageBytes))
                                _trace.TraceInformation("Memory exceeded info was added.")
                            ElseIf overranTime Then
                                _trace.TraceInformation("Adding the fact that the time limitation was exceeded")
                                Dim result As New CodeChallenge_Entry_Result With {
                                    .codechallenge_entry_id = entry.id,
                                    .duration = entry.TotalExecutionTime,
                                    .error = String.Format("The entry executed longer than allowed ({0} second(s) maximum)", entry.CodeChallenge.MaximumRunningSeconds),
                                    .result_message = "The entry did not succeed due to an error",
                                    .successful = False,
                                    .score = 0}
                                entry.CodeChallenge_Entry_Result.Add(result)
                                entry.FinalScore = 0
                                ChallengeEventLog.WriteInformation(String.Format("The entry with id {0} was stopped because it ran longer than it was allowed ({1} seconds)", entry.id, entry.CodeChallenge.MaximumRunningSeconds))
                                _trace.TraceInformation("Time exceeded info was added.")
                            Else
                                _trace.TraceInformation("The challenge completed without any limits being exceeded, write the results.")
                                ChallengeEventLog.WriteInformation(String.Format("The entry with id {0} completed with the following results:{1}{2}", entry.id, Environment.NewLine, challengeClientOutput))
                                Dim results As List(Of ChallengeResult) = ProcessResult(challengeResults)

                                If results IsNot Nothing Then
                                    Dim scoresTotal As Long = 0

                                    For Each result As ChallengeResult In results
                                        Dim dbResult As New CodeChallenge_Entry_Result

                                        dbResult.CodeChallenge_Entry = entry
                                        dbResult.duration = TimeSpan.FromTicks(result.DurationTicks).TotalMilliseconds
                                        dbResult.error = result.DisplayError
                                        dbResult.result_message = result.ResultMessage
                                        dbResult.successful = result.Successful
                                        dbResult.score = result.Score
                                        dbResult.author_note = result.AuthorNotes
                                        dbResult.cpu_cycles = result.CpuCyclesUsed
                                        If result.TestResults IsNot Nothing AndAlso result.TestResults.Count > 0 Then
                                            dbResult.test_result_data = ChallengeManager.CompressFileResultToZipByteArray(result.TestResults)
                                        Else
                                            dbResult.test_result_data = Nothing
                                        End If
                                        scoresTotal += result.Score

                                        entry.CodeChallenge_Entry_Result.Add(dbResult)

                                        _trace.TraceInformation("Added the following result:")
                                        _trace.TraceInformation("duration =       {0:N4} milliseconds", TimeSpan.FromTicks(result.DurationTicks).TotalMilliseconds)
                                        _trace.TraceInformation("CPU cycles =     {0:N0} cycles", result.CpuCyclesUsed)
                                        _trace.TraceInformation("error =          {0}", If(result.DisplayError Is Nothing, String.Empty, result.DisplayError))
                                        _trace.TraceInformation("result message = {0}", If(result.ResultMessage Is Nothing, String.Empty, result.ResultMessage))
                                        _trace.TraceInformation("successful =     {0}", result.Successful.ToString)
                                        _trace.TraceInformation("score =          {0:N0}", result.Score)
                                        _trace.TraceInformation("author notes =   {0}", If(result.AuthorNotes Is Nothing, String.Empty, result.AuthorNotes))
                                    Next

                                    If results.Count > 0 Then
                                        _trace.TraceInformation("Attempting to get final score...")
                                        entry.FinalScore = Math.Floor(scoresTotal / CType(results.Count, Long))
                                        _trace.TraceInformation("The final score was calculated as {0}", entry.FinalScore)
                                    End If
                                    Trace.TraceInformation("Final score = {0}", entry.FinalScore)
                                Else
                                    ChallengeEventLog.WriteError(String.Format("An unknown result was received from the challenge client: {0}", challengeClientOutput))
                                    _trace.TraceInformation("An unknown result was received from the challenge client: {0}", challengeClientOutput)
                                    _trace.TraceInformation("Writing this result to the database")
                                    Dim result As New CodeChallenge_Entry_Result With {
                                    .codechallenge_entry_id = entry.id,
                                    .duration = entry.TotalExecutionTime,
                                    .error = "An internal hosting error occurred while the entry was executing, please contact the administrator",
                                    .result_message = "The entry did not succeed due to an error",
                                    .successful = False,
                                    .score = 0}
                                    entry.CodeChallenge_Entry_Result.Add(result)
                                    entry.FinalScore = 0
                                    _trace.TraceInformation("The unknown result was noted in the database")
                                End If
                                End If

                        Catch ex As Exception
                            ChallengeEventLog.WriteError(String.Format("An error occurred while executing a challenge: {0}", ex.ToString))
                            _trace.TraceInformation("An error occurred while executing a challenge: {0}", ex.ToString)
                        Finally
                            Try
                                'Make sure that that DateRan field gets set, otherwise the entry might continuous execute if an unexpected exception occurred
                                entry.DateRan = DateTime.UtcNow
                                _trace.TraceInformation("Successfully set the entry's DateRan field.")
                            Catch ex As Exception
                                _trace.TraceInformation("Couldn't set the DateRan field on the entry because of an error")
                                _trace.TraceInformation(ex.ToString)
                            End Try

                            Try
                                entry.Status = "Finished"
                                db.SaveChanges()
                                _trace.TraceInformation("Successfully set the entry's status field to finished.")
                            Catch ex As Exception
                                _trace.TraceInformation("Couldn't set the status field on the entry because of an error")
                                _trace.TraceInformation(ex.ToString)
                            End Try

                            _trace.TraceInformation("Cleaning up assemblies written to the disk")
                            ArchiveAssemblies(assemblies, _archivePath, entry.id, entry.aspnet_Users.UserName)
                            Try
                                _trace.TraceInformation("Attempting to save changes to the database")
                                db.SaveChanges()
                            Catch ex As Exception
                                _trace.TraceInformation("An error happened while trying to save changes to the database.")
                                _trace.TraceInformation(ex.ToString())
                            End Try
                        End Try
                    Next

                    Try
                        _trace.TraceInformation("Attempting to write results to the database...")
                        db.SaveChanges()
                        _trace.TraceInformation("The results have been successfully added")
                    Catch ex As Exception
                        ChallengeEventLog.WriteError(String.Format("An error occurred while trying to save the results to the database: {0}", ex.ToString))
                        _trace.TraceInformation("An error occurred while trying to save the results to the database")
                        _trace.TraceInformation(ex.ToString)
                    End Try

                Else
                    _trace.TraceInformation("There were no entries available to process.  Restarting timer.")
                End If

            Catch ex As Exception
                _trace.TraceInformation("An error occurred while processing entries: {0}", ex.ToString)
            Finally
                _trace.TraceInformation("Finished - Clearing all files from untrusted folder")
                ClearUntrustedFolder()
                _trace.Flush()
                InternalStart(False)
            End Try
        End SyncLock
    End Sub

    Private Function SetupExecutionFolder() As String
        ClearUntrustedFolder()

        'Copy the challenge console application to the untrusted folder so it can be executed
        Dim newChallengeConsolePath As String = Path.Combine(_challengeWorkingDirectory, Path.GetFileName(_challengeConsolePath))
        'Console Application
        File.Copy(_challengeConsolePath, newChallengeConsolePath)
        'Config File
        File.Copy(_challengeConsolePath & ".config", newChallengeConsolePath & ".config")
        'Challenge Manager (contains the client in addition to the host)
        File.Copy(Path.Combine(Path.GetDirectoryName(_challengeConsolePath), Path.GetFileName(Assembly.GetExecutingAssembly.Location)), Path.Combine(_challengeWorkingDirectory, Path.GetFileName(Assembly.GetExecutingAssembly.Location)))
        Return newChallengeConsolePath
    End Function

    Private Function RunClientProcess(assemblies As List(Of String), entry As CodeChallenge_Entry, newChallengeConsolePath As String, ByRef overranMemory As Boolean, ByRef overranTime As Boolean) As String
        Dim challengeClientOutput As String = String.Empty
        Dim dataReceived As New DataReceivedEventHandler(
            Sub(sender As Object, e As DataReceivedEventArgs)
                SyncLock challengeClientOutput
                    challengeClientOutput &= e.Data & vbCrLf
                End SyncLock
            End Sub)

        _trace.TraceInformation("Initializing ProcessStart object")
        Dim clientArguments As String = String.Format("client ""{0}"" ""{1}"" {2}", assemblies(0), entry.TypeName, If(entry.IsTest.HasValue, entry.IsTest.ToString, "false"))
        _trace.TraceInformation("Arguments to be passed into the console host: '{0}'", clientArguments)
        Dim runnerSi As New ProcessStartInfo(newChallengeConsolePath, clientArguments)
        With runnerSi
            .CreateNoWindow = True
            .UseShellExecute = False
            .RedirectStandardOutput = True
        End With
        Dim runner As Process

        _trace.TraceInformation("Starting client process")
        runner = Process.Start(runnerSi)

        _trace.TraceInformation("Process is now running!!!")

        _trace.TraceInformation("Redirected the output to asynchronous reader.")
        runner.BeginOutputReadLine()
        _trace.TraceInformation("Adding handler to manage client output")
        AddHandler runner.OutputDataReceived, dataReceived


        challengeClientOutput = String.Empty

        Dim processTimer As Stopwatch = Stopwatch.StartNew

        While Not runner.HasExited
            Thread.Sleep(500)
            runner.Refresh()

            'SyncLock challengeClientOutput
            '    challengeClientOutput &= runner.StandardOutput.ReadToEnd()
            'End SyncLock

            If Not runner.HasExited Then
                Try
                    'Get the processor usage by doing something like this:
                    'http://www.codeproject.com/KB/system/processescpuusage.aspx

                    Dim memoryUsage As Long = runner.WorkingSet64 'GetProcessMemoryUsage(runner.ProcessName)

                    _trace.TraceInformation("The process is still running.  Memory Usage = {0:N0}/{1:N0} : Total Exection Time = {2:N0} milliseconds", memoryUsage, entry.CodeChallenge.MaximumMemoryUsageBytes, processTimer.ElapsedMilliseconds)
                    If Not entry.IsTest AndAlso (memoryUsage) > entry.CodeChallenge.MaximumMemoryUsageBytes Then
                        'Exceeded the memory size restrictions
                        _trace.TraceInformation("The process exceeded its memory restriction of {0:N0} bytes, currently using {1:N0} bytes.  Killing process.", entry.CodeChallenge.MaximumMemoryUsageBytes, memoryUsage)
                        runner.Kill()
                        overranMemory = True
                    End If

                    If processTimer.Elapsed.TotalSeconds > entry.CodeChallenge.MaximumRunningSeconds Then
                        'Exceed the time restrictions
                        _trace.TraceInformation("The process exceeded its time restriction of {0:N0} running seconds, executed for {1:N0} milliseconds.  Killing process.", entry.CodeChallenge.MaximumRunningSeconds, processTimer.ElapsedMilliseconds)
                        runner.Kill()
                        overranTime = True
                    End If
                Catch ex As Exception
                    _trace.TraceInformation("An error occurred while processing memory or time limitations.  This is just a warning.")
                    _trace.TraceInformation("{0}", ex.ToString)
                End Try
            End If
        End While

        processTimer.Stop()
        _trace.TraceInformation("The process has ended.")
        RemoveHandler runner.OutputDataReceived, dataReceived

        entry.TotalExecutionTime = processTimer.Elapsed.TotalMilliseconds

        If challengeClientOutput IsNot Nothing Then
            entry.ExecutionDetails = challengeClientOutput
        End If

        _trace.TraceInformation("The process ran for {0:N0} milliseconds.", entry.TotalExecutionTime)

        Return challengeClientOutput
    End Function

    Private Sub ClearUntrustedFolder()
        _trace.TraceInformation("Clearing all files from the untrusted execution folder: '{0}'", _challengeWorkingDirectory)
        If Directory.Exists(_challengeWorkingDirectory) Then
            For Each filePath As String In Directory.GetFiles(_challengeWorkingDirectory)
                _trace.TraceInformation("Deleting file '{0}'", filePath)
                Try
                    File.Delete(filePath)
                Catch ex As Exception
                    _trace.TraceInformation("Couldn't delete file {0} due to error", filePath)
                    _trace.TraceInformation(ex.ToString)
                End Try
                _trace.TraceInformation("File deleted")
            Next
            _trace.TraceInformation("All files have been removed.")
        Else
            _trace.TraceInformation("While trying to clear untrusted files, it was discovered that the untrusted files folder didn't exists, creating now.")
            Directory.CreateDirectory(_challengeWorkingDirectory)
            _trace.TraceInformation("Untrusted working directory ('{0}') created successfully", _challengeWorkingDirectory)
        End If
    End Sub

    Private Function InitializeChallengeAssemblies(entryAssembly As CodeChallenge_Entry, ByVal untrustedPath As String) As List(Of String)
        Try
            Dim returnValue As New List(Of String)

            _trace.TraceInformation("Loading dependent assemblies onto the local filesystem for entry with id {0}.", entryAssembly.id)
            _trace.TraceInformation("Assemblies will be loaded to the path '{0}'", untrustedPath)

            If Not Directory.Exists(untrustedPath) Then
                _trace.TraceInformation("The untrusted execution location does not exists, creating")
                Directory.CreateDirectory(untrustedPath)
            End If

            Dim db As New CodeChallengeModel(_connectionString)

            If entryAssembly IsNot Nothing Then
                _trace.TraceInformation("Found 1 entry that needs to have it's dependent assemblies loaded onto the local file system")

                If entryAssembly IsNot Nothing Then
                    Dim fileName As String

                    _trace.TraceInformation("SubmissionFullName = '{0}'", entryAssembly.AssemblyFullName)
                    fileName = Path.Combine(untrustedPath, GetAssemblyName(entryAssembly.AssemblyFullName) & ".dll")
                    If File.Exists(fileName) Then
                        _trace.TraceInformation("The submission ('{0}') already exists, about to throw invalid operation exception", fileName)
                        Throw New InvalidOperationException("Cannot write submission to the local file system because the file already exists, please contact the Code Challenge administrator")
                    End If
                    _trace.TraceInformation("Submission full path = '{0}'", fileName)
                    File.WriteAllBytes(fileName, entryAssembly.Submission)
                    _trace.TraceInformation("Wrote {0} bytes to the disk for the entry", entryAssembly.Submission.Length)
                    returnValue.Add(fileName)


                    _trace.TraceInformation("Executor = '{0}'", entryAssembly.CodeChallenge.ExecutorAssemblyFullName)
                    fileName = Path.Combine(untrustedPath, GetAssemblyName(entryAssembly.CodeChallenge.ExecutorAssemblyFullName) & ".dll")
                    If File.Exists(fileName) Then
                        _trace.TraceInformation("The executor ('{0}') already exists, about to throw invalid operation exception", fileName)
                        Throw New InvalidOperationException("Cannot write executor to the local file system because the file already exists, please contact the Code Challenge administrator")
                    End If
                    _trace.TraceInformation("Executor full path = '{0}'", fileName)
                    File.WriteAllBytes(fileName, entryAssembly.CodeChallenge.ExecutorAssembly)
                    _trace.TraceInformation("Wrote {0} bytes to the disk for the executor", entryAssembly.CodeChallenge.ExecutorAssembly.Length)
                    returnValue.Add(fileName)

                    _trace.TraceInformation("ExecutionCommon = '{0}'", entryAssembly.CodeChallenge.ExecutionCommonAssemblyFullName)
                    fileName = Path.Combine(untrustedPath, GetAssemblyName(entryAssembly.CodeChallenge.ExecutionCommonAssemblyFullName) & ".dll")
                    If File.Exists(fileName) Then
                        _trace.TraceInformation("The execution common assembly ('{0}') already exists, about to throw invalid operation exception", fileName)
                        Throw New InvalidOperationException("Cannot write ExecutionCommon to the local file system because the file already exists, please contact the Code Challenge administrator")
                    End If
                    _trace.TraceInformation("Execution common full path = '{0}'", fileName)
                    File.WriteAllBytes(fileName, entryAssembly.CodeChallenge.ExecutionCommonAssembly)
                    _trace.TraceInformation("Wrote {0} bytes to the disk for the exeuction common assembly", entryAssembly.CodeChallenge.ExecutionCommonAssembly.Length)
                    returnValue.Add(fileName)

                    _trace.TraceInformation("Writing {0} developer assembly(ies) to the local disk", entryAssembly.CodeChallenge.CodeChallenge_DeveloperAssembly.Count)
                    For Each devAssem In entryAssembly.CodeChallenge.CodeChallenge_DeveloperAssembly
                        _trace.TraceInformation("Developer assembly fullname = '{0}'", devAssem.assembly_fullname)
                        fileName = Path.Combine(untrustedPath, GetAssemblyName(devAssem.assembly_fullname) & ".dll")
                        _trace.TraceInformation("Developer assembly full path = '{0}'", fileName)
                        File.WriteAllBytes(fileName, devAssem.assembly)
                        _trace.TraceInformation("Wrote {0} bytes to the disk for the developer assembly", devAssem.assembly.Length)
                        returnValue.Add(fileName)
                    Next

                    'Dim codeChallengeAssemblies = From cca In db.CodeChallenge_Assembly
                    '                              Select cca
                    '_trace.TraceInformation("Writing {0} code challenge assembly(ies) to the local disk", codeChallengeAssemblies.Count)
                    'For Each codeChallengeAssembly In codeChallengeAssemblies
                    '    _trace.TraceInformation("Code Challenge assembly fullname = '{0}'", codeChallengeAssembly.AssemblyFullName)
                    '    fileName = Path.Combine(untrustedPath, GetAssemblyName(codeChallengeAssembly.AssemblyFullName) & ".dll")
                    '    _trace.TraceInformation("Code Challenge assembly full path = '{0}'", fileName)
                    '    File.WriteAllBytes(fileName, codeChallengeAssembly.assembly)
                    '    _trace.TraceInformation("Wrote {0} bytes to the disk for the developer assembly", codeChallengeAssembly.assembly.Length)
                    '    returnValue.Add(fileName)
                    'Next

                    _trace.TraceInformation("Done writing files to the local file system")
                    _trace.TraceInformation("Loaded {0} assemblies onto the local file system", returnValue.Count)
                Else
                    _trace.TraceInformation("There were no assemblies found to write to the disk")
                End If
            End If
            Return returnValue
        Catch ex As Exception
            _trace.TraceInformation("An error occurred while loading assemblies from the database.")
            _trace.TraceInformation("{0}", ex.ToString)
            Throw
        End Try
    End Function

    Private Sub ArchiveAssemblies(assemblies As List(Of String), archivePath As String, entryID As Integer, username As String)
        _trace.TraceInformation("Cleaning up assemblies that were written to the untrusted execution location")
        If assemblies IsNot Nothing Then
            If Not String.IsNullOrWhiteSpace(archivePath) AndAlso Not Directory.Exists(archivePath) Then
                Directory.CreateDirectory(archivePath)
            End If

            Dim destinationFolderPath As String = Path.Combine(archivePath, DateTime.Now.ToString("yyyyMMdd HHmmss") & "_" & username & "_" & entryID)
            If Not Directory.Exists(destinationFolderPath) Then
                Directory.CreateDirectory(destinationFolderPath)
            End If

            Dim resultsFilePath As String = Path.Combine(_challengeWorkingDirectory, "Results.xml")
            Dim destinationResultsFilePath As String = Path.Combine(destinationFolderPath, "Results.xml")
            If File.Exists(resultsFilePath) Then
                File.Move(resultsFilePath, destinationResultsFilePath)
            End If

            For Each filePath In assemblies
                If File.Exists(filePath) Then
                    If Not String.IsNullOrWhiteSpace(archivePath) Then
                        Try
                            _trace.TraceInformation("Attempting to archive assembly: {0}", filePath)
                            Dim destFilePath As String = Path.Combine(destinationFolderPath, Path.GetFileName(filePath))

                            File.Move(filePath, destFilePath)
                            _trace.TraceInformation("File successfully archived")
                        Catch ex As Exception
                            _trace.TraceInformation("An exception occurred while attempting to archive entry assembly: {0}", ex.ToString)

                            Try
                                File.Delete(filePath)
                                _trace.TraceInformation("File successfully removed")
                            Catch ex2 As Exception
                                _trace.TraceInformation("An exception occurred while attempting to delete entry assembly: {0}", ex.ToString)
                            End Try
                        End Try
                    Else
                        Try
                            _trace.TraceInformation("Attempting to delete assembly: {0}", filePath)
                            File.Delete(filePath)
                            _trace.TraceInformation("File successfully removed")
                        Catch ex As Exception
                            _trace.TraceInformation("An exception occurred while attempting to delete entry assembly: {0}", ex.ToString)
                        End Try
                    End If
                Else
                    _trace.TraceInformation("Couldn't find assembly to remove '{0}'", filePath)
                End If
            Next
        End If
    End Sub

    Private Shared Function GetAssemblyName(assemblyFullname As String) As String
        Dim returnValue As String = assemblyFullname
        Dim match As Match = Regex.Match(assemblyFullname, "^[^,]+(?=,)")
        If match.Success Then
            returnValue = match.Value
        End If

        Return returnValue
    End Function

    'Private Function CurrentDomain_AssemblyResolve(sender As Object, args As ResolveEventArgs) As Assembly
    '    Trace.TraceInformation("CurrentDomain_AssemblyResolve:  Attempting to resolve assembly '{0}'", args.Name)

    '    Dim returnValue As Assembly = Nothing
    '    _trace.TraceInformation("Establishing connection to database ('{0}')", _connectionString)
    '    Dim db As New CodeChallengeModel(_connectionString)

    '    _trace.TraceInformation("Attempting to resolve assembly from the database")
    '    Dim result = (From assm In db.CodeChallenge_Assembly Where assm.AssemblyFullName.ToLower = args.Name.ToLower).FirstOrDefault

    '    If result IsNot Nothing Then
    '        _trace.TraceInformation("The assembly was located and is about to be loaded into the ")
    '        returnValue = Assembly.ReflectionOnlyLoad(result.assembly)
    '    End If

    '    Return returnValue
    'End Function

    Private Function ProcessResult(ByVal result As String) As List(Of ChallengeResult)
        Dim returnValue As List(Of ChallengeResult) = Nothing
        If Not String.IsNullOrWhiteSpace(result) Then
            Dim x As New XmlSerializer(GetType(List(Of ChallengeResult)))

            Using reader As New StringReader(result)
                returnValue = x.Deserialize(reader)
            End Using
        End If
        Return returnValue
    End Function

    Private Shared _perfCounterCache As New Dictionary(Of String, PerformanceCounter)
    Private Shared Function GetProcessMemoryUsage(ByVal processName As String) As Long
        Dim workingSetPerfCounter As PerformanceCounter
        If _perfCounterCache.ContainsKey(processName) Then
            workingSetPerfCounter = _perfCounterCache(processName)
        Else
            workingSetPerfCounter = New PerformanceCounter("Process", "Working Set", processName)
            _perfCounterCache.Add(processName, workingSetPerfCounter)
        End If

        Return workingSetPerfCounter.RawValue
    End Function


End Class
