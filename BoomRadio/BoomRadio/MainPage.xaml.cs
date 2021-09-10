using BoomRadio.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BoomRadio
{
    public partial class MainPage : ContentPage
    {

        Dictionary<string, StackLayout> Views = new Dictionary<string, StackLayout>();
        string CurrentView;
        bool MenuShown = false;

        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            // Initialise views to load into content area
            Views["home"] = new HomeView();
            Views["shows"] = new ShowsView();
            Views["news"] = new NewsView();
            Views["about"] = new AboutView();
            Views["contact"] = new ContactView();
            Views["settings"] = new SettingsView();
            CurrentView = "home";
            UpdateUI();
        }

        /// <summary>
        /// Loads a view into the content area of the main page
        /// </summary>
        /// <param name="target">Name of view to load</param>
        public void Navigate(string target)
        {
            if (!Views.ContainsKey(target))
            {
                throw new Exception($"[Navigation error] View not found for '{target}'");
            }
            if (MenuShown)
            {
                CloseMenu();
            }
            ContentAreaScrollView.Content = Views[target];
            CurrentView = target;
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Update tabs
            HomeTab.TextColor = CurrentView == "home" ? Color.Orange : Color.Black;
            ShowsTab.TextColor = CurrentView == "shows" ? Color.Orange : Color.Black;
            NewsTab.TextColor = CurrentView == "news" ? Color.Orange : Color.Black;
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
            }
        }
        private void CloseMenu()
        {
            if (MenuShown)
            {
                MenuFrame.TranslateTo(-200, 0, 200, Easing.Linear);
                MenuShown = false;
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
