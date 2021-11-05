
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports SilverlightUI
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Objects.DataClasses
Imports System.Linq
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server


<MetadataType(GetType(CodeChallenge_Announcement.CodeChallenge_AnnouncementMetaData))>
Partial Public Class CodeChallenge_Announcement

    Friend NotInheritable Class CodeChallenge_AnnouncementMetaData
        Private Sub New()

        End Sub

        Public Property id() As Int32

        Public Property Title() As String

        Public Property PostingDate() As DateTime

        Public Property AnnouncementHtml() As Global.System.String
    End Class
End Class

<MetadataType(GetType(CodeChallenge_Sponsor.CodeChallenge_SponsorMetaData))>
Partial Public Class CodeChallenge_Sponsor

    Friend NotInheritable Class CodeChallenge_SponsorMetaData
        Private Sub New()

        End Sub

        Public Property id As Integer

        Public Property Name As String

        Public Property Description As String

        <Include()>
        Public Property CodeChallenges() As EntityCollection(Of CodeChallenge)
    End Class
End Class

'The MetadataTypeAttribute identifies CodeChallengeMetadata as the class
' that carries additional metadata for the CodeChallenge class.
<MetadataTypeAttribute(GetType(CodeChallenge.CodeChallengeMetadata))>  _
Partial Public Class CodeChallenge
    
    'This class allows you to attach custom attributes to properties
    ' of the CodeChallenge class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class CodeChallengeMetadata
        
        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New
        End Sub
        
        Public Property ChallengeName As String

        <Include()>
        Public Property CodeChallenge_DeveloperAssembly As EntityCollection(Of CodeChallenge_DeveloperAssembly)
        
        Public Property CodeChallenge_Entry As EntityCollection(Of CodeChallenge_Entry)
        
        Public Property EndDate As DateTime
        
        Public Property ExecutorAssembly() As Byte
        
        Public Property id As Integer
        
        Public Property Instructions As String
        
        Public Property MaximumMemoryUsageBytes As Long
        
        Public Property MaximumRunningSeconds As Integer
        
        Public Property StartDate As DateTime

        <Include()>
        Public Property CodeChallenge_Sponsor() As CodeChallenge_Sponsor

        Public Property CodeChallenge_Sponsor_id() As Nullable(Of Global.System.Int32)
    End Class
End Class

'The MetadataTypeAttribute identifies CodeChallenge_AssemblyMetadata as the class
' that carries additional metadata for the CodeChallenge_Assembly class.
<MetadataTypeAttribute(GetType(CodeChallenge_Assembly.CodeChallenge_AssemblyMetadata))> _
Partial Public Class CodeChallenge_Assembly

    'This class allows you to attach custom attributes to properties
    ' of the CodeChallenge_Assembly class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class CodeChallenge_AssemblyMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property [assembly]() As Byte

        Public Property AssemblyFullName As String

        Public Property id As Integer

        Public Property Trusted As Boolean
    End Class
End Class

'The MetadataTypeAttribute identifies CodeChallenge_DeveloperAssemblyMetadata as the class
' that carries additional metadata for the CodeChallenge_DeveloperAssembly class.
<MetadataTypeAttribute(GetType(CodeChallenge_DeveloperAssembly.CodeChallenge_DeveloperAssemblyMetadata))>  _
Partial Public Class CodeChallenge_DeveloperAssembly
    
    'This class allows you to attach custom attributes to properties
    ' of the CodeChallenge_DeveloperAssembly class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class CodeChallenge_DeveloperAssemblyMetadata
        
        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New
        End Sub
        
        Public Property [assembly]() As Byte
        
        Public Property assembly_fullname As String
        
        Public Property CodeChallenge As CodeChallenge
        
        Public Property codechallenge_id As Integer
        
        Public Property id As Integer
    End Class
End Class

'The MetadataTypeAttribute identifies CodeChallenge_EntryMetadata as the class
' that carries additional metadata for the CodeChallenge_Entry class.
<MetadataTypeAttribute(GetType(CodeChallenge_Entry.CodeChallenge_EntryMetadata))>  _
Partial Public Class CodeChallenge_Entry
    
    'This class allows you to attach custom attributes to properties
    ' of the CodeChallenge_Entry class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class CodeChallenge_EntryMetadata
        
        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New
        End Sub
        
        Public Property AssemblyFullName As String
        
        Public Property AuthorUserId As Guid

        <Include()>
        Public Property CodeChallenge As CodeChallenge

        <Include()>
        Public Property vw_codechallenge_secure As vw_codechallenge_secure

        <Include()>
        Public Property CodeChallenge_Entry_Result As EntityCollection(Of CodeChallenge_Entry_Result)
        
        Public Property CodeChallenge_Id As Integer
        
        Public Property DateAdded As DateTime
        
        Public Property DateRan As Nullable(Of DateTime)
        
        Public Property FinalScore As Nullable(Of Integer)
        
        Public Property id As Integer
        
        Public Property Submission() As Byte
        
        Public Property TotalExecutionTime As Nullable(Of Integer)
        
        Public Property TypeName As String

        <Include()>
        Public Property aspnet_Users As aspnet_Users
    End Class
End Class

