Imports System.Web
Imports System.Text.RegularExpressions

Public Class DocumentationDownloader
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
        Dim match As Match = Regex.Match(context.Request.CurrentExecutionFilePath, "(?i)(?<=/)\d+(?=.documentation$)")
        If match.Success Then
            Dim db As New CodeChallengeModel()
            Dim result = (From cc In db.CodeChallenges
                         Where cc.id = match.Value
                         Select cc).FirstOrDefault

            If result IsNot Nothing Then

                If result.SampleDownloads.HasValue Then
                    result.DocumentationDownloads += 1
                Else
                    result.SampleDownloads = 1
                End If

                db.SaveChanges()

                Dim documentation() As Byte = result.Documentation
                context.Response.ContentType = "application/octet-stream"
                context.Response.AddHeader("Content-disposition", String.Format("attachment; filename={0}", result.DocumentationFilename))
                context.Response.AddHeader("Content-Length", documentation.Length)
                context.Response.BinaryWrite(documentation)
                context.Response.End()
            End If
        Else
            context.Response.Write(String.Format("The CurrentExecutionFilePath ('{0}') doesn't appear to be valid.", context.Request.CurrentExecutionFilePath))
        End If

    End Sub

#End Region

End Class
