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

        public HomeView()
        {
            InitializeComponent();
            IsPlaying = false;
            trackInfo = new Track();
            //StatusLabel.Text = "Not yet playing";
        }
        private void UpdateUI()
        {
            StatusLabel.Text = IsPlaying ? "Now playing" : "Stopped playing";
            ArtistLabel.Text = trackInfo.Artist;
            SongTitleLabel.Text = trackInfo.Title;
            if (coverImageUri != trackInfo.ImageUri)
            {
                coverImageUri = trackInfo.ImageUri;
                CoverImage.Source = ImageSource.FromUri(new Uri(coverImageUri));
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
            DependencyService.Get<IStreaming>().Play();
            IsPlaying = true;
            UpdateUI();
            KeepTrackInfoUpdated();
        }



        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IStreaming>().Pause();
            IsPlaying = false;
            UpdateUI();
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IStreaming>().Stop();
            IsPlaying = false;
            UpdateUI();
        }


    }
}