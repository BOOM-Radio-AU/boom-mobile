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
        public MediaPlayerView()
        {
            InitializeComponent();
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Update orientation
            PlayerStackLayout.Orientation = PlayerExpanded ? StackOrientation.Vertical : StackOrientation.Horizontal;
            // Resize image
            CoverImage.ScaleTo(PlayerExpanded ? 1.5 : 1, 100, Easing.Linear);
            CoverImage.WidthRequest = PlayerExpanded ? 150 : 60;
            CoverImage.HeightRequest = PlayerExpanded ? 150 : 60;
            CoverImage.Margin = PlayerExpanded ? new Thickness(10, 50, 10, 50) : new Thickness(10, 5);
            // Resize and reposition labels
            ArtistLabel.HorizontalOptions = PlayerExpanded ? LayoutOptions.CenterAndExpand : LayoutOptions.StartAndExpand;
            ArtistLabel.FontSize = PlayerExpanded ? 20 : 15;
            SongTitleLabel.HorizontalOptions = PlayerExpanded ? LayoutOptions.CenterAndExpand : LayoutOptions.StartAndExpand;
            SongTitleLabel.FontSize = PlayerExpanded ? 20 : 15;
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
                Task.Delay(100).ContinueWith(_ =>
                {
                    Device.BeginInvokeOnMainThread(() => UpdateUI()); // The UI can only be updated from the main thread
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
                PlayerFrame.TranslateTo(0, 430, 200, Easing.Linear);
                PlayerExpanded = false;
                Task.Delay(100).ContinueWith(_ =>
                {
                    Device.BeginInvokeOnMainThread(() => UpdateUI()); // The UI can only be updated from the main thread
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
    }
}