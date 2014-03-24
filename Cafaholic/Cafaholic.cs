using Cafaholic.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cafaholic
{

    public class FourSquare
    {
        public string client_id = "JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4";
        public string client_secret = "3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH";
        public static string city;
        public static string radius="1000";
        public List<String> cafe_venues = new List<string>();
        public List<String> cafe_addresses = new List<string>();
        public List<String> cafe_likes = new List<string>();
        public List<String> cafe_lat = new List<string>();
        public List<String> cafe_long = new List<string>();
        public List<String> cafe_hours = new List<string>();
        public List<String> cafe_checkins = new List<string>();
        public List<String> bar_venues = new List<string>();
        public List<String> bar_addresses = new List<string>();
        public List<String> bar_likes = new List<string>();
        public List<String> bar_lat = new List<string>();
        public List<String> bar_long = new List<string>();
        public List<String> bar_hours = new List<string>();
        public List<String> bar_checkins = new List<string>();
        public static bool cafe_loaded = false;
        public static bool bar_loaded = false;
        public void getcafes(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll="+_lat+","+_long+"&llAcc=1000&radius=1000&section=coffee&limit=10&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            cafeloadcomplete();
        }

        public void getbars(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll="+_lat+","+_long+"&llAcc=1000&radius=1000&section=drinks&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
            //barloadcomplete();
        }
        public void getcafes2km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&section=coffee&limit=10&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }

        public void getcafes5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&section=coffee&limit=10&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }

        public void getbars2km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&section=drinks&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
        }

        public void getbars5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&section=drinks&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
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
                if (items.Count == 0)
                    MessageBox.Show("No cafes in your area now!");
                foreach (JObject item in items)
                {
                    var venue = item["venue"]["name"];
                    var address = item["venue"]["location"]["address"];
                    var latitude = item["venue"]["location"]["lat"];
                    var longitude = item["venue"]["location"]["lng"];
                    var like = item["venue"]["likes"]["count"];
                    var checkins = item["venue"]["stats"]["checkinsCount"];

                    if (null != item["venue"]["hours"])
                    {
                        var hours = item["venue"]["hours"]["status"];
                        cafe_hours.Add(hours.ToString());
                    }
                    else
                    {
                        cafe_hours.Add("------------");
                    }

                    cafe_venues.Add(venue.ToString());
                    cafe_addresses.Add(address.ToString());
                    cafe_likes.Add(like.ToString());
                    cafe_lat.Add(latitude.ToString());
                    cafe_long.Add(longitude.ToString());
                    cafe_checkins.Add(checkins.ToString());
                }
                //city = _city.ToString();
                if (cafe_venues.Count > 0)
                {
                    App.ViewModel.Items.Clear();
                    for (int i = 0; i < cafe_venues.Count; i++)
                    {
                        App.ViewModel.Items.Add(new ItemViewModel { LineOne = cafe_venues[i], LineTwo = cafe_addresses[i], LineThree = cafe_likes[i], Latitude= cafe_lat[i], Longitude=cafe_long[i], Hours=cafe_hours[i], Rating=cafe_checkins[i]});

                    }
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ran into a problem..Try again.");
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
                MessageBox.Show("No bars in your area now!");
            foreach (JObject item in items)
            {
                var venue = item["venue"]["name"];
                var address = item["venue"]["location"]["address"];
                var like = item["venue"]["likes"]["count"];
                var latitude = item["venue"]["location"]["lat"];
                var longitude = item["venue"]["location"]["lng"];

                
                if (null != item["venue"]["stats"]["checkinsCount"])
                {
                    var checkins = item["venue"]["stats"]["checkinsCount"];

                    bar_checkins.Add(checkins.ToString());
                }
                else
                {
                    bar_checkins.Add("0");
                }

                if (null != item["venue"]["hours"])
                {
                    var hours = item["venue"]["hours"]["status"];
                    bar_hours.Add(hours.ToString());
                }
                else
                {
                    bar_hours.Add("------------");
                }

                bar_venues.Add(venue.ToString());
                if(address!=null)
                    bar_addresses.Add(address.ToString());
                else
                    bar_addresses.Add("------------");
                bar_likes.Add(like.ToString());
                bar_lat.Add(latitude.ToString());
                bar_long.Add(longitude.ToString());
                
            }
            //city = _city.ToString();
            if (bar_venues.Count > 0)
            {
                App.ViewModel.Bar.Clear();
                for (int i = 0; i < bar_venues.Count; i++)
                {
                    App.ViewModel.Bar.Add(new Bars { LineOne = bar_venues[i], LineTwo = bar_addresses[i], LineThree = bar_likes[i], Latitude=bar_lat[i], Longitude=bar_long[i], Hours=bar_hours[i], Rating=bar_checkins[i]});
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
