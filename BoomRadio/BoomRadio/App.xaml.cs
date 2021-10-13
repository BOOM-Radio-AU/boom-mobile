using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Play the live strean if autoplay is set, and there is an internet connection
            if (Preferences.Get("autoplay", false) && ((MainPage)MainPage).HasInternet(true))
            {
                ((MainPage)MainPage).MediaPlayer.PlayLive();
                ((MainPage)MainPage).UpdatePlayerUIs();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            // Play the live strean if autoplay is set, there is an internet connection, and not currently playing
            if (
                Preferences.Get("autoplay", false) &&
                ((MainPage)MainPage).HasInternet(true) &&
                !((MainPage)MainPage).MediaPlayer.IsPlaying
            )
            {
                ((MainPage)MainPage).MediaPlayer.PlayLive();
                ((MainPage)MainPage).UpdatePlayerUIs();
            }
        }
    }
}
