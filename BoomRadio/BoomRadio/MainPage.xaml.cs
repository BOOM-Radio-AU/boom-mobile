using BoomRadio.Model;
using BoomRadio.View;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BoomRadio
{
    public partial class MainPage : ContentPage
    {

        Dictionary<string, Layout> Views = new Dictionary<string, Layout>();
        string CurrentView;
        bool MenuShown = false;
        public readonly MediaPlayer MediaPlayer = new MediaPlayer();
        bool UpdateTrackTimerRunning = false;
        NewsCollection News = new NewsCollection();
        ShowsCollection Show = new ShowsCollection();

        public MainPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            MediaPlayerView.MediaPlayer = MediaPlayer;
            MediaPlayerView.MainPage = this;
            // Initialise views to load into content area
            Views["home"] = new HomeView(MediaPlayer, this);
            Views["shows"] = new ShowsView(Show, this);
            Views["news"] = new NewsView(News, this);
            Views["news_article"] = new NewsArticleView(this);
            Views["about"] = new AboutView();
            Views["contact"] = new ContactView();
            Views["settings"] = new SettingsView(this);
            CurrentView = "home";
            Navigate("home");
            UpdateUI();
            UpdatePlayerUIs();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var safeinsets = On<iOS>().SafeAreaInsets();

            if (safeinsets.Bottom > 0)
            {
                Thickness Thick = new Thickness(10, 0, 10, 20);
                BottomBarGrid.Padding = Thick;
            }

            safeinsets.Bottom = 0;
            Padding = safeinsets;
        }

        internal void UpdatePlayerColours()
        {
            MediaPlayerView.UpdateColours();
        }

        /// <summary>
        /// Checks for an internet connection, and shows a popup message if not connected
        /// (unless supressed)
        /// </summary>
        /// <param name="supressPopup">Set to true to supress showing the popup</param>
        /// <returns></returns>
        public bool HasInternet(bool supressPopup = false)
        {
            // Check for internet
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return true;
            }

            // No internet - show a popup (if not supressed)
            if (!supressPopup)
            {
                DisplayAlertAsync("No Internet", "Connect to the internet and try again", "OK");
            }
            // Set Media player information
            MediaPlayer.SetNotConnected();
            UpdatePlayerUIs();

            return false;
        }


        public async Task DisplayAlertAsync(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        public void UpdatePlayerUIs()
        {
            if (!UpdateTrackTimerRunning && MediaPlayer.IsLive)
            {
                StartUpdateTrackTimer();
            }
            MediaPlayerView.UpdateUI();
            if (CurrentView == "home") ((HomeView)Views["home"]).UpdateUI();
            UpdateUI(); // Will update live icon/text colour if needed
        }

        /// <summary>
        /// Runs a timer to keep track information updated, as long as the media player
        /// is playing live
        /// </summary>
        private void StartUpdateTrackTimer()
        {
            UpdateTrackTimerRunning = true;
            Device.StartTimer(TimeSpan.FromSeconds(15), () =>
            {
                // Check if player is still playing live
                if (!MediaPlayer.IsLive)
                {
                    UpdateTrackTimerRunning = false;
                    return false; // ends timer
                }


                if (HasInternet(false))
                {
                    UpdateLiveTrackInfo();
                }
                return true; // continues timer
            });
        }

        /// <summary>
        /// Updates the media player's live track info, then updates the UI
        /// </summary>
        private async void UpdateLiveTrackInfo()
        {
            await MediaPlayer.UpdateLiveTrackInfo();
            Device.BeginInvokeOnMainThread(() => UpdatePlayerUIs());

        }

        /// <summary>
        /// Loads a view into the content area of the main page
        /// </summary>
        /// <param name="target">Name of view to load</param>
        public void Navigate(string target)
        {
            if (!Views.ContainsKey(target))
            {
                Exception ex = new Exception($"[Navigation error] View not found for '{target}'");
                DependencyService.Get<ILogging>().Error(this, ex);
                Navigate("home");
                return;
            }
            if (MenuShown)
            {
                CloseMenu();
            }
            ContentAreaScrollView.Content = Views[target];
            CurrentView = target;
            (Views[target] as IUpdatableUI)?.UpdateUI();
            UpdateUI();
            Analytics.TrackEvent("navigate", new Dictionary<string, string>{
                { "page", target }
            });
        }

        /// <summary>
        /// Sets the article for the news article view, and navigates to that view
        /// </summary>
        /// <param name="article">Article to be shown</param>
        public void NavigateToNewsArticle(NewsArticle article)
        {
            ((NewsArticleView)Views["news_article"]).Article = article;
            Navigate("news_article");
            Analytics.TrackEvent("read_article", new Dictionary<string, string>{
                { "title", article.Title }
            });
        }

        public void UpdateUI()
        {
            // Update bottom bar/tabs
            BottomBarGrid.BackgroundColor = Theme.GetColour("nav-bg");
            HomeText.TextColor = CurrentView == "home"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            HomeIcon.TextColor = CurrentView == "home"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            ShowsText.TextColor = CurrentView == "shows" ? Theme.GetColour("accent") : Theme.GetColour("text");
            ShowsIcon.TextColor = CurrentView == "shows" ? Theme.GetColour("accent") : Theme.GetColour("text");
            NewsText.TextColor = CurrentView == "news"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            NewsIcon.TextColor = CurrentView == "news"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            // Update top bar colours
            TopBarGrid.BackgroundColor = Theme.GetColour("nav-bg");
            MenuIcon.TextColor = Theme.GetColour("text");
            LiveIcon.TextColor = MediaPlayer.IsLive ? Theme.GetColour("is-live") : Theme.GetColour("not-live");
            LiveText.TextColor = MediaPlayer.IsLive ? Theme.GetColour("is-live") : Theme.GetColour("not-live");
            // Update menu colours
            MenuFrame.BackgroundColor = Theme.GetColour("background");
            AboutMenuItem.TextColor = Theme.GetColour("text");
            ContactMenuItem.TextColor = Theme.GetColour("text");
            SettingsMenuItem.TextColor = Theme.GetColour("text");

        }

        private void HomeTab_Clicked(object sender, EventArgs e)
        {
            Navigate("home");
        }

        private void ShowsTab_Clicked(object sender, EventArgs e)
        {
            Navigate("shows");
        }

        private void NewsTab_Clicked(object sender, EventArgs e)
        {
            Navigate("news");
        }

        private void OpenMenu()
        {
            if (!MenuShown)
            {
                MenuFrame.TranslateTo(0, 0, 400, Easing.Linear);
                MenuShown = true;
                MenuFrame.HasShadow = true;
            }
        }
        private void CloseMenu()
        {
            if (MenuShown)
            {
                MenuFrame.TranslateTo(-200, 0, 200, Easing.Linear);
                MenuShown = false;
                MenuFrame.HasShadow = false;
            }
        }

        private void Menu_Swiped_Left(object sender, SwipedEventArgs e)
        {
            CloseMenu();
        }

        private void MenuIcon_Tapped(object sender, EventArgs e)
        {
            if (MenuShown)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        private void AboutMenuItem_Clicked(object sender, EventArgs e)
        {
            Navigate("about");
        }

        private void ContactMenuItem_Clicked(object sender, EventArgs e)
        {
            Navigate("contact");
        }

        private void SettingsMenuItem_Clicked(object sender, EventArgs e)
        {
            Navigate("settings");
        }

    }
}
