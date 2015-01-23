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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using Microsoft.WindowsAzure.MobileServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        /*private async void redirect()
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
        }*/
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

            Progress.Text = "Logging you in..";
            Progress.IsIndeterminate = true;
            Progress.IsVisible = true;
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
                        Progress.IsVisible = false;
                        this.NavigationService.Navigate(new Uri("/Profile.xaml", UriKind.Relative));
                    }

                    else
                    {
                        ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                        CustomMessageBox mbox = new CustomMessageBox()
                        {
                            Caption = "Error",
                            Message = "Nope! That username/password seems to be not working.",
                            Background = img,
                            LeftButtonContent = "Try Another"
                        };
                        mbox.Show();
                        Progress.IsVisible = false;
                    }
                }
                else
                {
                    ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                    CustomMessageBox mbox = new CustomMessageBox()
                    {
                        Caption = "Error",
                        Message = "Nope! That username does not exist.",
                        Background = img,
                        LeftButtonContent = "OK"
                    };
                    mbox.Show();
                    Progress.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                CustomMessageBox mbox = new CustomMessageBox()
                {
                    Caption = "Error",
                    Message = "There is something wrong! Don't worry try again!",
                    Background = img,
                    LeftButtonContent = "Try Again"
                };
                mbox.Show();
                Progress.IsVisible = false;
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
            Progress.Text = "Signing you up! Hold on..";
            Progress.IsVisible = true;
            sgup.Visibility = Visibility.Collapsed;
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
                            ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                            CustomMessageBox mbox = new CustomMessageBox()
                            {
                                Caption = "Duplicate",
                                Message = "Sorry! That name is already taken. Choose another.",
                                Background = img,
                                LeftButtonContent = "Sure"
                            };
                            mbox.Show();
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
                            ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                            CustomMessageBox mbox = new CustomMessageBox()
                            {
                                Caption = "Signup Success",
                                Message = "Voila! Successfully signed up. Now login.",
                                Background = img,
                                LeftButtonContent = "Great"
                            };
                            mbox.Show(); pass.Password = "";
                            Progress.IsVisible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                        CustomMessageBox mbox = new CustomMessageBox()
                        {
                            Caption = "Error",
                            Message = "Can't log you in!",
                            Background = img,
                            LeftButtonContent = "OK"
                        };
                        mbox.Show();
                        Progress.IsVisible = false;
                    }
                }
                else
                {
                    ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                    CustomMessageBox mbox = new CustomMessageBox()
                    {
                        Caption = "Error",
                        Message = "Enter a valid email-id.",
                        Background = img,
                        LeftButtonContent = "Sure"
                    };
                    mbox.Show();
                    Progress.IsVisible = false;
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
                Progress.IsVisible = false;
            }
            sgup.Visibility = Visibility.Visible;
        }




    }
}