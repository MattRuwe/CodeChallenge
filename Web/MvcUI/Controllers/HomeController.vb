Imports MvcArchStarter.Core.Persistence
Imports OmahaMTG.Challenge.DomainModel

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private _unitOfWork As IUnitOfWork

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
    End Sub

    Function Index() As ActionResult
        ViewData("Message") = "Modify this template to jump-start your ASP.NET MVC application."

        Return View()
    End Function

    Function Announcements() As JsonResult
        Threading.Thread.Sleep(2000)
        Dim result = _unitOfWork.RepositoryOf(Of CodeChallenge_Announcement)().FindAll().OrderByDescending(Function(x As CodeChallenge_Announcement) x.PostingDate).ToList()
        Return Json(result, JsonRequestBehavior.AllowGet)
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your app description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
End Class
