using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using OmahaMTG.CodeChallengeMobile.CodeChallengeService;
using OmahaMTG.CodeChallengeMobile.AuthenticationService;
using System.Collections.ObjectModel;

namespace OmahaMTG.CodeChallengeMobile.Views
{
    public partial class LeaderboardView : PhoneApplicationPage
    {
        private CodeChallengeDomainServicesoapClient _client;
        private AuthenticationServicesoapClient _authClient;
        private Dictionary<int, ObservableCollection<ResultsListing>> _leaderboard;
        private int _currentChallengeId;

        public LeaderboardView()
        {
            InitializeComponent();

            _leaderboard = new Dictionary<int, ObservableCollection<ResultsListing>>();

            //http://blogs.msdn.com/b/deepm/archive/2010/03/17/configuring-your-domainservice-for-a-windows-phone-application.aspx
            _client = new CodeChallengeService.CodeChallengeDomainServicesoapClient();

            //http://dotnet-redzone.blogspot.com/2010/11/windows-phone-7-using-authenticated-ria.html
            _authClient = new AuthenticationServicesoapClient();

            //_authClient.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(_authClient_LoginCompleted);

            //_authClient.LoginAsync("mruwe", "MattMatt", true, string.Empty);

            _client.GetCodeChallengesSecureCompleted += new EventHandler<CodeChallengeService.GetCodeChallengesSecureCompletedEventArgs>(client_GetCodeChallengesSecureCompleted);
            _client.GetLeaderBoardResultsCompleted += new EventHandler<CodeChallengeService.GetLeaderBoardResultsCompletedEventArgs>(client_GetLeaderBoardResultsCompleted);

            _client.GetCodeChallengesSecureAsync(false);


        }

        //void _authClient_LoginCompleted(object sender, LoginCompletedEventArgs e)
        //{

        //}

        void client_GetCodeChallengesSecureCompleted(object sender, CodeChallengeService.GetCodeChallengesSecureCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                pivLeaderboard.ItemsSource = e.Result.RootResults;
                _currentChallengeId = e.Result.RootResults[0].id;
                RetrieveLeaderboardResults(_currentChallengeId);
            }
            else
            {
                MessageBox.Show("Could not connect to Code Challenge Service.  Please try again later.");
            }
        }

        void client_GetLeaderBoardResultsCompleted(object sender, CodeChallengeService.GetLeaderBoardResultsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (_leaderboard.ContainsKey((int)e.UserState))
                {
                    _leaderboard[((int)e.UserState)] = e.Result.RootResults;
                }
                else
                {
                    _leaderboard.Add((int)e.UserState, e.Result.RootResults);
                }

                BindLeaderboardToList(_leaderboard[(int)e.UserState]);
            }
            else
            {
                MessageBox.Show("Could not connect to Code Challenge Service.  Please try again later.");
            }

        }

        private void BindLeaderboardToList(ObservableCollection<ResultsListing> results)
        {
            PivotItem pivotItem = (PivotItem)pivLeaderboard.ItemContainerGenerator.ContainerFromIndex(pivLeaderboard.SelectedIndex);

            ItemsControl ic = GetChild<ItemsControl>(pivotItem);
            ic.ItemsSource = results;
        }

        private void RetrieveLeaderboardResults(int challengeID)
        {
            if (_leaderboard.ContainsKey(challengeID))
            {
                BindLeaderboardToList(_leaderboard[challengeID]);
            }
            else
            {
                _client.GetLeaderBoardResultsAsync(challengeID, challengeID);
            }
        }

        private T GetChild<T>(DependencyObject obj) where T : DependencyObject
        {
            DependencyObject child = null;
            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child.GetType() == typeof(T))
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetChild<T>(child);
                    if (child != null && child.GetType() == typeof(T))
                    {
                        break;
                    }
                }
            }
            return child as T;
        }

        private void pivLeaderboard_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            _currentChallengeId = ((CodeChallengeListing)pivLeaderboard.SelectedItem).id;
            RetrieveLeaderboardResults(_currentChallengeId);
        }

        private void pivLeaderboard_Loaded(object sender, RoutedEventArgs e)
        {
            //_client.GetLeaderBoardResultsAsync(((CodeChallengeListing)pivLeaderboard.SelectedItem).id);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _leaderboard.Clear();
            RetrieveLeaderboardResults(_currentChallengeId);
        }

    }
}