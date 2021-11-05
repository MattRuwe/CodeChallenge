Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Linq.Expressions
Imports System.Runtime.CompilerServices

Public Module ExpressionExtensions
    <Extension>
    Public Function GetExpressionPath(exp As Expression) As String
        If exp Is Nothing Then
            Throw New ArgumentNullException("exp")
        End If

        Select Case exp.NodeType
            Case ExpressionType.MemberAccess
                Dim name = If(GetExpressionPath(DirectCast(exp, MemberExpression).Expression), "")

                If name.Length > 0 Then
                    name += "."
                End If

                Return name + DirectCast(exp, MemberExpression).Member.Name
            Case ExpressionType.[Call]
                Return GetExpressionPath(DirectCast(exp, MethodCallExpression).[Object])
            Case ExpressionType.Convert, ExpressionType.Quote
                Return GetExpressionPath(DirectCast(exp, UnaryExpression).Operand)

            Case ExpressionType.Lambda
                Return GetExpressionPath(DirectCast(exp, LambdaExpression).Body)
            Case Else

                Return Nothing
        End Select
    End Function
End Module
