Imports System.Xml
Imports System.Text

Public Class AnnouncementsRss
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Response.Clear()
            Response.ContentType = "text/xml"

            Dim codeChallengeUrl As String = Request.Url.GetComponents(System.UriComponents.SchemeAndServer, System.UriFormat.UriEscaped) & Request.ApplicationPath.TrimEnd("/")


            Dim xmlRssFeed As New XmlTextWriter(Response.OutputStream, Encoding.UTF8)
            xmlRssFeed.Formatting = Formatting.Indented
            ' Start writing the rss tags
            xmlRssFeed.WriteStartDocument()
            xmlRssFeed.WriteStartElement("rss")
            xmlRssFeed.WriteAttributeString("version", "2.0")
            xmlRssFeed.WriteStartElement("channel")
            xmlRssFeed.WriteElementString("title", "OmahaMTG Code Challenge Announcements")
            xmlRssFeed.WriteElementString("link", codeChallengeUrl)
            xmlRssFeed.WriteElementString("description", "Announcements for the OmahaMTG code challenge")
            xmlRssFeed.WriteElementString("copyright", "Copyright " & Date.Now.Year)

            Dim db As New CodeChallengeModel()

            Dim announcements = From a In db.CodeChallenge_Announcement
                                Order By a.PostingDate Descending

            For Each announcement As CodeChallenge_Announcement In announcements
                xmlRssFeed.WriteStartElement("item")
                xmlRssFeed.WriteElementString("guid", codeChallengeUrl & "/" & announcement.id)
                xmlRssFeed.WriteElementString("title", announcement.Title)
                xmlRssFeed.WriteStartElement("description")
                xmlRssFeed.WriteCData(announcement.AnnouncementHtml)
                xmlRssFeed.WriteEndElement()
                xmlRssFeed.WriteElementString("link", codeChallengeUrl)
                'Tue, 03 Jun 2003 09:39:21 GMT
                xmlRssFeed.WriteElementString("pubDate", announcement.PostingDate.ToUniversalTime.ToString("ddd, dd MMM yyyy HH:mm:ss G\MT"))
                xmlRssFeed.WriteEndElement()
            Next

            ' Close all tags
            xmlRssFeed.WriteEndElement()
            xmlRssFeed.WriteEndElement()
            xmlRssFeed.WriteEndDocument()
            xmlRssFeed.Flush()
            xmlRssFeed.Close()
            Response.End()


        End If
    End Sub

End Class