'The MetadataTypeAttribute identifies CodeChallenge_Entry_ResultMetadata as the class
' that carries additional metadata for the CodeChallenge_Entry_Result class.
<MetadataTypeAttribute(GetType(CodeChallenge_Entry_Result.CodeChallenge_Entry_ResultMetadata))>  _
Partial Public Class CodeChallenge_Entry_Result
    
    'This class allows you to attach custom attributes to properties
    ' of the CodeChallenge_Entry_Result class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class CodeChallenge_Entry_ResultMetadata
        
        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New
        End Sub
        
        Public Property author_note As String
        
        Public Property CodeChallenge_Entry As CodeChallenge_Entry
        
        Public Property codechallenge_entry_id As Integer
        
        Public Property duration As Integer
        
        Public Property [error] As String
        
        Public Property id As Integer
        
        Public Property result_message As String
        
        Public Property score As Integer
        
        Public Property successful As Boolean

        Public Property cpu_cycles() As Nullable(Of Global.System.Decimal)
    End Class
End Class

'The MetadataTypeAttribute identifies aspnet_MembershipMetadata as the class
' that carries additional metadata for the aspnet_Membership class.
<MetadataTypeAttribute(GetType(aspnet_Membership.aspnet_MembershipMetadata))> _
Partial Public Class aspnet_Membership

    'This class allows you to attach custom attributes to properties
    ' of the aspnet_Membership class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class aspnet_MembershipMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property ApplicationId As Guid

        Public Property aspnet_Users As aspnet_Users

        Public Property Comment As String

        Public Property CreateDate As DateTime

        Public Property Email As String

        Public Property FailedPasswordAnswerAttemptCount As Integer

        Public Property FailedPasswordAnswerAttemptWindowStart As DateTime

        Public Property FailedPasswordAttemptCount As Integer

        Public Property FailedPasswordAttemptWindowStart As DateTime

        Public Property IsApproved As Boolean

        Public Property IsLockedOut As Boolean

        Public Property LastLockoutDate As DateTime

        Public Property LastLoginDate As DateTime

        Public Property LastPasswordChangedDate As DateTime

        Public Property LoweredEmail As String

        Public Property MobilePIN As String

        Public Property Password As String

        Public Property PasswordAnswer As String

        Public Property PasswordFormat As Integer

        Public Property PasswordQuestion As String

        Public Property PasswordSalt As String

        Public Property UserId As Guid
    End Class
End Class

'The MetadataTypeAttribute identifies aspnet_UsersMetadata as the class
' that carries additional metadata for the aspnet_Users class.
<MetadataTypeAttribute(GetType(aspnet_Users.aspnet_UsersMetadata))> _
Partial Public Class aspnet_Users

    'This class allows you to attach custom attributes to properties
    ' of the aspnet_Users class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class aspnet_UsersMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property ApplicationId As Guid

        Public Property aspnet_Membership As aspnet_Membership

        Public Property CodeChallenge_Entry As EntityCollection(Of CodeChallenge_Entry)

        Public Property IsAnonymous As Boolean

        Public Property LastActivityDate As DateTime

        Public Property LoweredUserName As String

        Public Property MobileAlias As String

        Public Property UserId As Guid

        Public Property UserName As String
    End Class
End Class

'The MetadataTypeAttribute identifies vw_codechallenge_entry_challenge_userMetadata as the class
' that carries additional metadata for the vw_codechallenge_entry_challenge_user class.
<MetadataTypeAttribute(GetType(vw_codechallenge_entry_challenge_user.vw_codechallenge_entry_challenge_userMetadata))> _
Partial Public Class vw_codechallenge_entry_challenge_user

    'This class allows you to attach custom attributes to properties
    ' of the vw_codechallenge_entry_challenge_user class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class vw_codechallenge_entry_challenge_userMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property AssemblyFullName As String

        Public Property AuthorUserId As Guid

        Public Property ChallengeName As String

        Public Property CodeChallenge_Id As Integer

        Public Property DateAdded As DateTime

        Public Property DateRan As Nullable(Of DateTime)

        Public Property EndDate As DateTime

        Public Property ExecutionCommonAssemblyFullName As String

        Public Property ExecutorAssemblyFullName As String

        Public Property Expr1 As Integer

        Public Property FinalScore As Nullable(Of Integer)

        Public Property id As Integer

        Public Property Instructions As String

        Public Property MaximumMemoryUsageBytes As Decimal

        Public Property MaximumRunningSeconds As Integer

        Public Property StartDate As DateTime

        Public Property TotalExecutionTime As Nullable(Of Integer)

        Public Property TypeName As String

        Public Property UserId As Guid

        Public Property UserName As String
    End Class
End Class

'The MetadataTypeAttribute identifies vw_codechallenge_secureMetadata as the class
' that carries additional metadata for the vw_codechallenge_secure class.
<MetadataTypeAttribute(GetType(vw_codechallenge_secure.vw_codechallenge_secureMetadata))> _
Partial Public Class vw_codechallenge_secure

    'This class allows you to attach custom attributes to properties
    ' of the vw_codechallenge_secure class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class vw_codechallenge_secureMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property ChallengeName As String

        <Include()>
        Public Property CodeChallenge_DeveloperAssembly As EntityCollection(Of CodeChallenge_DeveloperAssembly)

        <Include()>
        Public Property CodeChallenge_Entry As EntityCollection(Of CodeChallenge_Entry)

        Public Property EndDate As DateTime

        Public Property ExecutionCommonAssemblyFullName As String

        Public Property ExecutorAssemblyFullName As String

        Public Property id As Integer

        Public Property Instructions As String

        Public Property MaximumMemoryUsageBytes As Decimal

        Public Property MaximumRunningSeconds As Integer

        Public Property StartDate As DateTime
    End Class
End Class