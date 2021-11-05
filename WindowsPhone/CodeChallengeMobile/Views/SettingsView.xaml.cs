using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using OmahaMTG.CodeChallengeMobile.Objects;
using System.Windows.Data;
using System.Windows.Controls;

namespace OmahaMTG.CodeChallengeMobile.Views
{
    public partial class SettingsView : PhoneApplicationPage
    {
        private IStorageProvider _storageProvider;

        public SettingsView()
        {
            InitializeComponent();

            _storageProvider = Application.Current.Resources["StorageProvider"] as IStorageProvider;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            txtUsername.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtPassword.GetBindingExpression(PasswordBox.PasswordProperty).UpdateSource();


            NavigationService.GoBack();
        }
    }
}