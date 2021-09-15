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
    public partial class HomeView : StackLayout
    {
        // TODO: These should be properties of a model object, rather than this view
        bool IsPlaying;
        Track trackInfo;
        string coverImageUri;
        MainPage MainPage;
        MediaPlayer MediaPlayer { get; set; }
        

        public HomeView(MediaPlayer mediaPlayer, MainPage mainPage)
        {
            InitializeComponent();
            MediaPlayer = mediaPlayer;
            MainPage = mainPage;
            IsPlaying = false;
            trackInfo = new Track();
            //StatusLabel.Text = "Not yet playing";
        }

        public void UpdateUI()
        {
            ArtistLabel.Text = MediaPlayer.Artist;
            TrackTitleLabel.Text = MediaPlayer.Track;
            if (coverImageUri != MediaPlayer.CoverURI)
            {
                coverImageUri = MediaPlayer.CoverURI;
                CoverArtImage.Source = ImageSource.FromUri(new Uri(coverImageUri));
            }
        }
        private async void UpdateTrackInfo()
        {
            await trackInfo.Update();
            Device.BeginInvokeOnMainThread(() => UpdateUI()); // The UI can only be updated from the main thread
        }

        private void KeepTrackInfoUpdated()
        {
            UpdateTrackInfo();
            Device.StartTimer(TimeSpan.FromSeconds(15), () =>
            {
                UpdateTrackInfo();
                return IsPlaying;
            });
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

    }
}