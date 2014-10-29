using Microsoft.Phone.Controls;
using SettingsPageAnimation.Framework;
using SettingsPageAnimation.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Phone.Shell;

// Isolated Storage
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace SettingsPageAnimation
{
    public partial class MainPage : PhoneApplicationPage
    {
        private double _dragDistanceToOpen = 75.0;
        private double _dragDistanceToClose = 305.0;
        private double _dragDistanceNegative = -75.0;

        private FrameworkElement _feContainer;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            _feContainer = this.Container as FrameworkElement;
        }

        private async void Button_Click (object sender, RoutedEventArgs e)
        {
            String time = DateTime.Now.ToShortTimeString();
            this.ParkPlace.Text = this.ParkPlace.Text + " " + time;
            this.Result.Text = this.ParkPlace.Text;
            //btnFlipTile_Click(sender, e);
            //btnIconicTile_Click(sender, e);
            await WriteToFile();

            // Update UI.
            //this.btnWrite.IsEnabled = false;
            this.btnRead.IsEnabled = true;

        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            await ReadFile();

            // Update UI.
            //this.btnWrite.IsEnabled = true;
            //this.btnRead.IsEnabled = false;
        }

        private async Task ReadFile()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync("DataFolder");

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync("DataFile.txt");

                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    this.textBlock1.Text = streamReader.ReadToEnd();
                    this.ParkPlace.Text = streamReader.ReadToEnd();
                }

            }
        }

        void UpdateTitle()
        { 
             // This is the tile template 
            //string xmlString = @" 
            //<tile> 
                //<visual version =' 2'>
              //  <binding template =' TileSquare150x150Block' fallback =' TileSquareBlock' > 
               // < text id =' 1' > 25 </ text > 
               // < text id =' 2' > Live Tile </ text > 
               // </binding>
                //</visual>
            //</tile>";
            // Load the content into an XML document
            //XmlDocument document = new XmlDocument(); document.LoadXml(xmlString); 
            // Create a tile notification and send it 
            //TileNotification notification = new TileNotification(document); 
            //TileUpdateManager.CreateTileUpdaterForApplication(). Update( notification);
        }

        private void btnFlipTile_Click(object sender, RoutedEventArgs e)
        {
            // find the tile object for the application tile that using "flip" contains string in it.
            ShellTile oTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("flip".ToString()));


            if (oTile != null && oTile.NavigationUri.ToString().Contains("flip"))
            {
                FlipTileData oFliptile = new FlipTileData();
                oFliptile.Title = "park@ " + this.ParkPlace.Text;
                //oFliptile.Count = 11;
                oFliptile.BackTitle = "Updated Flip Tile";

                oFliptile.BackContent = "back of tile";
                oFliptile.WideBackContent = "back of the wide tile";

                //oFliptile.SmallBackgroundImage = new Uri("Assets/Tiles/Flip/159-159-006c.png", UriKind.Relative);
                oFliptile.SmallBackgroundImage = new Uri("Assets/Tiles/Flip/159x159.png", UriKind.Relative);
                oFliptile.BackgroundImage = new Uri("Assets/Tiles/Flip/336x336.png", UriKind.Relative);
                oFliptile.WideBackgroundImage = new Uri("Assets/Tiles/Flip/691x336.png", UriKind.Relative);

                oFliptile.BackBackgroundImage = new Uri("/Assets/Tiles/Flip/A336.png", UriKind.Relative);
                oFliptile.WideBackBackgroundImage = new Uri("/Assets/Tiles/Flip/A691.png", UriKind.Relative);
                oTile.Update(oFliptile);
                MessageBox.Show("Flip Tile Data successfully update.");
            }
            else
            {
                // once it is created flip tile
                Uri tileUri = new Uri("/MainPage.xaml?tile=flip", UriKind.Relative);
                ShellTileData tileData = this.CreateFlipTileData();
                ShellTile.Create(tileUri, tileData, true);
            }
        }

        private ShellTileData CreateFlipTileData()
        {
            return new FlipTileData()
            {
                Title = "park@ " + this.ParkPlace.Text,
                BackTitle = "BT" + "park@ " + this.ParkPlace.Text,
                BackContent = "BC" + "park@ " + this.ParkPlace.Text,
                WideBackContent = "WBC" + "park@ " + this.ParkPlace.Text,
                //Count = 8, 
                SmallBackgroundImage = new Uri("/Assets/Tiles/Flip/A159.png", UriKind.Relative),
                //SmallBackgroundImage = new Uri("/Assets/Tiles/Flip/159-159-006c.png", UriKind.Relative),
                BackgroundImage = new Uri("/Assets/Tiles/Flip/A336.png", UriKind.Relative),
                WideBackgroundImage = new Uri("/Assets/Tiles/Flip/A691.png", UriKind.Relative),
            };
        }

        private void btnIconicTile_Click(object sender, RoutedEventArgs e)
        {
            IconicTileData oIcontile = new IconicTileData();
            oIcontile.Title = "park@ " + this.ParkPlace.Text;
            //oIcontile.Count = 1;

            oIcontile.IconImage = new Uri("Assets/Tiles/Iconic/202x202.png", UriKind.Relative);
            oIcontile.SmallIconImage = new Uri("Assets/Tiles/Iconic/110x110.png", UriKind.Relative);

            oIcontile.WideContent1 = "park@" + this.ParkPlace.Text;
            oIcontile.WideContent2 = "park@" + this.ParkPlace.Text;
            oIcontile.WideContent3 = "park@" + this.ParkPlace.Text;

            oIcontile.BackgroundColor = System.Windows.Media.Colors.Orange;

            // find the tile object for the application tile that using "Iconic" contains string in it.
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Iconic".ToString()));

            if (TileToFind != null && TileToFind.NavigationUri.ToString().Contains("Iconic"))
            {
                TileToFind.Delete();
                ShellTile.Create(new Uri("/MainPage.xaml?id=Iconic", UriKind.Relative), oIcontile, true);
            }
            else
            {
                ShellTile.Create(new Uri("/MainPage.xaml?id=Iconic", UriKind.Relative), oIcontile, true);
            }

        }

        private async Task WriteToFile()
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(this.ParkPlace.Text.ToCharArray());
          
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


        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.LoadData();
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        private bool _isSettingsOpen = false;
        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            if (_isSettingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }

        private void CloseSettings()
        {
            var trans = _feContainer.GetHorizontalOffset().Transform;
            trans.Animate(trans.X, 0, TranslateTransform.XProperty, 300, 0, new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            });

            _isSettingsOpen = false;
        }

        private void OpenSettings()
        {
            var trans = _feContainer.GetHorizontalOffset().Transform;
            trans.Animate(trans.X, 380, TranslateTransform.XProperty, 300, 0, new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                });

            _isSettingsOpen = true;
        }

        private void GestureListener_OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
            {
                double offset = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;
                if (offset > _dragDistanceToOpen)
                    this.OpenSettings();
                else
                    _feContainer.SetHorizontalOffset(offset);
            }

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
            {
                double offsetContainer = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;
                if (offsetContainer < _dragDistanceToClose)
                    this.CloseSettings();
                else
                    _feContainer.SetHorizontalOffset(offsetContainer);
            }
        }

        private void GestureListener_OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
            {
                if (e.HorizontalChange < _dragDistanceToOpen)
                    this.ResetLayoutRoot();
                else
                    this.OpenSettings();
            }

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
            {
                if (e.HorizontalChange > _dragDistanceNegative)
                    this.ResetLayoutRoot();
                else
                    this.CloseSettings();
            }
        }

        private void SettingsStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ResetLayoutRoot();
        }

        private void ResetLayoutRoot()
        {
            if (!_isSettingsOpen)
                _feContainer.SetHorizontalOffset(0.0);
            else
                _feContainer.SetHorizontalOffset(380.0);
        }
    }
}