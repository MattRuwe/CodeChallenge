Imports System.Reflection
Imports OmahaMTG.Challenge.ExecutionCommon

Public Class ChallengeEntry
    Property Id As Integer
    Property CodeChallengeId As Integer
    Property AuthorUserID As Guid
    Property Submission As Byte()
    Property TypeName As String
    Property SubmissionAssembly As Assembly
    Property DateAdded As DateTime
    Property DateRan As DateTime?
    Property TotalExecutionTime As TimeSpan
    Property Results As List(Of ChallengeResult)
    Property MaxRunningTime As TimeSpan
    Property MaxMemoryUsageBytes As Integer
End Class
