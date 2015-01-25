using Microsoft.WindowsAzure.MobileServices;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cafaholic.ViewModels
{
    public class PersonalizationViewModel: INotifyPropertyChanged
    {
        public PersonalizationViewModel()
        {
            
            
        }
        private IMobileServiceTable<Favorites> Favorites = App.MobileService.GetTable<Favorites>();
        public MobileServiceCollection<Favorites, Favorites> favorites { get; private set; }
        public ObservableCollection<Bars> Bar { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData()
        {
           if (Login.appSettings.Contains("user"))
            {
                IMobileServiceTableQuery<Favorites> query = Favorites
                       .Where(todoItem => todoItem.User == Login.appSettings["user"].ToString());

                favorites = await query.ToCollectionAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
