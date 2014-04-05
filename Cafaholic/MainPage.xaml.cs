using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Cafaholic.ViewModels;
using System.Device.Location;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;

namespace Cafaholic
{
    public partial class MainPage : PhoneApplicationPage
    {
        public Geoposition pos;
        PanoramaItem p;
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (appSettings.Contains("togglebars"))
            {
                string status = (string)appSettings["togglebars"];
                if (status == "x")
                {
                    //main.Items.RemoveAt(2)
                    barToggle.IsChecked = true;
                }
            }
            else if (appSettings.Contains("togglecafes"))
            {
                string status = (string)appSettings["togglecafes"];
                if (status == "x")
                {
                    cafeToggle.IsChecked = true;
                }
            }
            //check for location permissions

            if (appSettings.Contains("LocationConsent"))
            {
                
                
                    if ((bool)appSettings["LocationConsent"] == true)
                    {
                        locationToggle.IsChecked = true;
                    }
                    else
                    {
                        locationToggle.IsChecked = false;
                    }
                
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Allow this app to access your location?", "Location", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    //appSettings["LocationConsent"] = true;
                    locationToggle.IsChecked = true;
                }
                else
                {
                    appSettings["LocationConsent"] = false;
                    App.Current.Terminate();
                }
                appSettings.Save();
            }

            //main.DefaultItem = main.Items[2];
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //Shows the rate reminder message, according to the settings of the RateReminder.
            //(App.Current as App).rateReminder.Notify();
            getGeoLocation(1);
            main.DefaultItem = main.Items[1];
            //GeoCoordinate pos = getGeoLocation();
            
