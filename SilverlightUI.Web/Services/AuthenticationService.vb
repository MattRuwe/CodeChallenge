Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Imports System.Web.Security

Namespace Web

    ''' <summary>
    ''' RIA Services DomainService responsible for authenticating users when
    ''' they try to log on to the application.
    ''' 
    ''' Most of the functionality is already provided by the base class
    ''' AuthenticationBase
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)

        Protected Overrides Function GetAuthenticatedUser(ByVal principal As System.Security.Principal.IPrincipal) As User
            Dim returnValue As User = MyBase.GetAuthenticatedUser(principal)
            Dim memUser As MembershipUser = Membership.GetUser(principal.Identity.Name)
            returnValue.UserID = memUser.ProviderUserKey

            Return returnValue
        End Function
    End Class
End Namespace