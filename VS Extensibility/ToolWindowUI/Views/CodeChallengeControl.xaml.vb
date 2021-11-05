Imports System.ServiceModel
Imports System.ComponentModel
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.AuthenticationService
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService
Imports System.ServiceModel.Channels
Imports EnvDTE80
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.ViewModels

Public Class CodeChallengeControl


    

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        DataContext = New CodeChallengeViewModel()
    End Sub

    Private _dte As DTE2
    Public Property DTE As DTE2
        Get
            Return _dte
        End Get
        Set(value As DTE2)
            _dte = value
            If DataContext IsNot Nothing Then
                CType(Me.DataContext, CodeChallengeViewModel).DTE = value
            End If
        End Set
    End Property
End Class
