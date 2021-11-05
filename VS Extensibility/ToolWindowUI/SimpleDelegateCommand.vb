Public Class SimpleDelegateCommand
    Implements ICommand

    Private _executeDelegate As Action(Of Object)

    Public Sub New(executeDelegate As Action(Of Object))
        _executeDelegate = executeDelegate
    End Sub


    Public Function CanExecute(parameter As Object) As Boolean Implements System.Windows.Input.ICommand.CanExecute
        Return True
    End Function

    Public Event CanExecuteChanged(sender As Object, e As System.EventArgs) Implements System.Windows.Input.ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements System.Windows.Input.ICommand.Execute
        _executeDelegate(parameter)
    End Sub
End Class
