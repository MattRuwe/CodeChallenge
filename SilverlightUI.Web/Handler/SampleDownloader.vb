Imports System.Web
Imports System.Text.RegularExpressions

Public Class SampleDownloader
    Implements IHttpHandler

    ''' <summary>
    '''  You will need to configure this handler in the web.config file of your 
    '''  web and register it with IIS before being able to use it. For more information
    '''  see the following link: http://go.microsoft.com/?linkid=8101007
    ''' </summary>
#Region "IHttpHandler Members"

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim match As Match = Regex.Match(context.Request.CurrentExecutionFilePath, "(?i)(?<=/)\d+(?=.sample$)")
        If match.Success Then
            Dim db As New CodeChallengeModel()
            Dim result = (From cc In db.CodeChallenges
                         Where cc.id = match.Value
                         Select cc).FirstOrDefault

            If result IsNot Nothing Then

                If result.SampleDownloads.HasValue Then
                    result.SampleDownloads += 1
                Else
                    result.SampleDownloads = 1
                End If

                db.SaveChanges()

                Dim sample() As Byte = result.SampleProject
                context.Response.ContentType = "application/octet-stream"
                context.Response.AddHeader("Content-disposition", String.Format("attachment; filename={0}", result.SampleProjectFilename))
                context.Response.AddHeader("Content-Length", sample.Length)
                context.Response.BinaryWrite(sample)
                context.Response.End()
            End If
        Else
            context.Response.Write(String.Format("The CurrentExecutionFilePath ('{0}') doesn't appear to be valid.", context.Request.CurrentExecutionFilePath))
        End If

    End Sub

#End Region

End Class
