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
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using Facebook;
using System.Windows.Resources;

namespace Cafaholic
{
	public partial class Profile : PhoneApplicationPage
	{
		private IMobileServiceTable<Favorites> Favorites = App.MobileService.GetTable<Favorites>();
		private IMobileServiceTable<User> Users = App.MobileService.GetTable<User>();
		public Profile()
		{
			InitializeComponent();

			DataContext = null;
			DataContext = App.PersonalizedViewModel;
			favoriteslist.Items.Clear();
			favoriteslist.ItemsSource = App.PersonalizedViewModel.favorites;
			
		}

		

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Progress.IsVisible = true;
			Progress.IsIndeterminate = true;
			if (NavigationContext.QueryString.ContainsKey("fb"))
			{
				LoadUserInfo();

			}
			else
			{
				user_tb.Text = Login.appSettings["user"].ToString();
			}
			if (Login.appSettings.Contains("photo"))
			{
				upload.Visibility = Visibility.Collapsed;
			}
			await App.PersonalizedViewModel.LoadData();

			
			if (Login.appSettings.Contains("count"))
			{
				favs_tb.Text = Login.appSettings["count"].ToString();
			}
			else
			{
				favs_tb.Text = "0";
			}
			

			if (NavigationContext.QueryString.ContainsKey("item"))
			{
				var index = NavigationContext.QueryString["item"];
				var indexParsed = int.Parse(index);
				mypivot.SelectedIndex = indexParsed;
			}

			if (NavigationService.BackStack.Any())
			{
				var length = NavigationService.BackStack.Count() - 1;
				var i = 0;
				while (i < length)
				{
					NavigationService.RemoveBackEntry();
					i++;
				}
			}
			if (Login.appSettings.Contains("photo"))
			{
				try{
					getPhoto();
				}catch(Exception ex){
					ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
					CustomMessageBox mbox = new CustomMessageBox()
					{
						Caption = "Swoop!",
						Message = "The minions couldn't get the right photo.",
						Background = img,
						LeftButtonContent = "OK"
					};
					mbox.Show();
				}
			}
			Progress.IsVisible = false;
		}

		private async Task delete(Favorites f)
		{
			await Favorites.DeleteAsync(f);
		}
		public MobileServiceCollection<User, User> users { get; private set; }
		private void LoadUserInfo()
		{
			var fb = new FacebookClient(App.AccessToken);

			fb.GetCompleted += (o, e) =>
			{
				if (e.Error != null)
				{
					Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
					return;
				}

				var result = (IDictionary<string, object>)e.GetResultData();

				Dispatcher.BeginInvoke(async () =>
				{
					var profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", App.FacebookId, "large", App.AccessToken);

					this.Profile_Image.Source = new BitmapImage(new Uri(profilePictureUrl));
					this.user_tb.Text = String.Format("{0} {1}", (string)result["first_name"], (string)result["last_name"]);
					var email = result["email"];

					if (Login.appSettings.Contains("user")) {
						Login.appSettings["user"] = email.ToString();
					}
					else
						Login.appSettings.Add("user", email.ToString());
					User newuser = new User { Username = email.ToString(), Email = email.ToString() };
					IMobileServiceTableQuery<User> query = Users
			  .Where(todoItem => todoItem.Username == email.ToString());

				users = await query.ToCollectionAsync();
				if (users.Count == 0)
				{


					await Users.InsertAsync(newuser);
					
				}
				upload.Visibility = Visibility.Collapsed;
				WebClient client = new WebClient();
				client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
				client.OpenReadAsync(new Uri(profilePictureUrl.ToString()), client);
				});
			};

			fb.GetTaskAsync("me");
		}
		IsolatedStorageFile MyStore = IsolatedStorageFile.GetUserStoreForApplication();

