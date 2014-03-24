using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps;
using System.Device.Location;
using Nokia.Phone.HereLaunchers;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Cafaholic
{
    public partial class cafe : PhoneApplicationPage
    {
        Coffee selected_Cafe;
        public cafe()
        {
            InitializeComponent();



            var venue = PhoneApplicationService.Current.State["venue"];
            var address = PhoneApplicationService.Current.State["address"];
            var latitude = PhoneApplicationService.Current.State["latitude"];
            var longitude = PhoneApplicationService.Current.State["longitude"];
            var hours = PhoneApplicationService.Current.State["hours"];
            var likes = PhoneApplicationService.Current.State["likes"];
            var checkins = PhoneApplicationService.Current.State["checkins"];

            selected_Cafe = new Coffee { LineOne = venue.ToString(), LineTwo = address.ToString(), LineThree = likes.ToString(), Latitude = latitude.ToString(), Longitude = longitude.ToString(), Hours = hours.ToString(), Rating = checkins.ToString() };
            DataContext = selected_Cafe;
            if (Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) >= 19)
                myMap.ColorMode = MapColorMode.Dark;
            myMap.Center = new GeoCoordinate(Double.Parse(latitude.ToString()), Double.Parse(longitude.ToString()));
            myMap.ZoomLevel = 18;

            BitmapImage bi3 = new BitmapImage();

            bi3.UriSource = new Uri("/Assets/push.ico", UriKind.Relative);
            //bi3.EndInit();

            MapOverlay overlay = new MapOverlay
{
    GeoCoordinate = myMap.Center,
    Content = new Image
    {
        Source = bi3,
        Stretch = Stretch.Uniform,
        Width = 50,
        Height = 80
    }
};
            MapLayer layer = new MapLayer();
            layer.Add(overlay);

            myMap.Layers.Add(layer);
        }

        private void myMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "209f7ed9-d7b6-45cc-9f16-4dabe80bc4ce";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "KoFXdhlWCoFntL13mpwT1Q";

        }

        private void myMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var latitude = PhoneApplicationService.Current.State["latitude"];
            var longitude = PhoneApplicationService.Current.State["longitude"];
            myMap.Center = new GeoCoordinate(Double.Parse(latitude.ToString()), Double.Parse(longitude.ToString()));
            myMap.ZoomLevel = 18;

            BitmapImage bi3 = new BitmapImage();

            bi3.UriSource = new Uri("/Assets/push.ico", UriKind.Relative);
            //bi3.EndInit();

            MapOverlay overlay = new MapOverlay
{
    GeoCoordinate = myMap.Center,
    Content = new Image
    {
        Source = bi3,
        Stretch = Stretch.Uniform,
        Width = 50,
        Height = 80
    }
};
            MapLayer layer = new MapLayer();
            layer.Add(overlay);

            myMap.Layers.Add(layer);
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
    }


}
