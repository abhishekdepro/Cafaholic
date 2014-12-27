using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using Microsoft.WindowsAzure.MobileServices;
using System.Text;
using System.Windows.Media;

namespace Cafaholic
{
    public partial class Login : PhoneApplicationPage
    {
        public static IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        private IMobileServiceTable<User> Users = App.MobileService.GetTable<User>();
        public Login()
        {
            InitializeComponent();
        }

        private async void redirect()
        {
            var fb = new FacebookClient();
            fb.AccessToken = ParseFacebookUtils.AccessToken;
            var x = fb.AccessToken;
            var me = await fb.GetTaskAsync("me");
            string fb_response = me.ToString();

            var json = JsonConvert.DeserializeObject(fb_response) as JObject;
            var uname = json["name"];
            var name = json["first_name"];

            user_tb.Text = uname.ToString();
            user.Visibility = Visibility.Collapsed;
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            user.Visibility = Visibility.Collapsed;
        }

        private void pass_tb_GotFocus(object sender, RoutedEventArgs e)
        {
            password.Visibility = Visibility.Collapsed;
        }

        private void user_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (user_tb.Text == "")
            {
                user.Visibility = Visibility.Visible;
            }
        }

        private void pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (pass.Password == "")
            {
                password.Visibility = Visibility.Visible;
            }
        }
        public MobileServiceCollection<User, User> users { get; private set; }
        private async void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {


            try
            {

                IMobileServiceTableQuery<User> query = Users
              .Where(todoItem => todoItem.Username == user_tb.Text);

                users = await query.ToCollectionAsync();
                if (users.Count == 1)
                {
                    byte[] p = users[0].Password;
                    string x = Cipher.Decrypt(p);
                    
                    if (x == pass.Password)
                    {
                        appSettings["user"] = user_tb.Text;
                        Login.appSettings["count"] = "0";
                        this.NavigationService.Navigate(new Uri("/Profile.xaml", UriKind.Relative));
                    }

                    else
                    {
                        MessageBox.Show("Error! Try again!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! Try again!");
            }

            
        }

       

        private void check()
        {
            ParseFacebookUtils.BeginLogIn(new[] { "email" });
        }

        private void email_tb_GotFocus(object sender, RoutedEventArgs e)
        {
            email.Visibility = Visibility.Collapsed;
        }

        private void email_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (email_tb.Text == "")
            {
                email.Visibility = Visibility.Visible;
            }
        }

        private async void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            byte[] result = Cipher.Encrypt(pass.Password);
            if (user_tb.Text != "" && pass.Password != "" && email_tb.Text != "")
            {
                if (email_tb.Text.Contains("@"))
                {
                    // From byte array to string
                    User newuser = new User { Username = user_tb.Text, Password = result, Email = email_tb.Text };
                    try
                    {
                        IMobileServiceTableQuery<User> query = Users
                      .Where(todoItem => todoItem.Username == user_tb.Text);

                        users = await query.ToCollectionAsync();
                        if (users.Count == 1)
                        {
                            MessageBox.Show("Pick another username!");
                        }
                        else
                        {
                            await Users.InsertAsync(newuser);
                            if (Login.appSettings.Contains("password"))
                            {
                                Login.appSettings["password"] = pass.Password;
                            }
                            else
                            {
                                Login.appSettings.Add("password", pass.Password);
                            }
                            MessageBox.Show("You have successfuly signed up!", "Signup Success", MessageBoxButton.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error! Try again!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid email id!");
                }
            }
            else
            {
                if (user_tb.Text == "")
                {
                    SolidColorBrush sb = new SolidColorBrush(Colors.Red);
                    user_tb.BorderBrush = sb;
                    user_tb.Focus();
                    
                }
                else if (pass.Password == "")
                {
                    SolidColorBrush sb = new SolidColorBrush(Colors.Red);
                    pass.BorderBrush = sb;
                    pass.Focus();
                }
                else
                {
                    SolidColorBrush sb = new SolidColorBrush(Colors.Red);
                    email_tb.BorderBrush = sb;
                    email_tb.Focus();
                }
            }
        }

        

        
    }
}