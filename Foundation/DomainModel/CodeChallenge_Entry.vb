'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class CodeChallenge_Entry
    Public Property id As Integer
    Public Property CodeChallenge_Id As Integer
    Public Property AuthorUserId As System.Guid
    Public Property AssemblyFullName As String
    Public Property Submission As Byte()
    Public Property TypeName As String
    Public Property DateAdded As Date
    Public Property DateRan As Nullable(Of Date)
    Public Property TotalExecutionTime As Nullable(Of Integer)
    Public Property FinalScore As Nullable(Of Long)
    Public Property ExecutionDetails As String
    Public Property IsPublished As Boolean
    Public Property Status As String
    Public Property IsTest As Nullable(Of Boolean)

    Public Overridable Property CodeChallenge As CodeChallenge
    Public Overridable Property CodeChallenge_Entry_Result As ICollection(Of CodeChallenge_Entry_Result) = New HashSet(Of CodeChallenge_Entry_Result)

End Class