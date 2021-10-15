using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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
            // Set up analytics
            AppCenter.Start("android=85a3bb9e-b369-4680-a07e-196d580cca11;" +
                  "ios=a580b641-58bf-4ba5-a97e-027a52265b41",
                  typeof(Analytics), typeof(Crashes));

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
