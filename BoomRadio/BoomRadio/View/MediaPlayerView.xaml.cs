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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayerView : Frame
    {

        bool PlayerExpanded = false;
        public MediaPlayer MediaPlayer { get; set; }
        public MainPage MainPage;

        public MediaPlayerView()
        {
            InitializeComponent();
        }

        public void UpdateUI()
        {
            // Update orientation
            PlayerStackLayout.Orientation = PlayerExpanded ? StackOrientation.Vertical : StackOrientation.Horizontal;

            // Update track info
            CoverImage.Source = ImageSource.FromUri(new Uri(MediaPlayer.CoverURI));
            ArtistLabel.Text = MediaPlayer.Artist;
            TrackTitleLabel.Text = MediaPlayer.Track;

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

        private void OnPlayerSwipedUp(object sender, SwipedEventArgs e)
        {
            ExpandPlayer();
        }
        private void OnPlayerSwipedDown(object sender, SwipedEventArgs e)
        {
            CollapsePlayer();
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.Play();
            MainPage.UpdatePlayerUIs();
        }
        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.Pause();
            MainPage.UpdatePlayerUIs();
        }

        private void LiveButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.PlayLive();
            MainPage.UpdatePlayerUIs();
        }
    }
}