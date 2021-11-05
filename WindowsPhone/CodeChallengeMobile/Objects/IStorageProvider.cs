using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmahaMTG.CodeChallengeMobile.Objects
{
    public interface IStorageProvider
    {
        bool Contains(string key);
        object GetData(string key);
        void SetData(string key, object value);
        void Remove(string key);
    }
}
