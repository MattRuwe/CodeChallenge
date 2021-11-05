Imports System.Text.RegularExpressions
Imports System.Web

Public Class ResultDetailsDownloader
    Implements IHttpHandler

    Public ReadOnly Property IsReusable As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        Dim match As Match = Regex.Match(context.Request.CurrentExecutionFilePath, "(?i)(?<=/)\d+(?=.results$)")
        If match.Success Then
            Dim db As New CodeChallengeModel()
            Dim result = (From ccer In db.CodeChallenge_Entry_Result
                         Where ccer.id = match.Value
                         Select ccer).FirstOrDefault

            If result IsNot Nothing Then
                Dim sample() As Byte = result.test_result_data
                context.Response.ContentType = "application/octet-stream"
                context.Response.AddHeader("Content-disposition", "attachment; filename=result.zip")
                context.Response.AddHeader("Content-Length", sample.Length)
                context.Response.BinaryWrite(sample)
                context.Response.End()
            End If
        Else
            context.Response.Write(String.Format("The CurrentExecutionFilePath ('{0}') doesn't appear to be valid.", context.Request.CurrentExecutionFilePath))
        End If
    End Sub
End Class
