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
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace Cafaholic
{
	public class Favorites
	{
		[JsonProperty(PropertyName = "Id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "User")]
		public string User { get; set; }

		[JsonProperty(PropertyName = "Venue")]
		public string Venue { get; set; }

		[JsonProperty(PropertyName = "Address")]
		public string Address { get; set; }
		[JsonProperty(PropertyName = "Likes")]
		public string Likes { get; set; }

		[JsonProperty(PropertyName = "VenueId")]
		public string VenueId { get; set; }

		private string _price;
		[JsonProperty(PropertyName = "Price")]
		public string Price { 
			
			get{
				if (_price == "1")
				{
					return "$";
				}
				else if (_price == "2")
				{
					return "$$";
				}
				else
					return "$$$";
			}
			set {
				if(value != _price)
					_price = value;
			} 
		}
		[JsonProperty(PropertyName = "Contact")]
		public string Contact { get; set; }
		[JsonProperty(PropertyName = "Category")]
		public string Category { get; set; }
	}
	public partial class MainPage : PhoneApplicationPage
	{
		public Geoposition pos;
		public ProgressIndicator progress;
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

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			NavigationService.RemoveBackEntry();

			
			/*if (NavigationService.BackStack.Any())
			{
				var length = NavigationService.BackStack.Count() - 1;
				var i = 0;
				while (i < length)
				{
					NavigationService.RemoveBackEntry();
					i++;
				}
			}*/

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
			//SystemTray.SetProgressIndicator(this, progress);
			//progress.IsVisible = true;
			
			if ((bool)appSettings["LocationConsent"] == true)
			{
				Geolocator gc = new Geolocator();
				gc.DesiredAccuracyInMeters = 500;
			
				try
				{
					pos = await gc.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5),timeout: TimeSpan.FromSeconds(10));

					string latitude = pos.Coordinate.Latitude.ToString();
					string longitude = pos.Coordinate.Longitude.ToString();
					FourSquare fs = new FourSquare();
					switch (radius)
					{
						case 1:
							fs.getccds(latitude, longitude);
							fs.getcafes(latitude, longitude);
						   
							fs.getbars(latitude, longitude);
							break;
						case 2:
							fs.getccds(latitude, longitude);
							fs.getcafes2km(latitude, longitude);

							fs.getbars2km(latitude, longitude);
							break;
						case 5:
							fs.getccds5km(latitude, longitude);
							fs.getcafes5km(latitude, longitude);
							fs.getbars5km(latitude, longitude);
							break;
						case 10:
							//fs.getccdsanywhere(latitude, longitude);
							fs.getcafesanywhere(latitude, longitude);
							fs.getbarsanywhere(latitude, longitude);
							break;
					}
					Progress.IsVisible = false;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Please turn location on!");
				}
			}
			else
			{
				MessageBox.Show("Please allow us know where you are! Else we can't confirm if you are a human or an alien? To turn location on go to App Settings > Location toggle on!","Turn Location On",MessageBoxButton.OK);
				
			}
		}

		async void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (Login.appSettings.Contains("count")==false)
			{
				Login.appSettings["count"] = "0";
			}
			if (!App.ViewModel.IsDataLoaded)

			{
				
				App.ViewModel.LoadData();
				try
				{
					if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString()!="")
					{
						await App.PersonalizedViewModel.LoadData();
						if (Login.appSettings["count"].ToString() != App.PersonalizedViewModel.favorites.Count.ToString())
						{
							Login.appSettings["count"] = App.PersonalizedViewModel.favorites.Count.ToString();
						}
					}
					Progress.IsVisible = false;
				}
				catch (Exception ex)
				{
					Progress.Text = "Cannot fetch data! No Internet!";
				}
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
			Progress.IsVisible = true;
			getGeoLocation(2);
			main.DefaultItem = main.Items[1];
			
		}

		private void fivekm_Checked(object sender, RoutedEventArgs e)
		{
			Progress.IsVisible = true;
			getGeoLocation(5);
			main.DefaultItem = main.Items[1];
			
		}

		private void any_Checked(object sender, RoutedEventArgs e)
		{
			Progress.IsVisible = true;
			getGeoLocation(10);
			main.DefaultItem = main.Items[1];
		}

		private void onekm_Checked(object sender, RoutedEventArgs e)
		{
			Progress.IsVisible = true;
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
					PhoneApplicationService.Current.State["id"] = App.ViewModel.Items[index].Id;
					PhoneApplicationService.Current.State["venue"] = App.ViewModel.Items[index].LineOne;
					PhoneApplicationService.Current.State["address"] = App.ViewModel.Items[index].LineTwo;
					PhoneApplicationService.Current.State["latitude"] = App.ViewModel.Items[index].Latitude;
					PhoneApplicationService.Current.State["longitude"] = App.ViewModel.Items[index].Longitude;
					PhoneApplicationService.Current.State["hours"] = App.ViewModel.Items[index].Hours;
					PhoneApplicationService.Current.State["likes"] = App.ViewModel.Items[index].LineThree;
					PhoneApplicationService.Current.State["checkins"] = App.ViewModel.Items[index].Rating;
					PhoneApplicationService.Current.State["price"] = App.ViewModel.Items[index].Price;
					PhoneApplicationService.Current.State["contact"] = App.ViewModel.Items[index].Contact;
					saveTile(App.ViewModel.Items[index].LineOne.ToString(),App.ViewModel.Items[index].Hours.ToString(), App.ViewModel.Items.Count.ToString(), "cafe");
				}
				else
				{
					index = barlist.SelectedIndex;
					PhoneApplicationService.Current.State["id"] = App.ViewModel.Bar[index].Id;
					PhoneApplicationService.Current.State["venue"] = App.ViewModel.Bar[index].LineOne;
					PhoneApplicationService.Current.State["address"] = App.ViewModel.Bar[index].LineTwo;
					PhoneApplicationService.Current.State["latitude"] = App.ViewModel.Bar[index].Latitude;
					PhoneApplicationService.Current.State["longitude"] = App.ViewModel.Bar[index].Longitude;
					PhoneApplicationService.Current.State["hours"] = App.ViewModel.Bar[index].Hours;
					PhoneApplicationService.Current.State["likes"] = App.ViewModel.Bar[index].LineThree;
					PhoneApplicationService.Current.State["checkins"] = App.ViewModel.Bar[index].Rating;
					PhoneApplicationService.Current.State["price"] = App.ViewModel.Bar[index].Price;
					PhoneApplicationService.Current.State["contact"] = App.ViewModel.Bar[index].Contact;
					saveTile(App.ViewModel.Bar[index].LineOne.ToString(),App.ViewModel.Bar[index].Hours.ToString(), App.ViewModel.Bar.Count.ToString(), "bar");
				}
			}
			else
			{
				index = barlist.SelectedIndex;
				PhoneApplicationService.Current.State["id"] = App.ViewModel.Bar[index].Id;
				PhoneApplicationService.Current.State["venue"] = App.ViewModel.Bar[index].LineOne;
				PhoneApplicationService.Current.State["address"] = App.ViewModel.Bar[index].LineTwo;
				PhoneApplicationService.Current.State["latitude"] = App.ViewModel.Bar[index].Latitude;
				PhoneApplicationService.Current.State["longitude"] = App.ViewModel.Bar[index].Longitude;
				PhoneApplicationService.Current.State["hours"] = App.ViewModel.Bar[index].Hours;
				PhoneApplicationService.Current.State["likes"] = App.ViewModel.Bar[index].LineThree;
				PhoneApplicationService.Current.State["checkins"] = App.ViewModel.Bar[index].Rating;
				PhoneApplicationService.Current.State["price"] = App.ViewModel.Bar[index].Price;
				PhoneApplicationService.Current.State["contact"] = App.ViewModel.Bar[index].Contact;
				saveTile(App.ViewModel.Bar[index].LineOne.ToString(),App.ViewModel.Bar[index].Hours.ToString(), App.ViewModel.Bar.Count.ToString(), "bar");
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


		private void saveTile(string item,string hours, string count, string selected)
		{
			ShellTile oTile = ShellTile.ActiveTiles.FirstOrDefault();


			if (oTile != null)
			{
				IconicTileData oFliptile = new IconicTileData();
				oFliptile.Title = "Number of "+selected+"s here:";
				oFliptile.Count = Int32.Parse(count);
				oFliptile.WideContent1 = "Cafaholic";
				if(selected=="cafe")
					oFliptile.WideContent3 = hours;
				else
					oFliptile.WideContent3 = hours;
				oFliptile.WideContent2 = item;
				oFliptile.SmallIconImage = new Uri("/Assets/Tiles/coffeeicon.png", UriKind.Relative);
				oFliptile.IconImage = new Uri("/Assets/Tiles/coffeeicon.png", UriKind.Relative);
				//oFliptile.BackgroundColor = (Color)Application.Current.Resources["PhoneAccentColor"];

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
				   SmallIconImage = new Uri("/Assets/Tiles/coffeeicon.png", UriKind.Relative),
				   IconImage = new Uri("/Assets/Tiles/coffeeicon.png", UriKind.Relative),
				  // BackgroundColor = (Color)Application.Current.Resources["PhoneAccentColor"]
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

		

		private MobileServiceCollection<Favorites, Favorites> items;
		private IMobileServiceTable<Favorites> Favorites = App.MobileService.GetTable<Favorites>();
		private async void Favorite_Click(object sender, RoutedEventArgs e)
		{
			int index;

			string venue,address,likes,price,contact,category,id;
			if (cafeToggle.IsChecked == false)
			{
				if (main.SelectedItem == main.Items[1])
				{
					index = cafelist.Items.IndexOf(((sender as MenuItem).DataContext));
					id = App.ViewModel.Items[index].Id;
					venue = App.ViewModel.Items[index].LineOne;
					address = App.ViewModel.Items[index].LineTwo;
					likes = App.ViewModel.Items[index].LineThree;
					price = App.ViewModel.Items[index].Price;
					contact = App.ViewModel.Items[index].Contact;
					category = "/Assets/coffee.png";
				}
				else
				{
					index = barlist.Items.IndexOf(((sender as MenuItem).DataContext));
					id = App.ViewModel.Bar[index].Id;
					venue = App.ViewModel.Bar[index].LineOne;
					address = App.ViewModel.Bar[index].LineTwo;
					likes = App.ViewModel.Bar[index].LineThree;
					price = App.ViewModel.Bar[index].Price;
					contact = App.ViewModel.Bar[index].Contact;
					category = "/Assets/beer.png";
				}
			}
			else
			{
				index = barlist.Items.IndexOf(((sender as MenuItem).DataContext));
				id = App.ViewModel.Bar[index].Id;
				venue = App.ViewModel.Bar[index].LineOne;
				address = App.ViewModel.Bar[index].LineTwo;
				likes = App.ViewModel.Bar[index].LineThree;
				price = App.ViewModel.Bar[index].Price;
				contact = App.ViewModel.Bar[index].Contact;
				category = "/Assets/beer.png";
			}
			if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString()!="")
			{
				Favorites todoItem = new Favorites { 
				VenueId = id,
				User = Login.appSettings["user"].ToString(),
				Venue = venue,
				Address = address,
				Likes = likes,
				Price = price,
				Contact = contact,
				Category = category
			};
			
				if (Login.appSettings.Contains("count"))
				{
					int cnt = Convert.ToInt32(Login.appSettings["count"]);
					Login.appSettings["count"] = (cnt + 1).ToString();
				}
				else
				{
					Login.appSettings.Add("count", "1");
				}
			
					await Favorites.InsertAsync(todoItem);

					ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
					CustomMessageBox mbox = new CustomMessageBox()
					{
						Caption = "Success",
						Message = "Voila! That's added to your favourites.",
						Background = img,
						LeftButtonContent = "Great"
					};
					mbox.Show();
					try
					{
						await App.PersonalizedViewModel.LoadData();
					}
					catch (Exception ex)
					{
						CustomMessageBox mb = new CustomMessageBox()
						{
							Caption = "Error",
							Message = "Snap! The internet is snapping us.",
							Background = img,
							LeftButtonContent = "OK"
						};
						mb.Show();
					}
				}
			else
			{
				this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
			}
			
		 }

		private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString() != "")
			{
				this.NavigationService.Navigate(new Uri("/Profile.xaml?item=1", UriKind.Relative));
			}
			else
			{
				this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
			}
		}


		private void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
		{
			if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString() != "")
			{
				this.NavigationService.Navigate(new Uri("/Profile.xaml?item=0", UriKind.Relative));
			}
			else
			{
				this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
			}
		}

		private void Image_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
		{
			if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString() != "")
			{
				this.NavigationService.Navigate(new Uri("/Profile.xaml?item=2", UriKind.Relative));
			}
			else
			{
				this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
			}
		}

		private void Checkin_Click(object sender, RoutedEventArgs e)
		{
			if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString() != "")
			{
				this.NavigationService.Navigate(new Uri("/Profile.xaml?item=2", UriKind.Relative));
			}
			else
			{
				this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
			}
		}

		private async void fav_star_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			int index;

			string venue, address, likes, price, contact, category, id;
			if (cafeToggle.IsChecked == false)
			{
				if (main.SelectedItem == main.Items[1])
				{
					index = cafelist.SelectedIndex;
					id = App.ViewModel.Items[index].Id;
					venue = App.ViewModel.Items[index].LineOne;
					address = App.ViewModel.Items[index].LineTwo;
					likes = App.ViewModel.Items[index].LineThree;
					price = App.ViewModel.Items[index].Price;
					contact = App.ViewModel.Items[index].Contact;
					category = "/Assets/coffee.png";
				}
				else
				{
					index = barlist.SelectedIndex;
					id = App.ViewModel.Bar[index].Id;
					venue = App.ViewModel.Bar[index].LineOne;
					address = App.ViewModel.Bar[index].LineTwo;
					likes = App.ViewModel.Bar[index].LineThree;
					price = App.ViewModel.Bar[index].Price;
					contact = App.ViewModel.Bar[index].Contact;
					category = "/Assets/beer.png";
				}
			}
			else
			{
				index = barlist.SelectedIndex;
				id = App.ViewModel.Bar[index].Id;
				venue = App.ViewModel.Bar[index].LineOne;
				address = App.ViewModel.Bar[index].LineTwo;
				likes = App.ViewModel.Bar[index].LineThree;
				price = App.ViewModel.Bar[index].Price;
				contact = App.ViewModel.Bar[index].Contact;
				category = "/Assets/beer.png";
			}
			if (Login.appSettings.Contains("user") && Login.appSettings["user"].ToString() != "")
			{
				Favorites todoItem = new Favorites
				{
					VenueId = id,
					User = Login.appSettings["user"].ToString(),
					Venue = venue,
					Address = address,
					Likes = likes,
					Price = price,
					Contact = contact,
					Category = category
				};

				if (Login.appSettings.Contains("count"))
				{
					int cnt = Convert.ToInt32(Login.appSettings["count"]);
					Login.appSettings["count"] = (cnt + 1).ToString();
				}
				else
				{
					Login.appSettings.Add("count", "1");
				}

				await Favorites.InsertAsync(todoItem);

				ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
				CustomMessageBox mbox = new CustomMessageBox()
				{
					Caption = "Success",
					Message = "Voila! That's added to your favourites.",
					Background = img,
					LeftButtonContent = "Great"
				};
				mbox.Show();
				try
				{
					await App.PersonalizedViewModel.LoadData();
				}
				catch (Exception ex)
				{
					//ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
					CustomMessageBox mb = new CustomMessageBox()
					{
						Caption = "Error",
						Message = "Snap! The internet is snapping us.",
						Background = img,
						LeftButtonContent = "OK"
					};
					mb.Show();
				}
			}
			else
			{
				this.NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
			}
		}

		private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
		}

		

		
		

		
	}
}
