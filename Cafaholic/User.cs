using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafaholic
{
    public class User: INotifyPropertyChanged
    {
        public string Id { get; set; }

        private string _uname;
        [JsonProperty(PropertyName = "UserName")]
        public string Username
        {
            get
            {
                return _uname;
            }
            set
            {
                if (value != _uname)
                {
                    _uname = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }

        private byte[] _pass;
        [JsonProperty(PropertyName = "Password")]
        public byte[] Password
        {
            get
            {
                return _pass;
            }
            set
            {
                if (value != _pass)
                {
                    _pass = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        private string _email;
        [JsonProperty(PropertyName = "Email")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
