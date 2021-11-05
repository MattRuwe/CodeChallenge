Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.CodeChallengeService
Imports OmahaMTG.Challenge.VsExtensibility.ToolWindowUI.ViewModels

Namespace ViewModels
    Public Class LeaderboardViewModel
        Inherits ViewModelBase

        Public Sub New()
            AddHandler ViewModelBase.LoggedInChanged, AddressOf ViewModelBase_LoggedInChanged
        End Sub
  
        Private Sub ViewModelBase_LoggedInChanged(sender As Object, e As EventArgs)

        End Sub

        Private _leaderboardResults As ResultsListing()
        Public Property LeaderboardResults() As ResultsListing()
            Get
                Return _leaderboardResults
            End Get
            Set(ByVal value As ResultsListing())
                _leaderboardResults = value
                OnPropertyChanged("LeaderboardResults")
            End Set
        End Property

        Private _selectedChallenge As CodeChallengeListing
        Public Property SelectedChallenge() As CodeChallengeListing
            Get
                Return _selectedChallenge
            End Get
            Set(ByVal value As CodeChallengeListing)
                _selectedChallenge = value
                OnPropertyChanged("SelectedChallenge")
                LeaderboardResults = ExecuteChallengeServiceCall(Of ResultsListing())(Function(params As Object())
                                                                                          If value IsNot Nothing Then
                                                                                              Return ChallengeClient.GetLeaderBoardResults(value.id).RootResults
                                                                                          End If
                                                                                      End Function, New Object() {})
            End Set
        End Property

    End Class
End Namespace