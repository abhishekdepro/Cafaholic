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
using Microsoft.Phone.Tasks;

namespace Cafaholic
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask e1 = new EmailComposeTask();
            e1.To = "abhishekde@hotmail.com";
            e1.Cc = "surrealbelongings@outlook.com";
            e1.Subject = "Feedback on Cafaholic 1.0.0";
            e1.Body = "Device Name: " + Microsoft.Phone.Info.DeviceStatus.DeviceName;
            e1.Show();
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask browse = new WebBrowserTask();
            Uri uri = new Uri("https://twitter.com/abhishekdepro", UriKind.Absolute);
            browse.Uri = uri;
            browse.Show();
        }
    }
}
