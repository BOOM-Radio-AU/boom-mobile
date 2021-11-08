using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using BoomRadio.Model;

namespace BoomRadio.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : StackLayout, IUpdatableUI
    {
        // Keys used to store preferences
        readonly string darkModeKey = "darkMode";
        readonly string deviceDarkModeKey = "deviceDarkMode";
        readonly string autoplayKey = "autoplay";
        MainPage MainPage;

        public SettingsView(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            UpdateTheme();
            Application.Current.RequestedThemeChanged += (sender, args) => UpdateTheme();
            
        }

        /// <summary>
        /// Updates the Theme (and UIs) to use dark/light mode based on the saved preference
        /// </summary>
        public void UpdateTheme()
        {
            if (Preferences.Get(deviceDarkModeKey, false))
            {
                Theme.UseDarkMode = Application.Current.RequestedTheme == OSAppTheme.Dark;
            }
            else
            {
                Theme.UseDarkMode = Preferences.Get(darkModeKey, false);
            }
            UpdateUI();
            MainPage.UpdateUI();
            MainPage.UpdatePlayerColours();
        }

        /// <summary>
        /// Updates the user interface
        /// </summary>
        public void UpdateUI()
        {
            // Toggle switches to the saved preference values, or off by default
            DarkModeSwitch.IsToggled = Preferences.Get(darkModeKey, false);
            DeviceDarkModeSwitch.IsToggled = Preferences.Get(deviceDarkModeKey, false);
            AutoplaySwitch.IsToggled = Preferences.Get(autoplayKey, false);
            // Disable darm mode toggle if using device mode
            if (DeviceDarkModeSwitch.IsToggled)
            {
                DarkModeSwitch.IsEnabled = false;
                DarkModeLabel.Opacity = 0.6;
            }
            else
            {
                DarkModeSwitch.IsEnabled = true;
                DarkModeLabel.Opacity = 1;
            }
            // Update colors
            ContainerGrid.BackgroundColor = Theme.GetColour("background");
            DarkModeLabel.TextColor = Theme.GetColour("text");
            DeviceDarkModeLabel.TextColor = Theme.GetColour("text");
            AutoplayLabel.TextColor = Theme.GetColour("text");
            HeadingBox.UpdateColours();
        }

        /// <summary>
        /// Handles the dark mode switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set(darkModeKey, e.Value);
            UpdateTheme();
        }

        /// <summary>
        /// Handles the device dark mode switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceDarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set(deviceDarkModeKey, e.Value);
            UpdateTheme();
        }

        /// <summary>
        /// Handles the autoplay switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoplaySwitch_Toggled(object sender, ToggledEventArgs e) => Preferences.Set(autoplayKey, e.Value);


        public void SetHorizontalDisplay()
        {
            // TODO;
        }

        public void SetVerticalDisplay()
        {
            // TODO;
        }
    }
}