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

        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            Views["home"] = new HomeView();
            Views["shows"] = new ShowsView();
            Views["news"] = new NewsView();
            CurrentView = "home";
        }

        public void Navigate(string target)
        {
            if (!Views.ContainsKey(target))
            {
                throw new Exception($"[Navigation error] View not found for '{target}'");
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
    }
}
