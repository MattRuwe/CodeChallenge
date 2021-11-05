Imports System.Reflection
Imports OmahaMTG.Challenge.ChallengeCommon
Imports System.IO
Imports System.Web.Hosting
Imports System.Configuration
Imports OmahaMTG.Challenge.ExecutionCommon
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core

Partial Public Class ChallengeManager
    Private _connString As String

    Public Sub New(ByVal connectionString As String)
        _connString = connectionString
    End Sub

    Public Function IsUserValid(ByVal userid As Guid) As Boolean
        Dim db As New CodeChallengeModel(_connString)

        Return (From au In db.aspnet_Users Where au.UserId = userid).Count() > 0
    End Function

    Public Function IsAssemblyValid(ByVal assemblyBytes() As Byte, ByVal implementationTypeString As String) As AssemblyValidationResult
        Dim loadingDomain As AppDomain = AppDomain.CreateDomain("LoadingDomain")
        loadingDomain.SetData("assemblyBytes", assemblyBytes)
        loadingDomain.SetData("implementationTypeString", implementationTypeString)
        loadingDomain.DoCallBack(AddressOf IsAssemblyValidAd)

        Dim returnValue As AssemblyValidationResult = loadingDomain.GetData("returnValue")

        Return returnValue
    End Function

    Private Sub IsAssemblyValidAd()
        Dim assemblybytes() As Byte = AppDomain.CurrentDomain.GetData("assemblyBytes")
        Dim implementationTypeString As String = AppDomain.CurrentDomain.GetData("implementationTypeString")
        Dim returnValue As New AssemblyValidationResult With {.IsValid = True}

        AddHandler AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf ReflectionOnlyAssemblyResolve

        Dim entryAssembly As Assembly = Nothing
        Dim implementationtype As Type = Nothing

        Try
            entryAssembly = Assembly.ReflectionOnlyLoad(assemblybytes)

            If entryAssembly Is Nothing Then
                returnValue.IsValid = False
                returnValue.ValidationMessage = "The assembly could not be loaded"
            Else
                returnValue.AssemblyFullName = entryAssembly.FullName
            End If
        Catch ex As Exception
            returnValue.IsValid = False
            returnValue.ValidationMessage = String.Format("An error occurred while loading the assembly: {0}{1}", Environment.NewLine, ex.ToString)
        End Try

        If returnValue.IsValid Then
            Try
                implementationtype = entryAssembly.GetType(implementationTypeString)
                If implementationtype Is Nothing Then
                    returnValue.IsValid = False
                    returnValue.ValidationMessage = "The type could not be loaded"
                End If
            Catch ex As Exception
                returnValue.IsValid = False
                returnValue.ValidationMessage = String.Format("An error occurred while retrieving the type from the assembly: {0}{1}", Environment.NewLine, ex.ToString)
            End Try
        End If

        If returnValue.IsValid Then
            Try
                If implementationtype.GetInterface(GetType(IChallenge).FullName) Is Nothing Then
                    returnValue.IsValid = False
                    returnValue.ValidationMessage = "The type does not derive from IChallenge"
                End If
            Catch ex As Exception
                returnValue.IsValid = False
                returnValue.ValidationMessage = String.Format("An error validating the type derives from IChallenge: {0}{1}", Environment.NewLine, ex.ToString)
            End Try
        End If

        AppDomain.CurrentDomain.SetData("returnValue", returnValue)
        RemoveHandler AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve, AddressOf ReflectionOnlyAssemblyResolve
    End Sub

    Public Sub InsertEntry(ByVal authorId As Guid, ByVal challengeId As Integer, ByVal submission() As Byte, ByVal implementationTypeName As String)
        Dim db As New CodeChallengeModel(_connString)

        Dim submissionAssembly As Assembly = Assembly.ReflectionOnlyLoad(submission)
        Dim assemblyFullname As String = submissionAssembly.FullName

        Dim entry As CodeChallenge_Entry = db.CodeChallenge_Entry.CreateObject()
        With entry
            .AuthorUserId = authorId
            .CodeChallenge_Id = challengeId
            .DateAdded = DateTime.Now
            .AssemblyFullName = assemblyFullname
            .Submission = submission
            .TypeName = implementationTypeName
        End With

        db.AddObject("CodeChallenge_Entry", entry)
    End Sub

    Public Function GetAssemblyFullNameWithoutLoading(ByVal assemblyBytes() As Byte) As String
        Dim loadingDomain As AppDomain = CreateLoadingAppdomain()

        loadingDomain.SetData("assemblyBytes", assemblyBytes)

        loadingDomain.DoCallBack(AddressOf GetAssemblyFullName)
        Dim returnValue As String = loadingDomain.GetData("assemblyFullname")

        AppDomain.Unload(loadingDomain)

        Return returnValue
    End Function

    Private Sub GetAssemblyFullName()
        Try
            Dim assemblyBytes() As Byte = AppDomain.CurrentDomain.GetData("assemblyBytes")
            Dim amb As Assembly = Assembly.ReflectionOnlyLoad(assemblyBytes)
            AppDomain.CurrentDomain.SetData("assemblyFullname", amb.FullName)
        Catch ex As Exception
            AppDomain.CurrentDomain.SetData("assemblyFullname", String.Empty)
        End Try
    End Sub

    Public Function CreateLoadingAppdomain() As AppDomain
        Dim appdomainsetup As New AppDomainSetup

        If HostingEnvironment.IsHosted Then
            appdomainsetup.PrivateBinPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "bin")
            appdomainsetup.ApplicationBase = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "bin")
        Else
            appdomainsetup.PrivateBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location)
            appdomainsetup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location)
        End If

        Dim returnValue As AppDomain = AppDomain.CreateDomain("LoadingDomain", Nothing, appdomainsetup)
        Return returnValue
    End Function

    ''' <summary>
    ''' Preloads assemblies in the reflection only context
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReflectionOnlyAssemblyResolve(ByVal sender As Object, ByVal args As ResolveEventArgs) As Assembly
        Dim currentAssemblyPath As String = Assembly.GetExecutingAssembly.Location

        Dim name As New AssemblyName(args.Name)

        Dim assemblyToLoadPath As String = Path.Combine(Path.GetDirectoryName(currentAssemblyPath), name.Name & ".dll")

        If File.Exists(assemblyToLoadPath) Then
            Return Assembly.ReflectionOnlyLoadFrom(assemblyToLoadPath)
        Else
            Dim db As New CodeChallengeModel(_connString)
            Dim result = (From dev In db.CodeChallenge_DeveloperAssembly Where dev.assembly_fullname.ToLower = name.FullName.ToLower Select dev.assembly).FirstOrDefault()

            If result IsNot Nothing AndAlso result.Length > 0 Then
                Return Assembly.ReflectionOnlyLoad(result)
            Else
                result = (From assem In db.CodeChallenge_Assembly Where assem.AssemblyFullName.ToLower = name.FullName.ToLower Select assem.assembly).FirstOrDefault()
                If result IsNot Nothing Then
                    Return Assembly.ReflectionOnlyLoad(result)
                Else
                    Return Nothing
                End If
            End If

        End If
    End Function

    Public Shared Function CompressFileResultToZipByteArray(fileResults As List(Of FileResult)) As Byte()
        Dim returnValue As Byte()

        Using outputStream As New MemoryStream
            Dim zipStream As New ZipOutputStream(outputStream)
            zipStream.SetLevel(9)

            For Each fileResult As FileResult In fileResults
                Using fileResultStream As New MemoryStream(fileResult.Contents)
                    Dim zipEntry As New ZipEntry(fileResult.Filename)
                    zipStream.PutNextEntry(zipEntry)
                    StreamUtils.Copy(fileResultStream, zipStream, New Byte(4095) {})
                    zipStream.CloseEntry()
                End Using
            Next
            zipStream.IsStreamOwner = False
            zipStream.Close()
            outputStream.Position = 0
            returnValue = outputStream.ToArray()
        End Using

        Return returnValue
    End Function

    'Public Function CreateToMemoryStream(memStreamIn As MemoryStream, zipEntryName As String) As MemoryStream

    '    Dim outputMemStream As New MemoryStream()
    '    Dim zipStream As New ZipOutputStream(outputMemStream)

    '    zipStream.SetLevel(3)       '0-9, 9 being the highest level of compression
    '    Dim newEntry As New ZipEntry(zipEntryName)
    '    newEntry.DateTime = DateTime.Now

    '    zipStream.PutNextEntry(newEntry)

    '    StreamUtils.Copy(memStreamIn, zipStream, New Byte(4095) {})
    '    zipStream.CloseEntry()

    '    zipStream.IsStreamOwner = False     ' False stops the Close also Closing the underlying stream.
    '    zipStream.Close()           ' Must finish the ZipOutputStream before using outputMemStream.
    '    outputMemStream.Position = 0
    '    Return outputMemStream

    '    ' Alternative outputs:
    '    ' ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
    '    Dim byteArrayOut As Byte() = outputMemStream.ToArray()

    '    ' GetBuffer returns a raw buffer raw and so you need to account for the true length yourself.
    '    Dim byteArrayOut As Byte() = outputMemStream.GetBuffer()
    '    Dim len As Long = outputMemStream.Length
    'End Function

End Class
