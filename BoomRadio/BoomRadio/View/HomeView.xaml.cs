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
        LiveStreamTrack trackInfo;
        string coverImageUri;
        MainPage MainPage;
        MediaPlayer MediaPlayer { get; set; }
        

        public HomeView(MediaPlayer mediaPlayer, MainPage mainPage)
        {
            InitializeComponent();
            MediaPlayer = mediaPlayer;
            MainPage = mainPage;
            IsPlaying = false;
            trackInfo = new LiveStreamTrack();
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

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.Play();
            MainPage.UpdatePlayerUIs();
            PlayButton.IsVisible = false;
            PauseButton.IsVisible = true;
            

        }

        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            MediaPlayer.Pause();
            MainPage.UpdatePlayerUIs();
            PlayButton.IsVisible = true;
            PauseButton.IsVisible = false;
        }

    }
}