using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SettingsPageAnimation.Resources;
// Isolated Storage
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace SettingsPageAnimation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        private async Task WriteToFile()
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes("Last Visit");

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync("DataFile.txt",
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        private async Task LoadRecentLocation()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Create a new folder name DataFolder. Or Get the DataFolder folder.
                var dataFolder = await local.CreateFolderAsync("DataFolder",
                    CreationCollisionOption.OpenIfExists);

                // Create a new DataFile.txt or Get DataFile.
                var tempfile = await dataFolder.CreateFileAsync("DataFile.txt",CreationCollisionOption.OpenIfExists);
                var file = await dataFolder.OpenStreamForReadAsync("DataFile.txt");
                   //Read the data.
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                      this.Items.Add(new ItemViewModel() { ID = "0", LineOne = streamReader.ReadToEnd(), LineTwo = "recent location", LineThree = "Parking Details Here" });
                    }
            } else {
                // Not sure how to get Windows.Storage.ApplicationData.Current.LocalFolder = null
                 this.Items.Add(new ItemViewModel() { ID = "0", LineOne = "recent location", LineTwo = "recent location", LineThree = "Parking Details Here" });
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
                    await LoadRecentLocation();
                    this.Items.Add(new ItemViewModel() { ID = "1", LineOne = "NP4 P1", LineTwo = "T-Mobile Newport 4 P1", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "2", LineOne = "NP4 P2", LineTwo = "T-Mobile Newport 4 P2", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "3", LineOne = "NP4 P3", LineTwo = "T-Mobile Newport 4 P3", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "4", LineOne = "NP4 P4", LineTwo = "T-Mobile Newport 4 P4", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "5", LineOne = "NP4 P5", LineTwo = "T-Mobile Newport 4 P5", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "6", LineOne = "NP4 P6", LineTwo = "T-Mobile Newport 4 P6", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "7", LineOne = "NP4 P7", LineTwo = "T-Mobile Newport 4 P7", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "8", LineOne = "NP4 P8", LineTwo = "T-Mobile Newport 4 P8", LineThree = "Parking Details Here" });
                    this.Items.Add(new ItemViewModel() { ID = "9", LineOne = "NP4 P9", LineTwo = "T-Mobile Newport 4 P8", LineThree = "Parking Details Here" });
                    this.IsDataLoaded = true;           
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}