using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.View
{
    /// <summary>
    /// View for the bottom/pull-up media player
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayerView : Frame
    {

        bool PlayerExpanded = false;
        public MediaPlayer MediaPlayer { get; set; }
        public MainPage MainPage;

        /// <summary>
        /// Constructor
        /// </summary>
        public MediaPlayerView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the UI
        /// </summary>
        public void UpdateUI()
        {
            // Update orientation
            if (PlayerExpanded)
            {
                ButtonsStackLayout.Orientation = StackOrientation.Vertical;
                // Move everthing into the middle column, in separate rows
                Grid.SetColumn(CoverImage, 1);

                Grid.SetRow(TrackDetailsStack, 1);

                Grid.SetColumn(ButtonsStackLayout, 1);
                Grid.SetRow(ButtonsStackLayout, 2);
            }
            else
            {
                ButtonsStackLayout.Orientation = StackOrientation.Horizontal;
                // Move everything into the top row, in separate columns
                Grid.SetColumn(CoverImage, 0);

                Grid.SetRow(TrackDetailsStack, 0);

                Grid.SetColumn(ButtonsStackLayout, 2);
                Grid.SetRow(ButtonsStackLayout, 0);
            }
            //PlayerStackLayout.Orientation = PlayerExpanded ? StackOrientation.Vertical : StackOrientation.Horizontal;

            // Update track info
            CoverImage.Source = ImageSource.FromUri(new Uri(MediaPlayer.CoverURI));
            ArtistLabel.Text = MediaPlayer.Artist;
            TrackTitleLabel.Text = MediaPlayer.Title;

            // Resize image
            CoverImage.ScaleTo(PlayerExpanded ? 1.5 : 1, 100, Easing.Linear);
            CoverImage.WidthRequest = PlayerExpanded ? 150 : 60;
            CoverImage.HeightRequest = PlayerExpanded ? 150 : 60;
            CoverImage.Margin = PlayerExpanded ? new Thickness(10, 50, 10, 50) : new Thickness(10, 5);

            // Resize and reposition labels
            ArtistLabel.HorizontalOptions = PlayerExpanded ? LayoutOptions.CenterAndExpand : LayoutOptions.StartAndExpand;
            ArtistLabel.FontSize = PlayerExpanded ? 20 : 15;
            TrackTitleLabel.HorizontalOptions = PlayerExpanded ? LayoutOptions.CenterAndExpand : LayoutOptions.StartAndExpand;
            TrackTitleLabel.FontSize = PlayerExpanded ? 20 : 15;

            //Resize Buttons
            PlayButton.FontSize = PlayerExpanded ? 50 : 25;
            PauseButton.FontSize = PlayerExpanded ? 50 : 25;

            // Show or hide buttons
            PlayButton.IsVisible = !MediaPlayer.IsPlaying;
            PauseButton.IsVisible = MediaPlayer.IsPlaying;

            if (PlayerExpanded) // Live button is for expanded view only}
            {
                LiveButton.IsVisible = MediaPlayer.CanGoLive();
            }
            else
            {
                LiveButton.IsVisible = false;
            }

            UpdateColours();
        }

        /// <summary>
        /// Updates the colors used
        /// </summary>
        internal void UpdateColours()
        {
            PlayerFrame.BackgroundColor = Theme.GetColour("player-bg");
            ArtistLabel.TextColor = Theme.GetColour("text");
            TrackTitleLabel.TextColor = Theme.GetColour("text");
            PlayButton.TextColor = Theme.GetColour("accent");
            PauseButton.TextColor = Theme.GetColour("accent");
            LiveButton.TextColor = Theme.GetColour("accent");
        }

        /// <summary>
        /// Exapnds the player to full size
        /// </summary>
        private void ExpandPlayer()
        {
            if (!PlayerExpanded)
            {

                PlayerFrame.TranslateTo(0, 0, 200, Easing.Linear);
                PlayerExpanded = true;
                PauseButton.Margin = 0;
                PlayButton.Margin = 0;
                
                Task.Delay(100).ContinueWith(_ =>
                {
                    Device.BeginInvokeOnMainThread(() => MainPage.UpdatePlayerUIs()); // The UI can only be updated from the main thread
                });
            }
        }

        /// <summary>
        /// Collapses the player to a small bar
        /// </summary>
        private void CollapsePlayer()
        {
            if (PlayerExpanded)
            {
                Thickness spacing = new Thickness(0, 0, 60, 0);
                PlayerFrame.TranslateTo(0, 420, 200, Easing.Linear);
                PlayerExpanded = false;
                PauseButton.Margin = spacing;
                PlayButton.Margin = spacing;
           

                Task.Delay(100).ContinueWith(_ =>
                {
                    Device.BeginInvokeOnMainThread(() => MainPage.UpdatePlayerUIs()); // The UI can only be updated from the main thread
                });
            }
        }

        /// <summary>
        /// Handles swipe-up events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlayerSwipedUp(object sender, SwipedEventArgs e)
        {
            ExpandPlayer();
        }

        /// <summary>
        /// Handles swipe-down events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlayerSwipedDown(object sender, SwipedEventArgs e)
        {
            CollapsePlayer();
        }

        /// <summary>
        /// Handles play button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            if (MainPage.HasInternet())
            {
                MediaPlayer.Play();
                MainPage.UpdatePlayerUIs();
            }
        }

        /// <summary>
        /// Handles pause button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.Pause();
            MainPage.UpdatePlayerUIs();
        }

        /// <summary>
        /// Handles "go live" button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LiveButton_Clicked(object sender, EventArgs e)
        {
            if (MainPage.HasInternet())
            {
                MediaPlayer.PlayLive();
                MainPage.UpdatePlayerUIs();
            }
        }
    }
}