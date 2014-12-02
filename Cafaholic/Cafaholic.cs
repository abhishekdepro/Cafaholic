/* Cafaholic Core Class Library
// Author: Abhishek Dey.
// Version: 2.4.12.1.
// Description: Contains all functions to fetch data using Foursquare API.
// Latest: Added Cafe Coffee Day using Search.
*/

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
	// class deals with all data access to Foursquare RESTful API.
    public class FourSquare
    {
		//API_Keys.
        public string client_id = "<YOUR_CLIENT_ID>";
        public string client_secret = "<YOUR_CLIENT_SECRET>";
        
		//List objects to store cafe details such as address, rating.
		public List<String> cafe_venues = new List<string>();
        public List<String> cafe_addresses = new List<string>();
        public List<String> cafe_likes = new List<string>();
        public List<String> cafe_lat = new List<string>();
        public List<String> cafe_long = new List<string>();
        public List<String> cafe_hours = new List<string>();
        public List<String> cafe_checkins = new List<string>();
        public List<String> cafe_price = new List<string>();
        public List<String> cafe_contact = new List<string>();

		//List objects to store bar details such as address, rating.
        public List<String> bar_venues = new List<string>();
        public List<String> bar_addresses = new List<string>();
        public List<String> bar_likes = new List<string>();
        public List<String> bar_lat = new List<string>();
        public List<String> bar_long = new List<string>();
        public List<String> bar_hours = new List<string>();
        public List<String> bar_checkins = new List<string>();
        public List<String> bar_price = new List<string>();
        public List<String> bar_contact = new List<string>();
        
		//optinal boolean responses: if needed.
		public static bool cafe_loaded = false;
        public static bool bar_loaded = false;
        
		//optional parameter: if required.
		public static string city;

		//method to make request for all cafes based on geo-location.
        public void getcafes(string _lat, string _long)
        {
            WebClient wc = new WebClient();
        
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll="+_lat+","+_long+"&llAcc=1000&radius=1000&section=coffee&limit=10&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
            //cafeloadcomplete();
        }

        public void getccds(string _lat, string _long)
        {
            WebClient wc = new WebClient();
			Uri ccd_request = new Uri("https://api.foursquare.com/v2/venues/search?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&query=ccd&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CcdCompletedDownload);
            wc.DownloadStringAsync(ccd_request);
        }

        
		//method to make request for all cafes based on geo-location.
        public void getbars(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri bar_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll="+_lat+","+_long+"&llAcc=1000&radius=1000&section=drinks&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BarCompletedDownload);
            wc.DownloadStringAsync(bar_request);
        }
        public void getcafes2km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=2000&section=coffee&limit=10&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
        }

        public void getcafes5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
            Uri coffee_request = new Uri("https://api.foursquare.com/v2/venues/explore?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&section=coffee&limit=10&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CafeCompletedDownload);
            wc.DownloadStringAsync(coffee_request);
        }

        public void getccds5km(string _lat, string _long)
        {
            WebClient wc = new WebClient();
			Uri ccd_request = new Uri("https://api.foursquare.com/v2/venues/search?ll=" + _lat + "," + _long + "&llAcc=1000&radius=5000&query=ccd&openNow=1&client_id=JJRQQGJTDLRNZWERBNQ0BTUYS2P4ZVBFA5MWHU5MEEJBINB4&client_secret=3MS1HAACC4RAU253OIT340HODO1CDQIDFZQNNSIMHPB2CVWH&v=20130815", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CcdCompletedDownload);
            wc.DownloadStringAsync(ccd_request);
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
                    
                        
                        var venue = item["name"];
                        var address = item["location"]["address"];
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


                        cafe_hours.Add("------------");


                        cafe_venues.Add(venue.ToString());
                        cafe_addresses.Add(address.ToString());
                        cafe_likes.Add("0");
                        cafe_lat.Add(latitude.ToString());
                        cafe_long.Add(longitude.ToString());



                        App.ViewModel.Items.Clear();
                        if (cafe_venues.Count > 0)
                        {
                            App.ViewModel.Items.Clear();
                            for (int i = 0; i < cafe_venues.Count; i++)
                            {
                                App.ViewModel.Items.Add(new ItemViewModel { LineOne = cafe_venues[i], LineTwo = cafe_addresses[i], LineThree = cafe_likes[i], Latitude = cafe_lat[i], Longitude = cafe_long[i], Hours = cafe_hours[i], Rating = cafe_checkins[i], Price = cafe_price[i], Contact = cafe_contact[i] });

                            }
                        }


                    }
                    
                }
            
            catch (Exception ex)
            {
                MessageBox.Show("Ran into a problem..Try again.");
            }
        }
        private void CafeCompletedDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                
                var container = JsonConvert.DeserializeObject(e.Result) as JObject;
                List<JObject> result = container["response"]["groups"].Children()
                                    .Cast<JObject>()
									.ToList();
                List<JObject> items = result[0]["items"].Children()
                                    .Cast<JObject>()
									.ToList();
                if (items.Count == 0 && App.ViewModel.Items.Count==0)
                    MessageBox.Show("No cafes in your area now!");
                foreach (JObject item in items)
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
                    catch (Exception p)
                    {
                        var likes = "0";
                        cafe_likes.Add(likes.ToString());
                    }
                    var venue = item["venue"]["name"];
                    var address = item["venue"]["location"]["address"];
                    var latitude = item["venue"]["location"]["lat"];
                    var longitude = item["venue"]["location"]["lng"];
                    if (null != item["venue"]["contact"]["phone"])
                    {
                        var contact = item["venue"]["contact"]["phone"];
                        cafe_contact.Add(contact.ToString());
                    }
                    else
                    {
                        cafe_contact.Add("No Contact Available");
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
                        var hours = item["venue"]["hours"]["status"];
                        cafe_hours.Add(hours.ToString());
                    }
                    else
                    {
                        cafe_hours.Add("------------");
                    }

                    cafe_venues.Add(venue.ToString());
                    cafe_addresses.Add(address.ToString());
                    cafe_lat.Add(latitude.ToString());
                    cafe_long.Add(longitude.ToString());
                    
                }
                App.ViewModel.Items.Clear();
                if (cafe_venues.Count > 0)
                {
                    App.ViewModel.Items.Clear();
                    for (int i = 0; i < cafe_venues.Count; i++)
                    {
                        App.ViewModel.Items.Add(new ItemViewModel { LineOne = cafe_venues[i], LineTwo = cafe_addresses[i], LineThree = cafe_likes[i], Latitude= cafe_lat[i], Longitude=cafe_long[i], Hours=cafe_hours[i], Rating=cafe_checkins[i], Price=cafe_price[i], Contact=cafe_contact[i]});
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
                catch (Exception p) {
                    var likes = "0";
                    bar_likes.Add(likes.ToString());
                }
                var venue = item["venue"]["name"];
                var address = item["venue"]["location"]["address"];
                var latitude = item["venue"]["location"]["lat"];
                var longitude = item["venue"]["location"]["lng"];
                var price = item["venue"]["price"]["tier"];
                if (null != item["venue"]["contact"]["phone"])
                {
                    var contact = item["venue"]["contact"]["phone"];
                    bar_contact.Add(contact.ToString());
                }
                else
                {
                    bar_contact.Add("No Contact Available");
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
                
                bar_lat.Add(latitude.ToString());
                bar_long.Add(longitude.ToString());
                bar_price.Add(price.ToString());
            }
            App.ViewModel.Bar.Clear();
            if (bar_venues.Count > 0)
            {
                App.ViewModel.Bar.Clear();
                for (int i = 0; i < bar_venues.Count; i++)
                {
                    App.ViewModel.Bar.Add(new Bars { LineOne = bar_venues[i], LineTwo = bar_addresses[i], LineThree = bar_likes[i], Latitude=bar_lat[i], Longitude=bar_long[i], Hours=bar_hours[i], Rating=bar_checkins[i], Price=bar_price[i], Contact=bar_contact[i]});
                }
            }
            
            
        }

    }


}
