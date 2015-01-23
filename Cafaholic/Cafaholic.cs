using Cafaholic.ViewModels;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cafaholic
{

    public class FourSquare
    {
        public string client_id = "JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4";
        public string client_secret = "3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH";
        public List<String> cafe_id = new List<string>();
        public List<String> cafe_venues = new List<string>();
        public List<String> cafe_addresses = new List<string>();
        public List<String> cafe_likes = new List<string>();
        public List<String> cafe_lat = new List<string>();
        public List<String> cafe_long = new List<string>();
        public List<String> cafe_hours = new List<string>();
        public List<String> cafe_checkins = new List<string>();
        public List<String> cafe_price = new List<string>();
        public List<String> cafe_contact = new List<string>();
        public List<String> bar_id = new List<string>();
        public List<String> bar_venues = new List<string>();
        public List<String> bar_addresses = new List<string>();
        public List<String> bar_likes = new List<string>();
        public List<String> bar_lat = new List<string>();
        public List<String> bar_long = new List<string>();
        public List<String> bar_hours = new List<string>();
        public List<String> bar_checkins = new List<string>();
        public List<String> bar_price = new List<string>();
        public List<String> bar_contact = new List<string>();
        public static bool cafe_loaded = false;
        public static bool bar_loaded = false;
        public static string city;
        public void getcafes(string _lat, string _long)
        {
            WebClient wc = new WebClient();

            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=1000&section=coffee&limit=10&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }
        public void getcafesanywhere(string _lat, string _long)
        {
            WebClient wc = new WebClient();

            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=15000&section=coffee&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }
        public void getccds(string _lat, string _long)
        {
            WebClient wc = new WebClient();

            Uri ccd_request = new Uri("https://api.foursquare.com/v2/venues/search?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&query=ccd&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CcdCompletedDownload);
            wc.DownloadStringAsync(ccd_request);
            //cafeloadcomplete();
        }

        public void getccdsanywhere(string _lat, string _long)
        {
            WebClient wc = new WebClient();

            Uri ccd_request = new Uri("https://api.foursquare.com/v2/venues/search?ll=" + _lat + "," + _long + "&llAcc=1000&radius=12000&query=ccd&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CcdCompletedDownload);
            wc.DownloadStringAsync(ccd_request);
            //cafeloadcomplete();
        }

        public void getbars(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=1000&section=drinks&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
            //barloadcomplete();
        }
        public void getbarsanywhere(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=15000&section=drinks&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
            //barloadcomplete();
        }
        public void getcafes2km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&section=coffee&limit=10&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }

        public void getcafes5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&section=coffee&limit=10&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }

        public void getccds5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();

            Uri ccd_request = new Uri("https://api.foursquare.com/v2/venues/search?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&query=ccd&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CcdCompletedDownload);
            wc.DownloadStringAsync(ccd_request);
            //cafeloadcomplete();
        }

        public void getbars2km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&section=drinks&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
        }

        public void getbars5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&section=drinks&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20150115", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
        }

        private void CcdCompletedDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                var container = JsonConvert.DeserializeObject(e.Result) as JObject;
                var _city = container["response"]["headerLocation"];

                List<JObject> result = container["response"]["venues"].Children()
                                    .Cast<JObject>()

                                    .ToList();

                foreach (JObject item in result)
                {

                    var id = item["id"];
                    var venue = item["name"];
                    if (null != item["location"]["address"])
                    {
                        var address = item["location"]["address"];
                        cafe_addresses.Add(address.ToString());
                    }
                    else
                    {
                        cafe_addresses.Add("");
                    }
                    var latitude = item["location"]["lat"];
                    var longitude = item["location"]["lng"];
                    if (null != item["contact"]["phone"])
                    {
                        var contact = item["contact"]["phone"];
                        cafe_contact.Add(contact.ToString());
                    }
                    else
                    {
                        cafe_contact.Add("None");
                    }
                    if (null != item["stats"]["checkinsCount"])
                    {
                        var checkins = item["stats"]["checkinsCount"];

                        cafe_checkins.Add(checkins.ToString());
                    }
                    else
                    {
                        cafe_checkins.Add("0");
                    }

                    var price = "1";
                    cafe_price.Add(price.ToString());

                    if (Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) <= 23 && Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) >= 8)
                        cafe_hours.Add("Likely Open");
                    else
                        cafe_hours.Add("Closed");

                    cafe_id.Add(id.ToString());
                    cafe_venues.Add(venue.ToString());
                    //cafe_addresses.Add(address.ToString());
                    cafe_likes.Add("0");
                    cafe_lat.Add(latitude.ToString());
                    cafe_long.Add(longitude.ToString());



                    App.ViewModel.Items.Clear();
                    //city = _city.ToString();
                    if (cafe_venues.Count > 0)
                    {
                        App.ViewModel.Items.Clear();
                        for (int i = 0; i < cafe_venues.Count; i++)
                        {
                            App.ViewModel.Items.Add(new ItemViewModel { Id = cafe_id[i], LineOne = cafe_venues[i], LineTwo = cafe_addresses[i], LineThree = cafe_likes[i], Latitude = cafe_lat[i], Longitude = cafe_long[i], Hours = cafe_hours[i], Rating = cafe_checkins[i], Price = cafe_price[i], Contact = cafe_contact[i] });

                        }
                    }


                }

            }

            catch (Exception ex)
            {
                ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                CustomMessageBox mbox = new CustomMessageBox()
                {
                    Caption = "Error",
                    Message = "Phew! Something went wrong there. Maybe the internet is getting old!",
                    Background = img,
                    LeftButtonContent = "Ignore"
                };
                mbox.Show();
            }
        }
        private void CafeCompletedDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                var container = JsonConvert.DeserializeObject(e.Result) as JObject;
                //var _city = container["response"]["geocode"]["where"];

                List<JObject> result = container["response"]["groups"].Children()
                                    .Cast<JObject>()

                                    .ToList();
                List<JObject> items = result[0]["items"].Children()
                                    .Cast<JObject>()

                                    .ToList();
                if (items.Count == 0 && App.ViewModel.Items.Count == 0)
                {
                    ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                    CustomMessageBox mbox = new CustomMessageBox()
                    {
                        Caption = "No Cafes",
                        Message = "No cafes in your area! Try increase radius.",
                        Background = img,
                        LeftButtonContent = "Cool"
                    };
                    mbox.Show();
                }
                foreach (JObject item in items)
                {
                    try
                    {
                        try
                        {
                            List<JObject> tips = item["tips"].Children()
                                    .Cast<JObject>()

                                    .ToList();
                            try
                            {
                                var likes = tips[0]["likes"]["count"];
                                cafe_likes.Add(likes.ToString());
                            }

                            catch (Exception ex)
                            {
                                var likes = "0";
                                cafe_likes.Add(likes.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            cafe_likes.Add("0");
                        }
                        //var likes = tips[0]["likes"]["count"];

                    }
                    catch (Exception p)
                    {
                        var likes = "0";
                        cafe_likes.Add(likes.ToString());
                    }
                    var id = item["venue"]["id"];
                    var venue = item["venue"]["name"];
                    if (null != item["venue"]["location"]["address"])
                    {
                        var address = item["venue"]["location"]["address"];
                        cafe_addresses.Add(address.ToString());
                    }
                    else
                    {
                        var address = item["venue"]["location"]["country"];
                        cafe_addresses.Add(address.ToString());
                    }
                    var latitude = item["venue"]["location"]["lat"];
                    var longitude = item["venue"]["location"]["lng"];
                    if (null != item["venue"]["contact"]["phone"])
                    {
                        var contact = item["venue"]["contact"]["phone"];
                        cafe_contact.Add(contact.ToString());
                    }
                    else
                    {
                        cafe_contact.Add("No Contact");
                    }
                    if (null != item["venue"]["stats"]["checkinsCount"])
                    {
                        var checkins = item["venue"]["stats"]["checkinsCount"];

                        cafe_checkins.Add(checkins.ToString());
                    }
                    else
                    {
                        cafe_checkins.Add("0");
                    }
                    try
                    {
                        var price = item["venue"]["price"]["tier"];
                        cafe_price.Add(price.ToString());
                    }
                    catch (Exception ex)
                    {
                        var price = "1";
                        cafe_price.Add(price.ToString());
                    }
                    if (null != item["venue"]["hours"])
                    {
                        if (null != item["venue"]["hours"]["status"])
                        {
                            var hours = item["venue"]["hours"]["status"];
                            cafe_hours.Add(hours.ToString());
                        }
                        else
                        {
                            if (Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) <= 23 && Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) >= 8)
                                cafe_hours.Add("Likely Open");
                            else
                                cafe_hours.Add("Closed");
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) <= 23 && Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) >= 8)
                            cafe_hours.Add("Likely Open");
                        else
                            cafe_hours.Add("Closed");


                    }
                    cafe_id.Add(id.ToString());
                    cafe_venues.Add(venue.ToString());
                    
                    //cafe_likes.Add(likes.ToString());
                    cafe_lat.Add(latitude.ToString());
                    cafe_long.Add(longitude.ToString());


                }
                App.ViewModel.Items.Clear();
                //city = _city.ToString();
                if (cafe_venues.Count > 0)
                {
                    App.ViewModel.Items.Clear();
                    for (int i = 0; i < cafe_venues.Count; i++)
                    {
                        App.ViewModel.Items.Add(new ItemViewModel { Id = cafe_id[i], LineOne = cafe_venues[i], LineTwo = cafe_addresses[i], LineThree = cafe_likes[i], Latitude = cafe_lat[i], Longitude = cafe_long[i], Hours = cafe_hours[i], Rating = cafe_checkins[i], Price = cafe_price[i], Contact = cafe_contact[i] });

                    }
                }


            }
            catch (Exception ex)
            {
                ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                CustomMessageBox mbox = new CustomMessageBox()
                {
                    Caption = "Error",
                    Message = "Phew! Something went wrong there. Maybe the internet is getting old!",
                    Background = img,
                    LeftButtonContent = "Ignore"
                };
                mbox.Show();
            }
        }

        private void BarCompletedDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            var container = JsonConvert.DeserializeObject(e.Result) as JObject;
            //var _city = container["response"]["geocode"]["where"];

            List<JObject> result = container["response"]["groups"].Children()
                                .Cast<JObject>()

                                .ToList();
            List<JObject> items = result[0]["items"].Children()
                                .Cast<JObject>()

                                .ToList();
            if (items.Count == 0)
            {
                ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
                CustomMessageBox mbox = new CustomMessageBox()
                {
                    Caption = "No Bars",
                    Message = "No bars in your area! Try increase radius.",
                    Background = img,
                    LeftButtonContent = "Cool"
                };
                mbox.Show();
            }
            foreach (JObject item in items)
            {
                try
                {
                    try
                    {
                        List<JObject> tips = item["tips"].Children()
                                .Cast<JObject>()

                                .ToList();
                        try
                        {
                            var likes = tips[0]["likes"]["count"];
                            bar_likes.Add(likes.ToString());
                        }

                        catch (Exception ex)
                        {
                            var likes = "0";
                            bar_likes.Add(likes.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        bar_likes.Add("0");
                    }
                    //var likes = tips[0]["likes"]["count"];

                }
                catch (Exception p)
                {
                    var likes = "0";
                    bar_likes.Add(likes.ToString());
                }
                var id = item["venue"]["id"];
                var venue = item["venue"]["name"];
                if (null != item["venue"]["location"]["address"])
                {
                    var address = item["venue"]["location"]["address"];
                    bar_addresses.Add(address.ToString());
                }
                else
                {
                    var address = item["venue"]["location"]["country"];
                    bar_addresses.Add(address.ToString());
                }
                var latitude = item["venue"]["location"]["lat"];
                var longitude = item["venue"]["location"]["lng"];
                if (null != item["venue"]["contact"]["phone"])
                {
                    var contact = item["venue"]["contact"]["phone"];
                    bar_contact.Add(contact.ToString());
                }
                else
                {
                    bar_contact.Add("No Contact");
                }
                if (null != item["venue"]["stats"]["checkinsCount"])
                {
                    var checkins = item["venue"]["stats"]["checkinsCount"];

                    bar_checkins.Add(checkins.ToString());
                }
                else
                {
                    bar_checkins.Add("0");
                }
                try
                {
                    var price = item["venue"]["price"]["tier"];
                    bar_price.Add(price.ToString());
                }
                catch (Exception ex)
                {
                    var price = "1";
                    bar_price.Add(price.ToString());
                }
                if (null != item["venue"]["hours"])
                {
                    if (null != item["venue"]["hours"]["status"])
                    {
                        var hours = item["venue"]["hours"]["status"];
                        bar_hours.Add(hours.ToString());
                    }
                    else
                    {
                        if (Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) <= 23 && Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) >= 8)
                            bar_hours.Add("Likely Open");
                        else
                            bar_hours.Add("Closed");
                    }
                }
                else
                {
                    if (Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) <= 23 && Convert.ToInt32(DateTime.Now.TimeOfDay.Hours) >= 8)
                        bar_hours.Add("Likely Open");
                    else
                        bar_hours.Add("Closed");


                }
                bar_id.Add(id.ToString());
                bar_venues.Add(venue.ToString());

                //cafe_likes.Add(likes.ToString());
                bar_lat.Add(latitude.ToString());
                bar_long.Add(longitude.ToString());


            }
            //city = _city.ToString();
            App.ViewModel.Bar.Clear();
            if (bar_venues.Count > 0)
            {
                App.ViewModel.Bar.Clear();
                for (int i = 0; i < bar_venues.Count; i++)
                {
                    App.ViewModel.Bar.Add(new Bars { Id = bar_id[i], LineOne = bar_venues[i], LineTwo = bar_addresses[i], LineThree = bar_likes[i], Latitude = bar_lat[i], Longitude = bar_long[i], Hours = bar_hours[i], Rating = bar_checkins[i], Price = bar_price[i], Contact = bar_contact[i] });
                }
            }


        }

        public void barloadcomplete()
        {
            bar_loaded = true;

            //return bar_loaded;
        }

        public void cafeloadcomplete()
        {
            cafe_loaded = true;
            //return cafe_loaded;
        }

    }


}
