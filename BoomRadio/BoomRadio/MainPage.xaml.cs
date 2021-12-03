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
    /// <summary>
    /// Main and only page for the app. Views are loaded into the content area when navigating.
    /// </summary>
    public partial class MainPage : ContentPage
    {

        Dictionary<string, Lazy<Layout>> Views = new Dictionary<string, Lazy<Layout>>();
        string CurrentView;
        bool MenuShown = false;
        public readonly MediaPlayer MediaPlayer = new MediaPlayer();
        bool UpdateTrackTimerRunning = false;
        NewsCollection News = new NewsCollection();
        ShowsCollection Show = new ShowsCollection();
        SponsorsCollection Sponsor = new SponsorsCollection();
        IStatusBarStyler statusBarStyler;
        bool orientationIsHorizontal = false;
        Thickness safeAreaInsets;
        bool safeAreaIsFromVerticalOreintation = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            MediaPlayerView.MediaPlayer = MediaPlayer;
            MediaPlayerView.MainPage = this;
            statusBarStyler = DependencyService.Get<IStatusBarStyler>();
            // Set initial status bar colors and logo
            if (Theme.UseDarkMode)
            {
                statusBarStyler.SetDarkTheme();
                LogoImage.Source = ImageSource.FromFile("boomlogowhite.png");
            } else
            {
                statusBarStyler.SetLightTheme();
                LogoImage.Source = ImageSource.FromFile("boomlogoblack.png");
            }
            // Initialise views to load into content area
            Views["home"] = new Lazy<Layout>(() => new HomeView(MediaPlayer, this));
            Views["shows"] = new Lazy<Layout>(() => new ShowsView(Show, this));
            Views["news"] = new Lazy<Layout>(() => new NewsView(News, this));
            Views["news_article"] = new Lazy<Layout>(() => new NewsArticleView(this));
            Views["about"] = new Lazy<Layout>(() => new AboutView(Sponsor));
            Views["contact"] = new Lazy<Layout>(() => new ContactView());
            Views["settings"] = new Lazy<Layout>(() => new SettingsView(this));
            CurrentView = "home";
            Navigate("home");
            UpdateUI();
            UpdatePlayerUIs();
            // Handle changes to device theme (dark/light mode)
            Xamarin.Forms.Application.Current.RequestedThemeChanged += RequestedThemeChanged;
            // Check and apply initial orientation
            Task.Run(ApplyInitialOrienatationAsync);
        }

        /// <summary>
        /// Handles changes to the requested theme (device dark/light mode) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void RequestedThemeChanged(object sender, AppThemeChangedEventArgs args)
        {
            Theme.UpdateFromPreferences();
            UpdateUI();
            UpdatePlayerColours();
        }


        /// <summary>
        /// Sets up the page when it appears - calculates the safe area offset to use, if needed
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            safeAreaInsets = On<iOS>().SafeAreaInsets();
            if (Width > 0 && Height > 0)
            {
                SetSafeOffsets(Width > Height);
            }
        }

        /// <summary>
        /// Calculates whether the device is initially orientated vertically or horizontally
        /// (which affects how iOS safe area offsets are reported), and applies that orientation
        /// </summary>
        /// <returns></returns>
        private async Task ApplyInitialOrienatationAsync()
        {
            // Wait until width and height are allocated
            while (Width <=0 || Height <= 0)
            {
                await Task.Delay(10);
            }
            safeAreaIsFromVerticalOreintation = Width < Height;
            Device.BeginInvokeOnMainThread(() => OnSizeAllocated(Width, Height));
        }

        /// <summary>
        /// Calculates and applies the safe area offset to use, if needed
        /// </summary>
        /// <param name="isCurrentlyHorizontal">Orientation is currently horizontal</param>
        private void SetSafeOffsets(bool isCurrentlyHorizontal)
        {

            // Calculate a thickness where horizontal and vertical values are switched.
            // We don't know if the physical device rotation was clockwise or anti-clockwise, so get the
            // max of the initial top/bottom and left/right values.
            Thickness rotatedSafeAreaInsets = new Thickness(
                Math.Max(safeAreaInsets.Top, safeAreaInsets.Bottom),
                Math.Max(safeAreaInsets.Left, safeAreaInsets.Right)
            );

            // Instatiate a single all-zero thickness that can be reused where needed
            Thickness zeroThickness = new Thickness(0);


            // The safe area offsets iOS reports depeneds on the initial device orientation. So the actual offsets se need
            // to use depends both on the current orientation and initial orientation.
            Thickness safeinsets;
            if (isCurrentlyHorizontal && safeAreaIsFromVerticalOreintation)
            {
                safeinsets = rotatedSafeAreaInsets;
            }
            else if (isCurrentlyHorizontal) // and safe area is from horizontal orientation
            {
                safeinsets = new Thickness(safeAreaInsets.Left, safeAreaInsets.Top, safeAreaInsets.Right, safeAreaInsets.Bottom);
            } else if (safeAreaIsFromVerticalOreintation) // and the orientation is currently vertical
            {
                safeinsets = new Thickness(0, safeAreaInsets.Top, 0, safeAreaInsets.Bottom);
            }
            else // safe area is from horizontal orientation, and the orientation is currently vertical
            {
                safeinsets = new Thickness(0, rotatedSafeAreaInsets.Top, 0, rotatedSafeAreaInsets.Bottom); ;
            }

            if (isCurrentlyHorizontal)
            {
                // Apply offsets within content area
                ContentAreaScrollView.Padding = new Thickness(safeinsets.Left, 0, safeinsets.Right, 0);
                // Revert the offsets needed when vertical
                BottomBarGrid.Padding = zeroThickness;
                safeinsets.Bottom = 0;
                safeinsets.Top = 0;
                Padding = zeroThickness;
            }
            else // Is Vertical
            {
                // Add some padding to the bottom bar if needed (accounts for rounded screen corners)
                if (safeinsets.Bottom > 0)
                {
                    BottomBarGrid.Padding = new Thickness(10, 0, 10, 20);
                }
                // Apply top-padding to the page to ensure the top bar controls/logo are within the safe area
                safeinsets.Bottom = 0;
                Padding = safeinsets;
                // Revert the offsets needed when horizontal
                ContentAreaScrollView.Padding = zeroThickness;
            }
        }

        /// <summary>
        /// Update's the media player view's colours
        /// </summary>
        internal void UpdatePlayerColours()
        {
            MediaPlayerView.UpdateColours();
        }

        /// <summary>
        /// Checks for an internet connection, and shows a popup message if not connected
        /// (unless supressed)
        /// </summary>
        /// <param name="supressPopup">Set to true to supress showing the popup</param>
        /// <returns>Internet connection is available</returns>
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

        /// <summary>
        /// Display an alert popup
        /// </summary>
        /// <param name="title">Alert title</param>
        /// <param name="message">Alert message</param>
        /// <param name="cancel">Text for the button to dismiss the laert</param>
        /// <returns></returns>
        public async Task DisplayAlertAsync(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        /// <summary>
        /// Updates all the UIs related to media playback 
        /// </summary>
        public void UpdatePlayerUIs()
        {
            if (!UpdateTrackTimerRunning && MediaPlayer.IsLive)
            {
                StartUpdateTrackTimer();
            }
            MediaPlayerView.UpdateUI();
            PlayPauseTabIcon.Text = MediaPlayer.IsPlaying ? "Pause" : "Play";
            PlayPauseTabText.Text = MediaPlayer.IsPlaying ? "Pause" : "Play";
            if (CurrentView == "home") ((HomeView)Views["home"].Value).UpdateUI();
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

            if(target == "about")
            {
                Views["about"] = new Lazy<Layout>(()=> new AboutView(Sponsor));
            }
            ContentAreaScrollView.ScrollToAsync(0, 0, false);
            ContentAreaScrollView.Content = Views[target].Value;
            CurrentView = target;
            (Views[target].Value as IUpdatableUI)?.UpdateUI();
            if (orientationIsHorizontal)
            {
                (Views[target].Value as IUpdatableUI)?.SetHorizontalDisplay();
            } else
            {
                (Views[target].Value as IUpdatableUI)?.SetVerticalDisplay();
            }
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
            ((NewsArticleView)Views["news_article"].Value).Article = article;
            Navigate("news_article");
            Analytics.TrackEvent("read_article", new Dictionary<string, string>{
                { "title", article.Title }
            });
        }

        /// <summary>
        /// Updates the page UI
        /// </summary>
        public void UpdateUI()
        {
            // Update bottom bar/tabs
            BottomBarGrid.BackgroundColor = Theme.GetColour("nav-bg");
            PlayPauseTabIcon.TextColor = Theme.GetColour("text");
            PlayPauseTabText.TextColor = Theme.GetColour("text");
            HomeText.TextColor = CurrentView == "home"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            HomeIcon.TextColor = CurrentView == "home"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            ShowsText.TextColor = CurrentView == "shows" ? Theme.GetColour("accent") : Theme.GetColour("text");
            ShowsIcon.TextColor = CurrentView == "shows" ? Theme.GetColour("accent") : Theme.GetColour("text");
            NewsText.TextColor = CurrentView == "news"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            NewsIcon.TextColor = CurrentView == "news"   ? Theme.GetColour("accent") : Theme.GetColour("text");
            // Update top bar colours
            this.BackgroundColor = Theme.GetColour("nav-bg");
            TopBarGrid.BackgroundColor = Theme.GetColour("nav-bg");
            MenuIcon.TextColor = Theme.GetColour("text");
            LiveIcon.TextColor = MediaPlayer.IsLive ? Theme.GetColour("is-live") : Theme.GetColour("not-live");
            LiveText.TextColor = MediaPlayer.IsLive ? Theme.GetColour("is-live") : Theme.GetColour("not-live");
            // Update menu colours
            MenuFrame.BackgroundColor = Theme.GetColour("background");
            AboutMenuItem.TextColor = Theme.GetColour("text");
            ContactMenuItem.TextColor = Theme.GetColour("text");
            SettingsMenuItem.TextColor = Theme.GetColour("text");
            
            if (!statusBarStyler.IsDarkTheme() && Theme.UseDarkMode)
            {
                statusBarStyler.SetDarkTheme();
                LogoImage.Source = ImageSource.FromFile("boomlogowhite.png");
            }
            else if (statusBarStyler.IsDarkTheme() && !Theme.UseDarkMode)
            {
                statusBarStyler.SetLightTheme();
                LogoImage.Source = ImageSource.FromFile("boomlogoblack.png");
            }
        }

        /// <summary>
        /// Handles home tab clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeTab_Clicked(object sender, EventArgs e)
        {
            Navigate("home");
        }

        /// <summary>
        /// Handles shows tab clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowsTab_Clicked(object sender, EventArgs e)
        {
            Navigate("shows");
        }

        /// <summary>
        /// Handles news tab clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsTab_Clicked(object sender, EventArgs e)
        {
            Navigate("news");
        }

        /// <summary>
        /// Slides open the menu, if not already open
        /// </summary>
        private void OpenMenu()
        {
            if (!MenuShown)
            {
                MenuFrame.TranslateTo(0, 0, 400, Easing.Linear);
                ContentOverlayFrame.IsVisible = true;
                MenuShown = true;
                MenuFrame.HasShadow = true;
            }
        }

        /// <summary>
        /// Slides closed the menu, if not already closed
        /// </summary>
        private void CloseMenu()
        {
            if (MenuShown)
            {
                MenuFrame.TranslateTo(-200, 0, 200, Easing.Linear);
                ContentOverlayFrame.IsVisible = false;
                MenuShown = false;
                MenuFrame.HasShadow = false;
            }
        }

        /// <summary>
        /// Handles left-swipes on the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Swiped_Left(object sender, SwipedEventArgs e)
        {
            CloseMenu();
        }

        /// <summary>
        /// Handles taps on the menu icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handles clicks on the About menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenuItem_Clicked(object sender, EventArgs e)
        {
            Navigate("about");
        }

        /// <summary>
        /// Handles clicks on the Contact menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactMenuItem_Clicked(object sender, EventArgs e)
        {
            Navigate("contact");
        }

        /// <summary>
        /// Handles clicks on the Settings menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsMenuItem_Clicked(object sender, EventArgs e)
        {
            Navigate("settings");
        }

        /// <summary>
        /// Handles changes to page size, i.e. orientation changes
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called

            if (this.Width > this.Height)
            {
                // Horizontal orientation
                orientationIsHorizontal = true;
                MediaPlayerView.IsVisible = false;
                PlayPauseTab.IsVisible = true;
                MainGridRowThree.Height = 0;
                BottomBarGridColOne.Width = GridLength.Star;
                PlayPauseTabIcon.FontSize = 16;
                PlayPauseTabStack.Orientation = StackOrientation.Horizontal;
                PlayPauseTabStack.Margin = new Thickness(0, 0, 0, 10);
                HomeIcon.FontSize = 16;
                HomeTabStack.Orientation = StackOrientation.Horizontal;
                HomeTabStack.Margin = new Thickness(0, 0, 0, 10);
                ShowsIcon.FontSize = 16;
                ShowsTabStack.Orientation = StackOrientation.Horizontal;
                ShowsTabStack.Margin = new Thickness(0, 0, 0, 10);
                NewsIcon.FontSize = 16;
                NewsTabStack.Orientation = StackOrientation.Horizontal;
                NewsTabStack.Margin = new Thickness(0, 0, 0, 10);
                (Views[CurrentView].Value as IUpdatableUI)?.SetHorizontalDisplay();
            }
            else
            {
                orientationIsHorizontal = false;
                MediaPlayerView.IsVisible = true;
                PlayPauseTab.IsVisible = false;
                MainGridRowThree.Height = 80;
                BottomBarGridColOne.Width = 0;
                PlayPauseTabIcon.FontSize = 20;
                PlayPauseTabStack.Orientation = StackOrientation.Vertical;
                PlayPauseTabStack.Margin = new Thickness(0); 
                HomeIcon.FontSize = 20;
                HomeTabStack.Orientation = StackOrientation.Vertical;
                HomeTabStack.Margin = new Thickness(0);
                ShowsIcon.FontSize = 20;
                ShowsTabStack.Orientation = StackOrientation.Vertical;
                ShowsTabStack.Margin = new Thickness(0);
                NewsIcon.FontSize = 20;
                NewsTabStack.Orientation = StackOrientation.Vertical;
                NewsTabStack.Margin = new Thickness(0);
                (Views[CurrentView].Value as IUpdatableUI)?.SetVerticalDisplay();
            }
            SetSafeOffsets(orientationIsHorizontal);
        }

        /// <summary>
        /// Handles taps on the play/pause tab (shown in horizontal view)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayPauseTab_Tapped(object sender, EventArgs e)
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Play();
            }
            UpdatePlayerUIs();
        }

        /// <summary>
        /// Handles taps outside the menu, when it is opened 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            CloseMenu();
        }
    }
}
