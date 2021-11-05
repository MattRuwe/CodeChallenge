Imports System.IO
Imports System.Reflection
Imports OmahaMTG.Challenge.ChallengeCommon
Imports OmahaMTG.Challenge.ExecutionCommon
Imports System.Xml.Serialization
Imports System.Configuration
Imports OmahaMTG.Challenge.Manager

Module ChallengeConsole

    Private _untrustedPath As String

    Sub Main(ByVal args() As String)
        Try
            If args Is Nothing OrElse args.Length < 1 Then
                ShowBanner()
                Console.WriteLine("No Parameters")
                ShowUsage()
            ElseIf args(0) = "host" Then
                ShowBanner()
                Dim connString As String = ConfigurationManager.ConnectionStrings("CodeChallengeModelChallengeManager").ConnectionString

                Dim host As New ChallengeHost(
                    connString,
                    My.Settings.ChallengeExecutionWorkingDirectory,
                    My.Settings.NewChallangeCheckIntervalSeconds,
                    Assembly.GetExecutingAssembly.Location,
                    My.Settings.ArchivePath)
                host.Start()
                Console.WriteLine("Press any key to stop host...")
                Console.ReadKey(True)
            ElseIf args(0) = "client" Then
                'Console.OpenStandardOutput(Integer.MaxValue)
                AddHandler AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf CurrentDomain_ReflectionOnlyAssemblyResolve
                If args Is Nothing OrElse args.Length < 4 Then
                    Console.WriteLine("Client - insufficient parameters")
                    ShowUsage()
                Else
                    Dim implementationAssembly As Assembly
                    Dim implementationType As Type
                    Dim isTest As Boolean

                    If Not Boolean.TryParse(args(3), isTest) Then
                        Console.WriteLine("IsTest argument ({0}) is invalid.  The argument needs to be 'true' or 'false'.", args(4))
                    Else
                        Console.WriteLine("Istest = {0}", isTest)
                    End If

                    If Not File.Exists(args(1)) Then
                        Console.WriteLine("The file '{0}' could not be found")
                        Exit Sub
                    End If

                    implementationAssembly = Assembly.ReflectionOnlyLoadFrom(args(1))
                    Console.WriteLine("Loaded assembly {0}", implementationAssembly.FullName)
                    Console.WriteLine("  from {0}", implementationAssembly.Location)

                    _untrustedPath = Path.GetDirectoryName(args(1))

                    implementationType = implementationAssembly.GetType(args(2))
                    If implementationType Is Nothing Then
                        Console.WriteLine("The type '{0}' could not be loaded from the assembly", args(2))
                        Exit Sub
                    End If
                    Console.WriteLine("Loaded type " & implementationType.FullName)

                    Console.WriteLine("Creating challenge client")
                    Dim manager As New ChallengeClient(implementationType, isTest)
                    Console.WriteLine("Executing challenge")

                    Dim results As List(Of ChallengeResult) = manager.ExecuteChallenge()

                    Console.WriteLine("The challenge finished.")

                    Using resultsOutput As New StreamWriter(Path.Combine(Path.GetDirectoryName(args(1)), "Results.xml"))
                        Dim x As New XmlSerializer(results.GetType())
                        x.Serialize(resultsOutput, results)
                    End Using
                    Dim i As Integer = 0
                    For Each result As ChallengeResult In results
                        i += 1
                        Console.WriteLine("Result #{0}", i)
                        Console.WriteLine("  Duration:       {0}", result.DurationTicks.ToString())
                        Console.WriteLine("  Successful:     {0}", result.Successful.ToString)
                        Console.WriteLine("  Result Message: {0}", result.ResultMessage)
                        Console.WriteLine("  Error:          {0}", result.DisplayError)
                        Console.WriteLine("  Detailed Error: {0}", result.DetailedError)
                    Next

                End If
            Else
                Console.WriteLine("Unrecognized mode '{0}'", args(0))
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Private Sub ShowUsage()
        Console.WriteLine("Usage:")
        Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName & " client <challenge assembly> <challenge type> <is test - [true|false]>")
        Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName & " host <path to untrusted executors>")
    End Sub

    Private Sub ShowBanner()
        Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName)
        Console.WriteLine("OmahaMTG Code Challenge Execution Tool - Version " & Assembly.GetEntryAssembly.GetName.Version.ToString(4))
        Console.WriteLine("Copyright (c) 2011 - Omaha Microsoft Technology Group")
    End Sub

    Private Function CurrentDomain_ReflectionOnlyAssemblyResolve(ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly
        Dim name As AssemblyName
        Dim assemblyToLoadPath As String
        Dim returnValue As Assembly = Nothing
        Try
            name = New AssemblyName(args.Name)
            assemblyToLoadPath = Path.Combine(_untrustedPath, name.Name & ".dll")

            Console.WriteLine("Attempting to load assembly {0} from {1}", args.Name, assemblyToLoadPath)

            If File.Exists(assemblyToLoadPath) Then
                Console.WriteLine("Assembly exists, loading from path ""{0}""", assemblyToLoadPath)
                returnValue = Assembly.ReflectionOnlyLoadFrom(assemblyToLoadPath)

                If returnValue Is Nothing Then
                    Console.WriteLine("The assembly could not be loaded")
                End If
            Else
                Console.WriteLine("The assembly does NOT exist, returning nothing")
            End If
        Catch ex As Exception
            Console.WriteLine(String.Format("An error occurred while attempting to load the assembly {0}", args.Name), ex)
        End Try

        Return Nothing
    End Function

End Module