		private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			var resInfo = new StreamResourceInfo(e.Result, null);
			var reader = new StreamReader(resInfo.Stream);
			byte[] contents;
			using (BinaryReader bReader = new BinaryReader(reader.BaseStream))
			{
				contents = bReader.ReadBytes((int)reader.BaseStream.Length);
			}
			IsolatedStorageFileStream stream = MyStore.CreateFile("image.jpg");
			stream.Write(contents, 0, contents.Length);
			stream.Close();
			if (Login.appSettings.Contains("photo"))
			{
				Login.appSettings.Remove("photo");
				Login.appSettings.Add("photo", "yes");
			}
			else
			{
				Login.appSettings.Add("photo", "yes");
			}
		}
		private void user_tb_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			double desiredHeight = 80;

			if (this.user_tb.ActualHeight > desiredHeight)
			{
				double fontsizeMultiplier = Math.Sqrt(desiredHeight / this.user_tb.ActualHeight);
				this.user_tb.FontSize = Math.Floor(this.user_tb.FontSize * fontsizeMultiplier);
			}

			this.user_tb.Height = desiredHeight;
		}

		private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
		   // this.NavigationService.Navigate(new Uri("/Profile.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
			
		}

		private async void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
		{
			await App.PersonalizedViewModel.LoadData();
		}

		private void ApplicationBarIconButton_Click(object sender, EventArgs e)
		{
			if (Login.appSettings.Contains("user")){
				Login.appSettings["user"] = "";
			}
			if (Login.appSettings.Contains("photo")){
				Login.appSettings.Remove("photo");
			}
			this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}

		private async void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
		{
			Progress.IsVisible = true;
			Progress.IsIndeterminate = true;
			Progress.Text = "Refreshing..";
			await App.PersonalizedViewModel.LoadData();
			this.favoriteslist.ItemsSource = App.PersonalizedViewModel.favorites;
			favs_tb.Text = App.PersonalizedViewModel.favorites.Count.ToString();
			Login.appSettings["count"] = App.PersonalizedViewModel.favorites.Count.ToString();
			//this.NavigationService.Navigate(new Uri("/Profile.xaml?" + DateTime.Now.Ticks, UriKind.Relative));
		}

		private async void Favorite_Click(object sender, RoutedEventArgs e)
		{
			int index = favoriteslist.Items.IndexOf(((sender as MenuItem).DataContext));
			string id = App.PersonalizedViewModel.favorites[index].Id;
			Favorites f = new Favorites { Id = id };
			await delete(f);
			ImageBrush img = new ImageBrush { ImageSource = new BitmapImage(new Uri("/Assets/cup.jpg", UriKind.Relative)), Opacity = 1, Stretch = Stretch.UniformToFill };
			CustomMessageBox mbox = new CustomMessageBox()
			{
				Caption = "Delete",
				Message = "Too bad! You don't like the place. We'll remove it soon.",
				Background = img,
				LeftButtonContent = "Thanks"
			};
			mbox.Show();
			int cnt = Convert.ToInt32(Login.appSettings["count"]);
			Login.appSettings["count"] = (cnt - 1).ToString();
			await App.PersonalizedViewModel.LoadData();
			Progress.IsVisible = true;
			Progress.IsIndeterminate = true;
			Progress.Text = "Refreshing..";
			await App.PersonalizedViewModel.LoadData();
			this.favoriteslist.ItemsSource = App.PersonalizedViewModel.favorites;
			favs_tb.Text = Login.appSettings["count"].ToString();
		}

		private void Profile_Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			
				
				PhotoChooserTask photoChooserTask = new PhotoChooserTask();
				photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
				photoChooserTask.Show();
			
		}

		private void getPhoto()
		{
			BitmapImage bi = new BitmapImage();

			using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read))
				{
					bi.SetSource(fileStream);
					this.Profile_Image.Height = 190;
					this.Profile_Image.Width = 190;
				}
			}
			this.Profile_Image.Source = bi;
			
			this.Profile_Image.Stretch = System.Windows.Media.Stretch.Fill;
		}

		string tempJPEG = "image.jpg";
		private void photoChooserTask_Completed(object sender, PhotoResult e)
		{
			using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (myIsolatedStorage.FileExists(tempJPEG))
				{
					myIsolatedStorage.DeleteFile(tempJPEG);
				}

				IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(tempJPEG);

				BitmapImage bitmap = new BitmapImage();
				bitmap.SetSource(e.ChosenPhoto);
				WriteableBitmap wb = new WriteableBitmap(bitmap);

				// Encode WriteableBitmap object to a JPEG stream.
				Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);

				//wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
				fileStream.Close();
			}
			if (Login.appSettings.Contains("photo"))
			{
				Login.appSettings.Remove("photo");
				Login.appSettings.Add("photo", "yes");
			}
			else
			{
				Login.appSettings.Add("photo", "yes");
			}
			upload.Visibility = Visibility.Collapsed;
		}

		
	}
}