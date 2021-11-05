Imports System.Runtime.Remoting
Imports System.Reflection
Imports System.IO
Imports System.Web
Imports System.Web.Hosting
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.Permissions

<Serializable()>
Public Class ChallengeHelper

    <SecuritySafeCritical()>
    Public Shared Function GetCurrentProcessCpuCycle() As ULong
        Dim returnValue As ULong = 0
        Try
            Dim permSet As New PermissionSet(PermissionState.Unrestricted)
            permSet.Assert()
            Dim currentProcess As Process = Process.GetCurrentProcess
            Dim processHandle As IntPtr = currentProcess.Handle
            QueryProcessCycleTime(processHandle, returnValue)
            PermissionSet.RevertAssert()
        Catch ex As Exception
            Debug.Print(ex.ToString())
            returnValue = 0
        End Try

        Return returnValue
    End Function

    <DllImport("kernel32.dll")> _
    Private Shared Function QueryProcessCycleTime(ProcessHandle As IntPtr, ByRef CycleTime As ULong) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function



End Class
