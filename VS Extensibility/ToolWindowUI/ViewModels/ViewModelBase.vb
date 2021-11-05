Imports System.ComponentModel
Imports EnvDTE80
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.AuthenticationService
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports VSLangProj
Imports Microsoft.Win32

Namespace ViewModels

    Public MustInherit Class ViewModelBase
        Implements INotifyPropertyChanged

        Private Shared _sharedCookie As String = String.Empty

        Protected Property AuthClient As AuthenticationServicesoapClient
        Protected Property ChallengeClient As CodeChallengeDomainServicesoapClient

        Public Shared Event LoggedInChanged As EventHandler
        Private Shared Event ChallengesOnChanged As EventHandler
        Private Shared Event SelectedTabChanged As eventhandler

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Sub New()
            SetupServiceClients()
            If LoginContext.Instance.LoggedInUser Is Nothing Then
                Dim sharedCookie As String = GetSavedCookieValue()
                If Not String.IsNullOrEmpty(sharedCookie) Then
                    _sharedCookie = sharedCookie

                    Using New OperationContextScope(AuthClient.InnerChannel)
                        Dim request As New HttpRequestMessageProperty()
                        request.Headers("Cookie") = _sharedCookie
                        OperationContext.Current.OutgoingMessageProperties(HttpRequestMessageProperty.Name) = request

                        LoginContext.Instance.LoggedInUser = AuthClient.GetUser().RootResults.FirstOrDefault()
                        IsLoggedIn = True
                    End Using



                End If
            Else
                IsLoggedIn = True
            End If

            AddHandler ChallengesOnChanged, AddressOf ChallengesChanged
            AddHandler SelectedTabChanged, AddressOf SelectedTab_Changed
        End Sub

        Protected Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Private Sub SetupServiceClients()
            Dim basicBinding As New BasicHttpBinding

            basicBinding.MaxReceivedMessageSize = Integer.MaxValue
            basicBinding.ReaderQuotas.MaxArrayLength = Integer.MaxValue
            basicBinding.ReaderQuotas.MaxStringContentLength = Integer.MaxValue


            'There was an error deserializing the object of type OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService.QueryResultOfResultsListing. 
            'The maximum string content length quota (8192) has been exceeded while reading XML data. This quota may be increased by changing the MaxStringContentLength 
            'property on the XmlDictionaryReaderQuotas object used when creating the XML reader. Line 102, position 77.'.  Please see InnerException for more details.


            'http://www.omahamtg.com/CodeChallenge/Services/SilverlightUI-CodeChallengeDomainService.svc
            'http://localhost:38033/CodeChallenge/Services/SilverlightUI-CodeChallengeDomainService.svc

            Dim authAddress As New EndpointAddress("http://www.omahamtg.com/CodeChallenge/Services/SilverlightUI-Web-AuthenticationService.svc/soap")
            Dim codeChallengeAddress As New EndpointAddress("http://www.omahamtg.com/CodeChallenge/Services/SilverlightUI-CodeChallengeDomainService.svc/soap")
            AuthClient = New AuthenticationService.AuthenticationServicesoapClient(basicBinding, authAddress)
            ChallengeClient = New CodeChallengeService.CodeChallengeDomainServicesoapClient(basicBinding, codeChallengeAddress)
        End Sub

        Public Function ValidateLogin(username As String, password As String) As Boolean
            Dim returnValue As Boolean = False

            Using New OperationContextScope(_AuthClient.InnerChannel)
                Dim result = _AuthClient.Login(username, password, True, Nothing)
                Dim response As HttpResponseMessageProperty = CType(OperationContext.Current.IncomingMessageProperties(HttpResponseMessageProperty.Name), HttpResponseMessageProperty)
                _sharedCookie = response.Headers("Set-Cookie")

                If result.RootResults IsNot Nothing And result.RootResults.Count > 0 Then
                    LoginContext.Instance.LoggedInUser = result.RootResults.First()
                    returnValue = True
                    SaveCookieValue(_sharedCookie)
                End If
            End Using

            Return returnValue
        End Function

        Public Sub Logoff()
            Using New OperationContextScope(_AuthClient.InnerChannel)
                Dim request As New HttpRequestMessageProperty()
                request.Headers("Cookie") = _sharedCookie
                OperationContext.Current.OutgoingMessageProperties(HttpRequestMessageProperty.Name) = request

                _AuthClient.Logout()
            End Using


            SaveCookieValue(String.Empty)
            IsLoggedIn = False
        End Sub

        Private Sub SaveCookieValue(sharedCookie As String)
            Dim key As RegistryKey = Nothing
            Try
                key = Registry.CurrentUser.CreateSubKey("Software\OmahaMTG\CodeChallenge")
                key.SetValue("LoginCookie", sharedCookie)
            Finally
                If key IsNot Nothing Then
                    key.Close()
                End If
            End Try
        End Sub

        Private Function GetSavedCookieValue() As String
            Dim returnValue As String = String.Empty
            Dim key As RegistryKey = Nothing
            Try
                key = Registry.CurrentUser.CreateSubKey("Software\OmahaMTG\CodeChallenge")
                returnValue = key.GetValue("LoginCookie")
            Finally
                If key IsNot Nothing Then
                    key.Close()
                End If
            End Try

            Return returnValue
        End Function

        Protected Function ExecuteChallengeServiceCall(Of T)(predicate As Func(Of IEnumerable(Of Object), T), params As Object()) As T
            Using New OperationContextScope(ChallengeClient.InnerChannel)
                Dim request As New HttpRequestMessageProperty()
                request.Headers("Cookie") = _sharedCookie
                OperationContext.Current.OutgoingMessageProperties(HttpRequestMessageProperty.Name) = request

                Return predicate.Invoke(params)
            End Using
        End Function

        Protected Sub ExecuteChallengeServiceCall(predicate As Action(Of IEnumerable(Of Object)), params As Object())
            ExecuteChallengeServiceCall(Of Object)(New Func(Of IEnumerable(Of Object), Object)(
                                                   Function(insideParams As IEnumerable(Of Object))
                                                       predicate.Invoke(insideParams)
                                                       Return Nothing
                                                   End Function), params)
        End Sub

        Public Sub RefreshChallenges()
            Challenges = ExecuteChallengeServiceCall(Of CodeChallengeListing())(Function(params As Object())
                                                                                    Return ChallengeClient.GetCodeChallengesSecure(False).RootResults
                                                                                End Function, New Object() {})
        End Sub

        Private Sub ChallengesChanged(sender As Object, e As EventArgs)
            OnPropertyChanged("Challenges")
        End Sub

        Private Shared _challenges As CodeChallengeListing()
        Public Property Challenges As CodeChallengeListing()
            Get
                Return _challenges
            End Get
            Set(value As CodeChallengeListing())
                _challenges = value
                RaiseEvent ChallengesOnChanged(Me, EventArgs.Empty)
            End Set
        End Property


        Private _isLoggedIn As Boolean
        Public Property IsLoggedIn() As Boolean
            Get
                Return _isLoggedIn
            End Get
            Set(ByVal value As Boolean)
                If _isLoggedIn <> value Then
                    _isLoggedIn = value
                    OnPropertyChanged("IsLoggedIn")
                    RaiseEvent LoggedInChanged(Me, EventArgs.Empty)
                    SelectedTab = ActiveTab.Challenges
                End If
            End Set
        End Property

        Private Shared _dte As DTE2
        Public Property DTE() As DTE2
            Get
                Return _dte
            End Get
            Set(ByVal value As DTE2)
                _dte = value
            End Set
        End Property

        Public ReadOnly Property ActiveProjects As List(Of EnvDTE.Project)
            Get
                Dim dteProjects() As Object = CType(DTE.ActiveSolutionProjects, Object())
                Dim returnValue As New List(Of EnvDTE.Project)
                For Each project As Object In dteProjects
                    returnValue.Add(CType(project, EnvDTE.Project))
                Next

                Return returnValue
            End Get
        End Property

        Public ReadOnly Property ActiveVsProjects As List(Of VSProject)
            Get
                Dim envDteProjects As List(Of EnvDTE.Project) = ActiveProjects
                Dim returnValue As New List(Of VSProject)
                For Each project As EnvDTE.Project In envDteProjects
                    returnValue.Add(CType(project.Object, VSProject))
                Next

                Return returnValue
            End Get
        End Property

        Private Sub SelectedTab_Changed(sender As Object, e As EventArgs)
            OnPropertyChanged("SelectedTab")
        End Sub

        Private Shared _selectedTab As ActiveTab
        Public Property SelectedTab() As ActiveTab
            Get
                Return _selectedTab
            End Get
            Set(ByVal value As ActiveTab)
                If value <> _selectedTab Then
                    _selectedTab = value
                    RaiseEvent SelectedTabChanged(Me, EventArgs.Empty)
                End If
            End Set
        End Property



    End Class

End Namespace
