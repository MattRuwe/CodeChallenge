Imports OmahaMTG.Challenge.ChallengeCommon
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Security
Imports System.Net.Mail
Imports System.Reflection

<Serializable()>
Public MustInherit Class ChallengeExecutorBase(Of T As IChallenge)
    Private _results As List(Of ChallengeResult)

    Private _startingCpuCycles As ULong

    Private Sub InitializeExecutor(ByVal challenge As T)
        _challenge = challenge
        _results = New List(Of ChallengeResult)
    End Sub

    Public Property IsTest As Boolean

    <SecuritySafeCritical()>
    Public Function RunChallenge(ByVal challenge As T) As List(Of ChallengeResult)

        'Dim assemblyCount As Integer = 0
        'Dim typeCount As Integer = 0
        'Dim methodCount As Integer = 0
        'For Each assembly As Assembly In AppDomain.CurrentDomain.GetAssemblies()
        '    assemblyCount += 1
        '    For Each type As Type In assembly.GetTypes()
        '        typeCount += 1
        '        For Each method As MethodInfo In type.GetMethods(BindingFlags.DeclaredOnly Or BindingFlags.NonPublic Or BindingFlags.[Public] Or BindingFlags.Instance Or BindingFlags.[Static])
        '            methodCount += 1
        '            If Not method.IsAbstract Then
        '                Try
        '                    System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle)
        '                Catch ex As Exception
        '                    Console.WriteLine("Failed to prepare the method: {0}", ex.ToString())
        '                End Try
        '            End If
        '        Next
        '    Next
        'Next

        'Console.WriteLine("Finished preparing methods in {0} assemblies, {1} types, and {2} methods.", assemblyCount, typeCount, methodCount)

        InitializeExecutor(challenge)
        Try
            _startingCpuCycles = ChallengeHelper.GetCurrentProcessCpuCycle()
            RunChallengeOverride()
        Catch ex As Exception
            ResultsAvailable(New ChallengeResult With {
                            .ResultMessage = "An error occurred while the user challenge was running",
                            .DisplayError = "A runtime error occured",
                            .DetailedError = ex.ToString,
                            .Successful = False})
        End Try

        Return _results
    End Function

    <SecuritySafeCritical()>
    Protected Sub ResultsAvailable(ByVal result As ChallengeResult)
        If result.CpuCyclesUsed = 0 Then
            Dim currentCpuCycles As ULong = ChallengeHelper.GetCurrentProcessCpuCycle()
            result.CpuCyclesUsed = currentCpuCycles - _startingCpuCycles
            _startingCpuCycles = currentCpuCycles
        End If

        Try
            result.AuthorNotes = _challenge.AuthorNotes
        Catch
            result.AuthorNotes = String.Empty
        End Try

        If (String.IsNullOrEmpty(result.ResultMessage)) Then
            result.ResultMessage = "No result message provided."
        End If

        If Not String.IsNullOrWhiteSpace(result.AuthorNotes) AndAlso MaxAuthorNotesLength > -1 AndAlso result.AuthorNotes.Length > MaxAuthorNotesLength Then
            result.AuthorNotes = result.AuthorNotes.Substring(0, MaxAuthorNotesLength)
        End If
        If Not IsTest Then
            result.TestResults.Clear()
        End If
        _results.Add(result)
    End Sub

    Protected MustOverride Sub RunChallengeOverride()
    Protected MustOverride ReadOnly Property MaxAuthorNotesLength As Integer

    Private _challenge As T
    Protected ReadOnly Property Challenge() As T
        Get
            Return _challenge
        End Get
    End Property


End Class
