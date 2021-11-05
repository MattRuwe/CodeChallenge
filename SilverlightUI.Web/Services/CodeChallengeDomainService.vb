
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports SilverlightUI
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data
Imports System.Linq
Imports System.ServiceModel.DomainServices.EntityFramework
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports OmahaMTG.Challenge.ExecutionCommon
Imports OmahaMTG.Challenge.Manager
Imports System.Configuration
Imports SilverlightUI.Web
Imports System.Data.Objects
Imports System.Text.RegularExpressions
Imports System.Web.Security
Imports System.IO
Imports System.Diagnostics

'Implements application logic using the OmahaMtgEF context.
' TODO: Add your application logic to these methods or in additional methods.
' TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
' Also consider adding roles to restrict access as appropriate.
'<RequiresAuthentication> _
<EnableClientAccess()> _
Public Class CodeChallengeDomainService
    Inherits LinqToEntitiesDomainService(Of CodeChallengeModel)

    Private Function IsCurrentUserAdmin() As Boolean
        Dim mu As MembershipUser = Membership.GetUser()

        Dim userIsAdmin As Boolean = mu IsNot Nothing AndAlso My.User.IsInRole("CodeChallengeAdmin")

        Return userIsAdmin
    End Function

    <Invoke()>
    <RequiresAuthentication()>
    Public Function GetAssemblyFullname(ByVal assemblyBytes() As Byte) As String
        Dim manager As New ChallengeManager(ConfigurationManager.ConnectionStrings("CodeChallengeModelChallengeManager").ConnectionString)

        Return manager.GetAssemblyFullNameWithoutLoading(assemblyBytes)
    End Function

    <Invoke()>
    <RequiresAuthentication()>
    Public Function GetIChallengeImplementations(ByVal assemblyBytes() As Byte) As String
        Dim returnValue As New GetChallengeImplementationResult()

        Try
            Dim manager As New ChallengeManager(ConfigurationManager.ConnectionStrings("CodeChallengeModelChallengeManager").ConnectionString)

            Dim implementations As List(Of ImplementationChallenge) = manager.GetAllChallengeTypeStrings(assemblyBytes)

            If implementations.Count > 0 Then

                'Regex.Match(implementations(0).ChallengeInterfaceTypeString, "(?<type>[^,]+),(?<assembly>.*)")

                returnValue.ImplementationFullname = GetTypeName(implementations(0).ImplementationTypeString)

                Dim challengeInterfaceTypeString = GetAssemblyName(implementations(0).ChallengeInterfaceTypeString)

                Dim challenge = (From cc In ObjectContext.CodeChallenges Where cc.id =
                                (From ccda In Me.ObjectContext.CodeChallenge_DeveloperAssembly
                                Where ccda.assembly_fullname = challengeInterfaceTypeString
                                Select ccda.codechallenge_id).FirstOrDefault).FirstOrDefault

                If challenge IsNot Nothing Then
                    returnValue.ChallengeName = challenge.ChallengeName
                    returnValue.ChallengeID = challenge.id
                End If

            End If
        Catch ex As FileNotFoundException
            If Not IsCurrentUserAdmin() Then
                returnValue.LoadingError = "Your entry does not appear to be valid.  Make sure that you have the latest developer assemblies."
            Else
                returnValue.LoadingError = "A FileNotFoundException error occurred while loading the assembly: " & ex.ToString()
            End If

        Catch ex As Exception
            returnValue.LoadingError = "Could not load challenge due to unknown error."
        End Try
        Return String.Format("{0}||{1}||{2}||{3}", returnValue.ImplementationFullname, returnValue.ChallengeName, returnValue.ChallengeID, returnValue.LoadingError)
    End Function

    Private Function GetTypeName(assemblyQualifiedName As String) As String
        Dim returnValue As String = String.Empty
        Dim match As Match = Regex.Match(assemblyQualifiedName, "(?<type>[^,]+),\s*(?<assembly>.*)")
        If match.Success Then
            returnValue = match.Groups("type").Value
        End If

        Return returnValue
    End Function

    Private Function GetAssemblyName(assemblyQualifiedName As String) As String
        Dim returnValue As String = String.Empty
        Dim match As Match = Regex.Match(assemblyQualifiedName, "(?<type>[^,]+),\s*(?<assembly>.*)")
        If match.Success Then
            returnValue = match.Groups("assembly").Value
        End If

        Return returnValue
    End Function

    <Query()>
    <RequiresAuthentication()>
    Public Function GetResults(ByVal codeChallengeID As Integer) As IEnumerable(Of ResultsListing)
        Dim mu As MembershipUser = Membership.GetUser()

        Dim userIsNotAdmin As Boolean = mu Is Nothing OrElse Not My.User.IsInRole("CodeChallengeAdmin")

        Dim db As New CodeChallengeModel()
        Dim result = From cce In db.CodeChallenge_Entry.Include("CodeChallenge").Include("aspnet_Users")
                     Where cce.CodeChallenge_Id = codeChallengeID
                     Order By cce.DateAdded Descending
                     Select New ResultsListing With {
                         .ID = cce.id,
                         .AssemblyFullName = cce.AssemblyFullName,
                         .CodeChallengeID = cce.CodeChallenge_Id,
                         .CodeChallengeName = cce.CodeChallenge.ChallengeName,
                         .DateAdded = cce.DateAdded,
                         .DateRan = cce.DateRan,
                         .FinalScore = cce.FinalScore,
                         .TotalExecutionTime = cce.TotalExecutionTime,
                         .Username = cce.aspnet_Users.UserName,
                         .UserID = cce.aspnet_Users.UserId,
                         .ExecutionDetails = If(userIsNotAdmin, String.Empty, cce.ExecutionDetails),
                         .IsPublished = cce.IsPublished,
                         .Status = cce.Status,
                         .Results = (From ccer In cce.CodeChallenge_Entry_Result
                                     Select New ResultDetailsListing With
                                     {
                                        .AuthorNote = ccer.author_note,
                                        .Duration = ccer.duration,
                                        .Error = ccer.error,
                                        .ID = ccer.id,
                                        .ResultMessage = ccer.result_message,
                                        .Score = ccer.score,
                                        .Successful = ccer.successful,
                                        .EntryID = cce.id,
                                        .CpuCycles = ccer.cpu_cycles,
                                        .TestDataAvailable = ccer.test_result_data IsNot Nothing
                                     })
                     }



        If userIsNotAdmin Then
            result = result.Where(Function(r) r.UserID = CType(mu.ProviderUserKey, Guid))
        End If

        Return result
    End Function

    Public Function GetLatestEntries(ByVal count As Integer) As IEnumerable(Of EntryListing)
        Dim db As New CodeChallengeModel

        Dim mu As MembershipUser = Membership.GetUser()
        Dim userIsNotAdmin As Boolean = mu Is Nothing OrElse Not My.User.IsInRole("CodeChallengeAdmin")

        Dim result = (From cce In db.CodeChallenge_Entry.Include("CodeChallenge").Include("aspnet_Users")
                     Order By cce.DateAdded Descending
                     Select New EntryListing With {
                         .EntryID = cce.id,
                         .AuthorUsername = cce.aspnet_Users.UserName,
                         .FinalScore = cce.FinalScore,
                         .ChallengeName = cce.CodeChallenge.ChallengeName,
                         .DateAdded = cce.DateAdded,
                         .IsPublished = cce.IsPublished,
                         .IsChallengeHidden = cce.CodeChallenge.IsHidden}
                     )

        If userIsNotAdmin Then
            result = result.Where(Function(entry As EntryListing) entry.IsPublished AndAlso (Not entry.IsChallengeHidden.HasValue OrElse Not entry.IsChallengeHidden.Value))
        End If

        Return result.Take(count)
    End Function

    Public Function GetCodeChallengeSecure() As IQueryable(Of vw_codechallenge_secure)
        Return Me.ObjectContext.vw_codechallenge_secure.Include("CodeChallenge_DeveloperAssembly")
    End Function

    <Query()>
    Public Function GetCodeChallengesSecure(onlyActive As Boolean) As IEnumerable(Of CodeChallengeListing)
        Dim returnValue As IEnumerable(Of CodeChallengeListing)
        If onlyActive Then
            returnValue = From cc In Me.ObjectContext.CodeChallenges.Include("CodeChallenge_Sponsor")
                   Where cc.StartDate < DateTime.Now AndAlso cc.EndDate > DateTime.Now
                   Select New CodeChallengeListing With {
                       .ChallengeName = cc.ChallengeName,
                       .id = cc.id,
                       .SponsorName = cc.CodeChallenge_Sponsor.Name,
                       .IsHidden = cc.IsHidden}
        Else
            returnValue = From cc In Me.ObjectContext.CodeChallenges.Include("CodeChallenge_Sponsor")
                   Select New CodeChallengeListing With {
                       .ChallengeName = cc.ChallengeName,
                       .id = cc.id,
                       .SponsorName = cc.CodeChallenge_Sponsor.Name,
                       .IsHidden = cc.IsHidden}
        End If

        If Not IsCurrentUserAdmin() Then
            returnValue = returnValue.Where(Function(r) Not r.IsHidden.HasValue OrElse Not r.IsHidden.Value)
        End If

        Dim temp = returnValue.ToList()
        Return returnValue
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Function GetCodeChallenges() As IQueryable(Of CodeChallenge)
        Return Me.ObjectContext.CodeChallenges.Include("CodeChallenge_Sponsor")
    End Function

    Public Function GetCodeChallengesWithDeveloperAssemblies() As IQueryable(Of vw_codechallenge_secure)
        If IsCurrentUserAdmin() Then
            Return Me.ObjectContext.vw_codechallenge_secure.Include("CodeChallenge_DeveloperAssembly")
        Else
            Return Me.ObjectContext.vw_codechallenge_secure.Include("CodeChallenge_DeveloperAssembly").Where(Function(c) Not c.IsHidden.HasValue Or (c.IsHidden.HasValue AndAlso Not c.IsHidden.Value))
        End If
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallengesWithDeveloperAssemblies(vwCodeChallenge As vw_codechallenge_secure)
        Dim codeChallenge As CodeChallenge = codeChallenge.CreateCodeChallenge(vwCodeChallenge.id, vwCodeChallenge.ChallengeName, vwCodeChallenge.StartDate, vwCodeChallenge.EndDate, vwCodeChallenge.MaximumRunningSeconds, Convert.ToInt64(vwCodeChallenge.MaximumMemoryUsageBytes), 0)

        If (codeChallenge.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenges.Attach(codeChallenge)
        End If
        Me.ObjectContext.CodeChallenges.DeleteObject(codeChallenge)
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge(ByVal codeChallenge As CodeChallenge)
        If ((codeChallenge.EntityState = EntityState.Detached) _
                    = False) Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenges.AddObject(codeChallenge)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge(ByVal currentCodeChallenge As CodeChallenge)
        Me.ObjectContext.CodeChallenges.AttachAsModified(currentCodeChallenge, Me.ChangeSet.GetOriginal(currentCodeChallenge))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge(ByVal codeChallenge As CodeChallenge)
        If (codeChallenge.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenges.Attach(codeChallenge)
        End If
        Me.ObjectContext.CodeChallenges.DeleteObject(codeChallenge)
    End Sub

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    'To support paging you will need to add ordering to the 'CodeChallenge_Assembly' query.
    Public Function GetCodeChallenge_Assembly() As IQueryable(Of CodeChallenge_Assembly)
        Return Me.ObjectContext.CodeChallenge_Assembly
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge_Assembly(ByVal codeChallenge_Assembly As CodeChallenge_Assembly)
        If ((codeChallenge_Assembly.EntityState = EntityState.Detached) _
                    = False) Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge_Assembly, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenge_Assembly.AddObject(codeChallenge_Assembly)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge_Assembly(ByVal currentCodeChallenge_Assembly As CodeChallenge_Assembly)
        Me.ObjectContext.CodeChallenge_Assembly.AttachAsModified(currentCodeChallenge_Assembly, Me.ChangeSet.GetOriginal(currentCodeChallenge_Assembly))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_Assembly(ByVal codeChallenge_Assembly As CodeChallenge_Assembly)
        If (codeChallenge_Assembly.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenge_Assembly.Attach(codeChallenge_Assembly)
        End If
        Me.ObjectContext.CodeChallenge_Assembly.DeleteObject(codeChallenge_Assembly)
    End Sub


    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    'To support paging you will need to add ordering to the 'CodeChallenge_DeveloperAssembly' query.
    Public Function GetCodeChallenge_DeveloperAssembly() As IQueryable(Of CodeChallenge_DeveloperAssembly)
        Return Me.ObjectContext.CodeChallenge_DeveloperAssembly
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge_DeveloperAssembly(ByVal codeChallenge_DeveloperAssembly As CodeChallenge_DeveloperAssembly)
        If ((codeChallenge_DeveloperAssembly.EntityState = EntityState.Detached) _
                    = False) Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge_DeveloperAssembly, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenge_DeveloperAssembly.AddObject(codeChallenge_DeveloperAssembly)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge_DeveloperAssembly(ByVal currentCodeChallenge_DeveloperAssembly As CodeChallenge_DeveloperAssembly)
        Me.ObjectContext.CodeChallenge_DeveloperAssembly.AttachAsModified(currentCodeChallenge_DeveloperAssembly, Me.ChangeSet.GetOriginal(currentCodeChallenge_DeveloperAssembly))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_DeveloperAssembly(ByVal codeChallenge_DeveloperAssembly As CodeChallenge_DeveloperAssembly)
        If (codeChallenge_DeveloperAssembly.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenge_DeveloperAssembly.Attach(codeChallenge_DeveloperAssembly)
        End If
        Me.ObjectContext.CodeChallenge_DeveloperAssembly.DeleteObject(codeChallenge_DeveloperAssembly)
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub RerunEntry(entryId As Integer)
        ObjectContext.ExecuteStoreCommand("DELETE FROM codechallenge_entry_result WHERE codechallenge_entry_id = {0}", entryId)
        ObjectContext.ExecuteStoreCommand("UPDATE codechallenge_entry SET dateran = null, totalexecutiontime = null, finalscore = null, status = 'Pending' where id = {0}", entryId)
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub PurgeEntry(entryId As Integer)
        ObjectContext.ExecuteStoreCommand("DELETE FROM codechallenge_entry_result WHERE codechallenge_entry_id = {0}", entryId)
        ObjectContext.ExecuteStoreCommand("DELETE FROM codechallenge_entry where id = {0}", entryId)
    End Sub


    <Query()>
    <RequiresAuthentication()>
    Public Function GetEntry_Challenge_User() As IQueryable(Of vw_codechallenge_entry_challenge_user)
        Return Me.ObjectContext.vw_codechallenge_entry_challenge_user
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Function GetCodeChallenge_Entry() As IQueryable(Of CodeChallenge_Entry)
        Return Me.ObjectContext.CodeChallenge_Entry
    End Function

    <RequiresAuthentication()>
    Public Sub InsertCodeChallenge_Entry(ByVal codeChallenge_Entry As CodeChallenge_Entry)
        codeChallenge_Entry.DateAdded = DateTime.UtcNow
        codeChallenge_Entry.Status = "Pending"

        If codeChallenge_Entry.IsPublished Then
            codeChallenge_Entry.IsTest = False
        End If

        Dim codeChallenge = (From db In ObjectContext.CodeChallenges()
                            Where db.id = codeChallenge_Entry.CodeChallenge_Id).FirstOrDefault()

        If codeChallenge IsNot Nothing Then
            codeChallenge.TotalEntries += 1
            ObjectContext.SaveChanges()

            codeChallenge_Entry.AuthorUserId = CType(System.Web.Security.Membership.GetUser(ServiceContext.User.Identity.Name).ProviderUserKey, Guid)
            If ((codeChallenge_Entry.EntityState = EntityState.Detached) = False) Then
                Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge_Entry, EntityState.Added)
            Else
                Me.ObjectContext.CodeChallenge_Entry.AddObject(codeChallenge_Entry)
            End If
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge_Entry(ByVal currentCodeChallenge_Entry As CodeChallenge_Entry)
        Me.ObjectContext.CodeChallenge_Entry.AttachAsModified(currentCodeChallenge_Entry, Me.ChangeSet.GetOriginal(currentCodeChallenge_Entry))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_Entry(ByVal codeChallenge_Entry As CodeChallenge_Entry)
        If (codeChallenge_Entry.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenge_Entry.Attach(codeChallenge_Entry)
        End If
        Me.ObjectContext.CodeChallenge_Entry.DeleteObject(codeChallenge_Entry)
    End Sub

    Public Function GetLeaderBoardResults(challengeID As Integer) As IEnumerable(Of ResultsListing)
        Dim returnValue As IEnumerable(Of ResultsListing) = Nothing

        Dim i As Integer = 0
        returnValue = (From cce In
                        (From cce2 In Me.ObjectContext.CodeChallenge_Entry
                         Where cce2.IsPublished = True
                        Group By authoruserid = cce2.AuthorUserId, codeChallendID = cce2.CodeChallenge_Id Into groupEntries = Group
                        Select groupEntries.OrderByDescending(Function(c) c.FinalScore).FirstOrDefault)
                        Where cce.CodeChallenge_Id = challengeID
                        Order By cce.FinalScore Ascending, cce.DateAdded Ascending
                        Select New ResultsListing() With
                        {
                            .ID = cce.id,
                            .CodeChallengeID = cce.CodeChallenge_Id,
                            .CodeChallengeName = cce.CodeChallenge.ChallengeName,
                            .DateAdded = cce.DateAdded,
                            .DateRan = cce.DateRan,
                            .FinalScore = cce.FinalScore,
                            .TotalExecutionTime = cce.TotalExecutionTime,
                            .Username = cce.aspnet_Users.UserName,
                            .UserID = cce.aspnet_Users.UserId,
                            .IsPublished = cce.IsPublished,
                            .IsHidden = cce.CodeChallenge.IsHidden
                        }).OrderByDescending(Function(cce) cce.FinalScore).ThenBy(Function(cce) cce.DateAdded)

        returnValue = returnValue.ToList

        'Set the scoring position
        Dim currentChallengeId As Integer = 0
        Dim position As Integer = 0
        For Each result As ResultsListing In returnValue
            If result.CodeChallengeID <> currentChallengeId Then
                position = 0
                currentChallengeId = result.CodeChallengeID
            End If

            position += 1

            result.Position = position
        Next

        If Not IsCurrentUserAdmin() Then
            returnValue = returnValue.Where(Function(result) Not result.IsHidden.HasValue Or (result.IsHidden.HasValue AndAlso Not result.IsHidden.Value))
        End If

        'http://stackoverflow.com/questions/1606454/conditional-orderby-sort-order-in-linq
        'If onlyMyEntries Then
        '    returnValue = returnValue.Where(Function(result) result.UserID = userId)
        'End If

        Return returnValue
    End Function

    Public Function GetCodeChallenge_Entry_Result() As IQueryable(Of CodeChallenge_Entry_Result)
        Return Me.ObjectContext.CodeChallenge_Entry_Result
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge_Entry_Result(ByVal codeChallenge_Entry_Result As CodeChallenge_Entry_Result)
        If ((codeChallenge_Entry_Result.EntityState = EntityState.Detached) _
                    = False) Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge_Entry_Result, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenge_Entry_Result.AddObject(codeChallenge_Entry_Result)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge_Entry_Result(ByVal currentCodeChallenge_Entry_Result As CodeChallenge_Entry_Result)
        Me.ObjectContext.CodeChallenge_Entry_Result.AttachAsModified(currentCodeChallenge_Entry_Result, Me.ChangeSet.GetOriginal(currentCodeChallenge_Entry_Result))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_Entry_Result(ByVal codeChallenge_Entry_Result As CodeChallenge_Entry_Result)
        If (codeChallenge_Entry_Result.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenge_Entry_Result.Attach(codeChallenge_Entry_Result)
        End If
        Me.ObjectContext.CodeChallenge_Entry_Result.DeleteObject(codeChallenge_Entry_Result)
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Function GetCodeChallenge_Sponsor() As IQueryable(Of CodeChallenge_Sponsor)
        Return Me.ObjectContext.CodeChallenge_Sponsor
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge_Sponsor(codeChallenge_Sponsor As CodeChallenge_Sponsor)
        If codeChallenge_Sponsor.EntityState <> EntityState.Detached Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge_Sponsor, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenge_Sponsor.AddObject(codeChallenge_Sponsor)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge_Sponsor(currentCodeChallenge_Sponsor As CodeChallenge_Sponsor)
        Me.ObjectContext.CodeChallenge_Sponsor.AttachAsModified(currentCodeChallenge_Sponsor, Me.ChangeSet.GetOriginal(currentCodeChallenge_Sponsor))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_Sponsor(codeChallenge_Sponsor As CodeChallenge_Sponsor)
        If codeChallenge_Sponsor.EntityState = EntityState.Detached Then
            Me.ObjectContext.CodeChallenge_Sponsor.Attach(codeChallenge_Sponsor)
        End If
        Me.ObjectContext.CodeChallenge_Sponsor.DeleteObject(codeChallenge_Sponsor)
    End Sub

    Public Function GetCodeChallenge_Announcement() As IQueryable(Of CodeChallenge_Announcement)
        Return Me.ObjectContext.CodeChallenge_Announcement
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge_Announcement(codeChallenge_Announcement As CodeChallenge_Announcement)
        codeChallenge_Announcement.PostingDate = DateTime.UtcNow
        If codeChallenge_Announcement.EntityState <> EntityState.Detached Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(codeChallenge_Announcement, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenge_Announcement.AddObject(codeChallenge_Announcement)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdatecodeChallenge_Announcement(currentCodeChallenge_Announcement As CodeChallenge_Announcement)
        Me.ObjectContext.CodeChallenge_Announcement.AttachAsModified(currentCodeChallenge_Announcement, Me.ChangeSet.GetOriginal(currentCodeChallenge_Announcement))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_Announcement(codeChallenge_announcement As CodeChallenge_Announcement)
        If codeChallenge_announcement.EntityState = EntityState.Detached Then
            Me.ObjectContext.CodeChallenge_Announcement.Attach(codeChallenge_announcement)
        End If
        Me.ObjectContext.CodeChallenge_Announcement.DeleteObject(codeChallenge_announcement)
    End Sub

    Public Function GetCodeChallenge_Config() As IQueryable(Of CodeChallenge_Config)
        Return Me.ObjectContext.CodeChallenge_Config
    End Function

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub InsertCodeChallenge_Config(ByVal CodeChallenge_Config As CodeChallenge_Config)
        If ((CodeChallenge_Config.EntityState = EntityState.Detached) = False) Then
            Me.ObjectContext.ObjectStateManager.ChangeObjectState(CodeChallenge_Config, EntityState.Added)
        Else
            Me.ObjectContext.CodeChallenge_Config.AddObject(CodeChallenge_Config)
        End If
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub UpdateCodeChallenge_Config(ByVal currentCodeChallenge_Config As CodeChallenge_Config)
        Me.ObjectContext.CodeChallenge_Config.AttachAsModified(currentCodeChallenge_Config, Me.ChangeSet.GetOriginal(currentCodeChallenge_Config))
    End Sub

    <RequiresRole("CodeChallengeAdmin")>
    Public Sub DeleteCodeChallenge_Config(ByVal CodeChallenge_Config As CodeChallenge_Config)
        If (CodeChallenge_Config.EntityState = EntityState.Detached) Then
            Me.ObjectContext.CodeChallenge_Config.Attach(CodeChallenge_Config)
        End If
        Me.ObjectContext.CodeChallenge_Config.DeleteObject(CodeChallenge_Config)
    End Sub

    <Query()>
    Public Function GetChallengeStats(challengeId As Integer) As IEnumerable(Of ChallengeStatistics)
        Dim returnValue As New List(Of ChallengeStatistics)

        Dim result = From g In (
                    From cce In ObjectContext.CodeChallenge_Entry
                    Join au In ObjectContext.aspnet_Users On au.userid Equals cce.AuthorUserId
                    Join cc In ObjectContext.CodeChallenges On cc.id Equals cce.CodeChallenge_Id
                    Where cce.CodeChallenge_Id = challengeId And cce.IsPublished And cce.FinalScore > 0
                    Order By cce.DateAdded
                    Select cc.ChallengeName, au.Username, cce.FinalScore, cce.DateAdded)
                    Group By g.ChallengeName, g.Username Into Group
                    Select Group



        Dim challengeStat As New ChallengeStatistics

        For Each a In result
            'User
            Dim stat As New UserChallengeStatistics
            a = a.OrderBy(Function(p) p.DateAdded)
            For Each b In a
                'Entry
                If String.IsNullOrWhiteSpace(challengeStat.ChallengeName) Then
                    challengeStat.ChallengeName = b.ChallengeName
                End If
                stat.Username = b.Username
                stat.Scores.Add(New UserScore(stat.ID) With {
                                .DateAdded = b.DateAdded,
                                .Score = b.FinalScore.Value})
            Next
            challengeStat.UserStatistics.Add(stat)
        Next


        challengeStat.PublishedEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And cce.IsPublished Select cce.id).Count()
        challengeStat.UnpublishedEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And Not cce.IsPublished Select cce.id).Count()
        challengeStat.PendingEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And cce.Status = "Pending" Select cce.id).Count()
        challengeStat.QueuedEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And cce.Status = "Queued" Select cce.id).Count()
        challengeStat.InitializingEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And cce.Status = "Initializing" Select cce.id).Count()
        challengeStat.RunningEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And cce.Status = "Running" Select cce.id).Count()
        challengeStat.FinishedEntryCount = (From cce In ObjectContext.CodeChallenge_Entry Where cce.CodeChallenge_Id = challengeId And cce.Status = "Finished" Select cce.id).Count()

        If challengeStat.PublishedEntryCount > 0 OrElse challengeStat.UnpublishedEntryCount > 0 Then
            challengeStat.AverageScore = CType((From cce In ObjectContext.CodeChallenge_Entry Where cce.FinalScore.HasValue And cce.CodeChallenge_Id = challengeId Select cce.FinalScore).Average(Function(entry) entry.Value), Long)
        End If
        If challengeStat.UnpublishedEntryCount > 0 Then
            challengeStat.AverageUnpublishedScore = CType((From cce In ObjectContext.CodeChallenge_Entry Where cce.FinalScore.HasValue And cce.CodeChallenge_Id = challengeId And Not cce.IsPublished Select cce.FinalScore).Average(Function(entry) entry.Value), Long)
        End If
        If challengeStat.PublishedEntryCount > 0 Then
            challengeStat.AveragePublishedScore = CType((From cce In ObjectContext.CodeChallenge_Entry Where cce.FinalScore.HasValue And cce.CodeChallenge_Id = challengeId And cce.IsPublished Select cce.FinalScore).Average(Function(entry) entry.Value), Long)
        End If

        If challengeStat.PublishedEntryCount > 0 OrElse challengeStat.UnpublishedEntryCount > 0 Then
            challengeStat.MaxScore = (From cce In ObjectContext.CodeChallenge_Entry Where cce.FinalScore.HasValue And cce.CodeChallenge_Id = challengeId Select cce.FinalScore).Max(Function(entry) entry.Value)
        End If
        If challengeStat.UnpublishedEntryCount > 0 Then
            challengeStat.MaxUnpublishedScore = (From cce In ObjectContext.CodeChallenge_Entry Where cce.FinalScore.HasValue And cce.CodeChallenge_Id = challengeId And Not cce.IsPublished Select cce.FinalScore).Max(Function(entry) entry.Value)
        End If
        If challengeStat.PublishedEntryCount > 0 Then
            challengeStat.MaxPublishedScore = (From cce In ObjectContext.CodeChallenge_Entry Where cce.FinalScore.HasValue And cce.CodeChallenge_Id = challengeId And cce.IsPublished Select cce.FinalScore).Max(Function(entry) entry.Value)
        End If


        returnValue.Add(challengeStat)

        Return returnValue
    End Function
End Class

