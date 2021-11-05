Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports SilverlightUI.LoginUI
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

''' <summary>
''' <see cref="UserControl"/> class providing the main UI for the application.
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    Private _currentUri As Uri
    ''' <summary>
    ''' Creates a new <see cref="MainPage"/> instance.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        Me.loginContainer.Child = New LoginStatus()
    End Sub

    ''' <summary>
    ''' After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
    ''' </summary>
    Private Sub ContentFrame_Navigated(ByVal sender As Object, ByVal e As NavigationEventArgs)
        For Each child As UIElement In LinksStackPanel.Children
            Dim hb As HyperlinkButton = TryCast(child, HyperlinkButton)
            If hb IsNot Nothing AndAlso hb.NavigateUri IsNot Nothing Then
                If hb.NavigateUri.ToString().Equals(e.Uri.ToString()) Then
                    VisualStateManager.GoToState(hb, "ActiveLink", True)
                    _currentUri = e.Uri
                Else
                    VisualStateManager.GoToState(hb, "InactiveLink", True)
                End If
            End If
        Next
    End Sub


    ''' <summary>
    ''' If an error occurs during navigation, show an error window
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class