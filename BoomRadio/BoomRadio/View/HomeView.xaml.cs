using BoomRadio.Components;
using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.View
{
    /// <summary>
    /// View for the home page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : StackLayout, IUpdatableUI
    {
        string coverImageUri;
        MainPage MainPage;
        DateTime contestsLastFetched = DateTime.MinValue;

        MediaPlayer MediaPlayer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediaPlayer">Media player</param>
        /// <param name="mainPage">Main (parent) page</param>
        public HomeView(MediaPlayer mediaPlayer, MainPage mainPage)
        {
            InitializeComponent();
            MediaPlayer = mediaPlayer;
            MainPage = mainPage;
        }

        /// <summary>
        /// Updates the state of UI elements. Should not be called directly, rather call
        /// MainPage.UpdatePlayerUIs (and that method will call this, as well as updating
        /// the UI of the MediaPlayerView)
        /// </summary>
        public void UpdateUI()
        {
            ArtistLabel.Text = MediaPlayer.Artist;
            TrackTitleLabel.Text = MediaPlayer.Title;
            if (coverImageUri != MediaPlayer.CoverURI)
            {
                coverImageUri = MediaPlayer.CoverURI;
                CoverArtImage.Source = ImageSource.FromUri(new Uri(coverImageUri));
            }
            PlayButton.IsVisible = !MediaPlayer.IsPlaying;
            PauseButton.IsVisible = MediaPlayer.IsPlaying;

            PlayButton.FontSize = 35;
            PauseButton.FontSize = 35;

            bool showLiveButton = MediaPlayer.CanGoLive();
            LiveButton.Opacity = showLiveButton ? 1 : 0;
            LiveButton.IsEnabled = showLiveButton;

            // Update colours
            PlayerFrame.BackgroundColor = Theme.GetColour("accent");
            PlayerFrameInner.BackgroundColor = Theme.GetColour("background");
            ArtistLabel.TextColor = Theme.GetColour("text");
            TrackTitleLabel.TextColor = Theme.GetColour("text");
            PlayButton.TextColor = Theme.GetColour("accent");
            PauseButton.TextColor = Theme.GetColour("accent");
            LiveButton.TextColor = Theme.GetColour("accent");
            EventsHeadingBox.UpdateColours();
            EventsStackLayout.BackgroundColor = Theme.GetColour("background");
            EventsLabel.TextColor = Theme.GetColour("text");
            VisitWebsiteButton.BackgroundColor = Theme.GetColour("accent");
            VisitWebsiteButton.TextColor = Color.White;

            // If contests not yet fetched, or not fetched recently, update from api
            if (contestsLastFetched == null || DateTime.Now - contestsLastFetched > TimeSpan.FromMinutes(10))
            {
                Task.Run(UpdateContestsAsync);
            }
            else // just update the colours
            {
                foreach (var contestItem in ContestsStackLayout.Children)
                {
                    (contestItem as ContestFrame)?.UpdateColours();
                }
            }
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

        /// <summary>
        /// Sets the view for horizontal display
        /// </summary>
        public void SetHorizontalDisplay()
        {
            Grid.SetRow(InnerStackLayout, 0);
            Grid.SetColumn(InnerStackLayout, 1);
        }

        /// <summary>
        /// Sets the view for vertical display
        /// </summary>
        public void SetVerticalDisplay()
        {
            Grid.SetRow(InnerStackLayout, 1);
            Grid.SetColumn(InnerStackLayout, 0);

        }

        /// <summary>
        /// Handles "Visit website" button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisitWebsiteButton_Clicked(object sender, EventArgs e) => Launcher.OpenAsync("https://www.boomradio.com.au/");

        /// <summary>
        /// Updates the contests from the api, asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task UpdateContestsAsync()
        {
            contestsLastFetched = DateTime.Now;
            List<Contest> contests = await Api.GetContestsAsync();
            if (contests.Count == 0) return;

            Device.BeginInvokeOnMainThread(() =>
            {
                ContestsStackLayout.Children.Clear();
                foreach (Contest contest in contests)
                {
                    ContestFrame item = new ContestFrame(contest, MainPage);

                    ContestsStackLayout.Children.Add(item);
                }
            });
        }
    }
}