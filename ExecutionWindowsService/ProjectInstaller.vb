Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.IO
Imports System.Text.RegularExpressions

Public Class ProjectInstaller

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Protected Overrides Sub OnAfterInstall(savedState As System.Collections.IDictionary)
        MyBase.OnAfterInstall(savedState)

        'TargetDir is defined in the CustomActionData property in the custom actions editor
        Dim assemblyPath As String = ServiceInstaller1.Context.Parameters("assemblypath")

        Dim configFilePath As String = Path.Combine(Path.GetDirectoryName(assemblyPath), Path.GetFileName(assemblyPath) & ".config")
        Dim untrustedWorkingFolder As String = Path.Combine(Path.GetDirectoryName(assemblyPath), "UntrustedWorkingFolder")
        Dim consolePath As String = Path.Combine(Path.GetDirectoryName(assemblyPath), "ChallengeConsole.exe")
        Dim configFileContent As String

        Using sr As New StreamReader(configFilePath)
            configFileContent = sr.ReadToEnd
        End Using

        configFileContent = Regex.Replace(configFileContent, "(?is)(?<=<configuration>.*?<applicationsettings>\s+<OmahaMTG.Challenge.ExecutionWindowsService.My.MySettings>.*?<setting name=""ChallengeExecutionWorkingDirectory"" serializeAs=""String"">\s+<value>).*?(?=</value>)", untrustedWorkingFolder)
        configFileContent = Regex.Replace(configFileContent, "(?is)(?<=<configuration>.*?<applicationsettings>\s+<OmahaMTG.Challenge.ExecutionWindowsService.My.MySettings>.*?<setting name=""ChallengeConsolePath"" serializeAs=""String"">\s+<value>).*?(?=</value>)", consolePath)
        configFileContent = Regex.Replace(configFileContent, "(?is)(?<=<configuration>.*?<applicationsettings>\s+<OmahaMTG.Challenge.ExecutionWindowsService.My.MySettings>.*?<setting name=""ArchivePath"" serializeAs=""String"">\s+<value>).*?(?=</value>)", Path.Combine(untrustedWorkingFolder, "Archive"))

        Using sw As New StreamWriter(configFilePath, False)
            sw.Write(configFileContent)
        End Using

    End Sub
End Class
