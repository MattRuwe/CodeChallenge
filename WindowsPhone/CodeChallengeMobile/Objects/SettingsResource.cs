using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace OmahaMTG.CodeChallengeMobile.Objects
{
    public class SettingsResource : INotifyPropertyChanged
    {

        private IStorageProvider _storageProvider;

        public SettingsResource()
        {
            
        }

        private void EnsureStorageProvider()
        {
            if (_storageProvider == null)
            {
                _storageProvider = Application.Current.Resources["StorageProvider"] as IStorageProvider;
            }
        }

        public string Username
        {
            get
            {
                EnsureStorageProvider();
                return _storageProvider.GetData("Username") as string;
            }
            set
            {
                EnsureStorageProvider();
                if (_storageProvider.GetData("Username") != value)
                {
                    _storageProvider.SetData("Username", value);
                    PropertyChanged(this, new PropertyChangedEventArgs("Username"));
                }
                
            }
        }

        public string Password
        {
            get
            {
                EnsureStorageProvider();
                return _storageProvider.GetData("Password") as string;
            }
            set
            {
                EnsureStorageProvider();
                if (_storageProvider.GetData("Password") != value)
                {
                    _storageProvider.SetData("Password", value);
                    PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
