﻿using BoomRadio.Model;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : StackLayout, IUpdatableUI
    {
        string coverImageUri;
        MainPage MainPage;
        MediaPlayer MediaPlayer { get; set; }
        

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
            PlayerFrame.BackgroundColor = Theme.GetColour("background");
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
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            if (MainPage.HasInternet())
            {
                MediaPlayer.Play();
                MainPage.UpdatePlayerUIs();
            }
        }

        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.Pause();
            MainPage.UpdatePlayerUIs();
        }

        private void LiveButton_Clicked(object sender, EventArgs e)
        {
            if (MainPage.HasInternet())
            {
                MediaPlayer.PlayLive();
                MainPage.UpdatePlayerUIs();
            }
        }

        public void SetHorizontalDisplay()
        {
            Grid.SetRow(InnerStackLayout, 0);
            Grid.SetColumn(InnerStackLayout, 1);
        }
        public void SetVerticalDisplay()
        {
            Grid.SetRow(InnerStackLayout, 1);
            Grid.SetColumn(InnerStackLayout, 0);

        }

        private void VisitWebsiteButton_Clicked(object sender, EventArgs e) => Launcher.OpenAsync("https://www.boomradio.com.au/");

    }
}