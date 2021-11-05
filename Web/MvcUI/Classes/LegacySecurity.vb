'Imports System.Data.Common.CommandTrees.ExpressionBuilder
'Imports System.Linq
'Imports System.Security.Cryptography

'Public Class LegacySecurity
' ''' <summary>
'    ''' The user's profile record.
'    ''' </summary>
'    Private userProfile As UserProfile

'    ''' <summary>
'    ''' The users membership record.
'    ''' </summary>
'    Private membership As webpages_Membership

'    ''' <summary>
'    ''' The clear text password.
'    ''' </summary>
'    Private clearPassword As String

'    ''' <summary>
'    ''' The password after it has been hashed using SHA1.
'    ''' </summary>
'    Private sha1HashedPassword As String

'    ''' <summary>
'    ''' The user's user name.
'    ''' </summary>
'    Private userName As String

'    ''' <summary>
'    ''' Inidcates if the authentication token in the cookie should be persisted beyond the current session.
'    ''' </summary>
'    Private persistCookie As Boolean

'    ''' <summary>
'    ''' Validates the user against legacy values.
'    ''' </summary>
'    ''' <param name="userName">The user's UserName.</param>
'    ''' <param name="password">The user's password.</param>
'    ''' <param name="persistCookie">Inidcates if the authentication token in the cookie should be persisted beyond the current session.</param>
'    ''' <returns>true if the user is validated and logged in, otherwise false.</returns>
'    Public Function Login(userName As String, password As String, Optional persistCookie As Boolean = False) As Boolean
'        Me.userName = userName
'        Me.clearPassword = password
'        Me.persistCookie = persistCookie

'        If Not GetOriginalValues() Then
'            Return False
'        End If

'        SetHashedPassword()

'        If Me.sha1HashedPassword <> Me.membership.Password Then
'            Return False
'        End If

'        Me.SetPasswordAndLoginUser()

'        Return True
'    End Function

'    ''' <summary>
'    ''' Gets the original password values
'    ''' </summary>
'    Protected Function GetOriginalValues() As Boolean
'        Using context = New Models.UsersContext()
'            Me.userProfile = context.UserProfiles.Where(Function(x) x.UserName.ToLower() = userName.ToLower()).SingleOrDefault()

'            If Me.userProfile Is Nothing Then
'                Return False
'            End If

'            Me.membership = context.Memberships.Where(Function(x) x.UserId = Me.userProfile.UserId).SingleOrDefault()

'            If Me.membership Is Nothing Then
'                Return False
'            End If

'            If Not Me.membership.IsConfirmed Then
'                Return False
'            End If
'        End Using

'        Return True
'    End Function

'    ''' <summary>
'    ''' Encrypts the password using the SHA1 algorithm.
'    ''' </summary>
'    ''' <remarks>
'    ''' Many thanks to Malcolm Swaine for the hashing code.
'    ''' http://www.codeproject.com/Articles/32600/Manually-validating-an-ASP-NET-user-account-with-a
'    ''' </remarks>
'    Protected Sub SetHashedPassword()
'        Dim bIn As Byte() = Encoding.Unicode.GetBytes(clearPassword)
'        Dim bSalt As Byte() = Convert.FromBase64String(membership.PasswordSalt)
'        Dim bAll As Byte() = New Byte(bSalt.Length + (bIn.Length - 1)) {}
'        Dim bRet As Byte() = Nothing
'        Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length)
'        Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length)

'        Dim s As HashAlgorithm = HashAlgorithm.Create("SHA1")
'        bRet = s.ComputeHash(bAll)
'        Dim newHash As String = Convert.ToBase64String(bRet)
'        Me.sha1HashedPassword = newHash
'    End Sub

'    ''' <summary>
'    ''' Sets the password using the new algorithm and perofrms a login.
'    ''' </summary>
'    Protected Sub SetPasswordAndLoginUser()
'        Dim token = WebMatrix.WebData.WebSecurity.GeneratePasswordResetToken(Me.userName, 2)
'        WebMatrix.WebData.WebSecurity.ResetPassword(token, clearPassword)
'        WebMatrix.WebData.WebSecurity.Login(userName, clearPassword, persistCookie)
'    End Sub
'End Class
