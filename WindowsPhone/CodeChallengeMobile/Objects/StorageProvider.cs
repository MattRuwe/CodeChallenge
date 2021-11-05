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
using System.IO.IsolatedStorage;

namespace OmahaMTG.CodeChallengeMobile.Objects
{
    public class StorageProvider : IStorageProvider
    {
        public StorageProvider()
        {

        }

        public bool Contains(string key)
        {
            if (!DesignerProperties.IsInDesignTool)
                return IsolatedStorageSettings.ApplicationSettings.Contains(key);
            else
                return false;
        }

        public object GetData(string key)
        {
            if (!DesignerProperties.IsInDesignTool && IsolatedStorageSettings.ApplicationSettings.Contains(key))
                return IsolatedStorageSettings.ApplicationSettings[key];
            else
                return null;
        }

        public void SetData(string key, object value)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                    IsolatedStorageSettings.ApplicationSettings[key] = value;
                else
                    IsolatedStorageSettings.ApplicationSettings.Add(key, value);

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public void Remove(string key)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(key);
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
    }
}
