Public Class BindablePasswordBox
    Inherits Decorator
    ''' <summary>
    ''' The password dependency property.
    ''' </summary>
    Public Shared ReadOnly PasswordProperty As DependencyProperty

    Private _isPreventCallback As Boolean
    Private ReadOnly _savedCallback As RoutedEventHandler

    ''' <summary>
    ''' Static constructor to initialize the dependency properties.
    ''' </summary>
    Shared Sub New()
        PasswordProperty = DependencyProperty.Register("Password", GetType(String), GetType(BindablePasswordBox), New FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, New PropertyChangedCallback(AddressOf OnPasswordPropertyChanged)))
    End Sub

    ''' <summary>
    ''' Saves the password changed callback and sets the child element to the password box.
    ''' </summary>
    Public Sub New()
        _savedCallback = AddressOf HandlePasswordChanged

        Dim passwordBox As New PasswordBox()
        AddHandler passwordBox.PasswordChanged, _savedCallback
        Child = passwordBox
    End Sub

    ''' <summary>
    ''' The password dependency property.
    ''' </summary>
    Public Property Password() As String
        Get
            Return TryCast(GetValue(PasswordProperty), String)
        End Get
        Set(value As String)
            SetValue(PasswordProperty, value)
        End Set
    End Property

    ''' <summary>
    ''' Handles changes to the password dependency property.
    ''' </summary>
    ''' <param name="d">the dependency object</param>
    ''' <param name="eventArgs">the event args</param>
    Private Shared Sub OnPasswordPropertyChanged(d As DependencyObject, eventArgs As DependencyPropertyChangedEventArgs)
        Dim bindablePasswordBox As BindablePasswordBox = DirectCast(d, BindablePasswordBox)
        Dim passwordBox As PasswordBox = DirectCast(bindablePasswordBox.Child, PasswordBox)

        If bindablePasswordBox._isPreventCallback Then
            Return
        End If

        RemoveHandler passwordBox.PasswordChanged, bindablePasswordBox._savedCallback
        passwordBox.Password = If((eventArgs.NewValue IsNot Nothing), eventArgs.NewValue.ToString(), "")
        AddHandler passwordBox.PasswordChanged, bindablePasswordBox._savedCallback
    End Sub

    ''' <summary>
    ''' Handles the password changed event.
    ''' </summary>
    ''' <param name="sender">the sender</param>
    ''' <param name="eventArgs">the event args</param>
    Private Sub HandlePasswordChanged(sender As Object, eventArgs As RoutedEventArgs)
        Dim passwordBox As PasswordBox = DirectCast(sender, PasswordBox)

        _isPreventCallback = True
        Password = passwordBox.Password
        _isPreventCallback = False
    End Sub
End Class
