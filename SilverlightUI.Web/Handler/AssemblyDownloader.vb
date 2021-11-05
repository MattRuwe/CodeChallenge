Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class AssemblyDownloader
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

        '"/OmahaMTG.Challenge.RubiksCubeChallenge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b86d58ba93c337a0.devassembly"

        Dim match As Match = Regex.Match(context.Request.CurrentExecutionFilePath, "(?i)(?<=/)[^/]+(?=.devassembly$)")

        If match.Success Then
            Dim db As New CodeChallengeModel()
            Dim developerAssembly As CodeChallenge_DeveloperAssembly
            Dim name As AssemblyName


            Dim assemblyId As Integer
            If Integer.TryParse(match.Value, assemblyId) Then
                developerAssembly = (From da In db.CodeChallenge_DeveloperAssembly Where da.id = assemblyId).FirstOrDefault()
                name = New AssemblyName(developerAssembly.assembly_fullname)
            Else
                name = New AssemblyName(context.Server.UrlDecode(match.Value))
                developerAssembly = (From da In db.CodeChallenge_DeveloperAssembly Where da.assembly_fullname = name.FullName).FirstOrDefault()
            End If

            If developerAssembly IsNot Nothing Then
                If developerAssembly.NumberOfDownloads.HasValue Then
                    developerAssembly.NumberOfDownloads += 1
                Else
                    developerAssembly.NumberOfDownloads = 1
                End If
                db.SaveChanges()

                Dim assemblyBytes() As Byte = developerAssembly.assembly
                context.Response.ContentType = "application/octet-stream"
                context.Response.AddHeader("Content-disposition", String.Format("attachment; filename={0}.dll", name.Name))
                context.Response.AddHeader("Content-Length", assemblyBytes.Length)
                context.Response.BinaryWrite(assemblyBytes)
                context.Response.End()
            Else
                context.Response.Write(String.Format("The assembly ('{0}') parsed from the url ('{1}') could not be found.  Please contact the administrator.", name.FullName, context.Request.CurrentExecutionFilePath))
            End If
        Else
            context.Response.Write(String.Format("The CurrentExecutionFilePath ('{0}') doesn't appear to be valid.", context.Request.CurrentExecutionFilePath))
        End If


    End Sub

#End Region

End Class
