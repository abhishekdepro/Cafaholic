using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook.Client;
using System.Threading.Tasks;

namespace Cafaholic
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        public FacebookLoginPage()
        {
            InitializeComponent();
            this.Loaded += FacebookLoginPage_Loaded;
        }

        private async void FacebookLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }

        }
        private FacebookSession session;
        private async Task Authenticate()
        {
            string message = String.Empty;
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("email");
                App.AccessToken = session.AccessToken;
                App.FacebookId = session.FacebookId;

                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/Profile.xaml?fb=1", UriKind.Relative)));
            }
            catch (InvalidOperationException e)
            {
                message = "Login failed! Exception details: " + e.Message;
                MessageBox.Show(message);
            }
        }
    }
}