            //busy.IsRunning = false;
            
        }

        

        private async void getGeoLocation(int radius)
        {
            /*GeoCoordinateWatcher gc = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            if (gc.Status == GeoPositionStatus.Disabled)
            {
                MessageBox.Show("Please enable Location");
                return null;
            }
            else
            {
                return gc.Position.Location;
            }*/
            if ((bool)appSettings["LocationConsent"] == true)
            {
                Geolocator gc = new Geolocator();
                try
                {
                    pos = await gc.GetGeopositionAsync();

                    string latitude = pos.Coordinate.Latitude.ToString();
                    string longitude = pos.Coordinate.Longitude.ToString();
                    FourSquare fs = new FourSquare();
                    switch (radius)
                    {
                        case 1:
                            fs.getcafes(latitude, longitude);
                            fs.getbars(latitude, longitude);
                            break;
                        case 2:
                            fs.getcafes2km(latitude, longitude);
                            fs.getbars2km(latitude, longitude);
                            break;
                        case 5:
                            fs.getcafes5km(latitude, longitude);
                            fs.getbars5km(latitude, longitude);
                            break;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please turn Location services on!");
                }
            }
            else
            {
                MessageBox.Show("Please enable location in settings!");
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)

            {
                
                App.ViewModel.LoadData();
            }
            //main.Title = FourSquare.city;
        }

        /// <summary>
        /// Navigates to about page.
        /// </summary>
        private void GoToAbout(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }

        private void twokm_Checked(object sender, RoutedEventArgs e)
        {
            getGeoLocation(2);
            main.DefaultItem = main.Items[1];
        }

        private void fivekm_Checked(object sender, RoutedEventArgs e)
        {
            getGeoLocation(5);
            main.DefaultItem = main.Items[1];
            
        }

        private void onekm_Checked(object sender, RoutedEventArgs e)
        {
            getGeoLocation(1);
            main.DefaultItem = main.Items[1];
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index;
            if (cafeToggle.IsChecked == false)
            {
                if (main.SelectedItem == main.Items[1])
                {
                    index = cafelist.SelectedIndex;
                    PhoneApplicationService.Current.State["venue"] = App.ViewModel.Items[index].LineOne;
                    PhoneApplicationService.Current.State["address"] = App.ViewModel.Items[index].LineTwo;
                    PhoneApplicationService.Current.State["latitude"] = App.ViewModel.Items[index].Latitude;
                    PhoneApplicationService.Current.State["longitude"] = App.ViewModel.Items[index].Longitude;
                    PhoneApplicationService.Current.State["hours"] = App.ViewModel.Items[index].Hours;
                    PhoneApplicationService.Current.State["likes"] = App.ViewModel.Items[index].LineThree;
                    PhoneApplicationService.Current.State["checkins"] = App.ViewModel.Items[index].Rating;
                    PhoneApplicationService.Current.State["price"] = App.ViewModel.Items[index].Price;
                    saveTile(App.ViewModel.Items[index].LineOne.ToString(), App.ViewModel.Items.Count.ToString(), "cafe");
                }
                else
                {
                    index = barlist.SelectedIndex;
                    PhoneApplicationService.Current.State["venue"] = App.ViewModel.Bar[index].LineOne;
                    PhoneApplicationService.Current.State["address"] = App.ViewModel.Bar[index].LineTwo;
                    PhoneApplicationService.Current.State["latitude"] = App.ViewModel.Bar[index].Latitude;
                    PhoneApplicationService.Current.State["longitude"] = App.ViewModel.Bar[index].Longitude;
                    PhoneApplicationService.Current.State["hours"] = App.ViewModel.Bar[index].Hours;
                    PhoneApplicationService.Current.State["likes"] = App.ViewModel.Bar[index].LineThree;
                    PhoneApplicationService.Current.State["checkins"] = App.ViewModel.Bar[index].Rating;
                    PhoneApplicationService.Current.State["price"] = App.ViewModel.Bar[index].Price;
                    saveTile(App.ViewModel.Bar[index].LineOne.ToString(), App.ViewModel.Bar.Count.ToString(), "bar");
                }
            }
            else
            {
                index = barlist.SelectedIndex;
                PhoneApplicationService.Current.State["venue"] = App.ViewModel.Bar[index].LineOne;
                PhoneApplicationService.Current.State["address"] = App.ViewModel.Bar[index].LineTwo;
                PhoneApplicationService.Current.State["latitude"] = App.ViewModel.Bar[index].Latitude;
                PhoneApplicationService.Current.State["longitude"] = App.ViewModel.Bar[index].Longitude;
                PhoneApplicationService.Current.State["hours"] = App.ViewModel.Bar[index].Hours;
                PhoneApplicationService.Current.State["likes"] = App.ViewModel.Bar[index].LineThree;
                PhoneApplicationService.Current.State["checkins"] = App.ViewModel.Bar[index].Rating;
                PhoneApplicationService.Current.State["price"] = App.ViewModel.Bar[index].Price;
                saveTile(App.ViewModel.Bar[index].LineOne.ToString(), App.ViewModel.Bar.Count.ToString(), "bar");
            }
            this.NavigationService.Navigate(new Uri("/Cafe.xaml", UriKind.Relative));
            
        }

        private void barToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (cafeToggle.IsChecked == true)
                cafeToggle.IsChecked = false;
            
            appSettings.Remove("togglebars");
            appSettings.Add("togglebars", "x");
            p = (PanoramaItem)main.Items[2];
            main.Items.RemoveAt(2);
        }

        private void cafeToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (barToggle.IsChecked == true)
                barToggle.IsChecked = false;

            appSettings.Remove("togglecafes");
            appSettings.Add("togglecafes", "x");
            p = (PanoramaItem)main.Items[1];
            main.Items.RemoveAt(1);
        }

        private void barToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings.Remove("togglebars");
            main.Items.Insert(2, p);
            main.DefaultItem = main.Items[2];
        }

        private void cafeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings.Remove("togglecafes");
            main.Items.Insert(1, p);
            main.DefaultItem = main.Items[1];
        }


        private void saveTile(string item, string count, string selected)
        {
            ShellTile oTile = ShellTile.ActiveTiles.FirstOrDefault();


            if (oTile != null)
            {
                IconicTileData oFliptile = new IconicTileData();
                oFliptile.Title = "Number of "+selected+"s here:";
                oFliptile.Count = Int32.Parse(count);
                oFliptile.WideContent1 = "Cafaholic";
                if(selected=="cafe")
                    oFliptile.WideContent3 = "Coffee is good for health";
                else
                    oFliptile.WideContent3 = "Do not drink and drive";
                oFliptile.WideContent2 = item;
                oFliptile.SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmall.ico", UriKind.Relative);
                oFliptile.IconImage = new Uri("/Assets/Tiles/IconicTileSmall.ico", UriKind.Relative);
                oFliptile.BackgroundColor = (Color)Application.Current.Resources["PhoneAccentColor"];

                oTile.Update(oFliptile);
                //MessageBox.Show("Flip Tile Data successfully update.");
            }
            else
            {
                // once it is created flip tile
                Uri tileUri = new Uri("/MainPage.xaml?tile=flip", UriKind.Relative);
                ShellTileData tileData = this.CreateIconicTile();
                ShellTile.Create(tileUri, tileData, true);
            }
        }

        private ShellTileData CreateFlipTileData()
        {
            return new FlipTileData()
            {
                Title = "Mammail 3.0",
                BackTitle = "Mail.Easy.Fast",
                BackContent = "Messages",
                WideBackContent = "Messages",

                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
            };
        }


        private ShellTileData CreateIconicTile()
        {
            IconicTileData TileData = new IconicTileData()
                {
                   Title = "Cafaholic",
                   Count = 0,
                   WideContent1 = "Get Coffee instantly",
                   WideContent2 = "Get to a Bar now",
                   WideContent3 = "Location based",
                   SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmall.ico", UriKind.Relative),
                   IconImage = new Uri("/Assets/Tiles/IconicTileSmall.ico", UriKind.Relative),
                   BackgroundColor = (Color)Application.Current.Resources["PhoneAccentColor"]
                };
            return TileData;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            MarketplaceReviewTask review = new MarketplaceReviewTask();
          
            review.Show();
        }

        private void locationToggle_Checked(object sender, RoutedEventArgs e)
        {
            
                appSettings["LocationConsent"] = true;
                appSettings.Save();
            
        }

        private void locationToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            if (appSettings.Contains("LocationConsent"))
            {
                appSettings["LocationConsent"] = false;
                appSettings.Save();
            }
        }

        
        

        
    }
}
