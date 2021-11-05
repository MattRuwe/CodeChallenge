using Microsoft.Phone.Controls;
using System;

namespace OmahaMTG.CodeChallengeMobile.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //ServiceReference1.CodeChallengeDomainServicesoapClient client = new ServiceReference1.CodeChallengeDomainServicesoapClient();

            //client.GetLeaderBoardResultsCompleted += new EventHandler<ServiceReference1.GetLeaderBoardResultsCompletedEventArgs>(client_GetLeaderBoardResultsCompleted);
            //client.GetLeaderBoardResultsAsync(10);
        }

        private void btnSettings_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }

        private void btnLeaderboard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/LeaderboardView.xaml", UriKind.Relative));
        }

        //void client_GetLeaderBoardResultsCompleted(object sender, ServiceReference1.GetLeaderBoardResultsCompletedEventArgs e)
        //{
            
        //}
    }
}