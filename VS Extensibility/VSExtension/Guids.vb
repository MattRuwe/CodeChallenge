Imports System

Class GuidList
    Private Sub New()
    End Sub

    Public Const guidVSExtensionPkgString As String = "475eed61-7828-4eaf-862b-47b514177ed1"
    Public Const guidVSExtensionCmdSetString As String = "43b70911-ed7c-41d7-9e26-6c3a089087bb"
    Public Const guidToolWindowPersistanceString As String = "f6073606-201b-4e72-b34b-f905730523dd"

    Public Shared ReadOnly guidVSExtensionCmdSet As New Guid(guidVSExtensionCmdSetString)
End Class