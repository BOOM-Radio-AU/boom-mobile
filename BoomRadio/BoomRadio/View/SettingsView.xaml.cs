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
            Theme.UseDarkMode = Preferences.Get(darkModeKey, false);
            MainPage = mainPage;
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
            // Update colors
            ContainerGrid.BackgroundColor = Theme.GetColour("background");
            DarkModeLabel.TextColor = Theme.GetColour("text");
            DeviceDarkModeLabel.TextColor = Theme.GetColour("text");
            AutoplayLabel.TextColor = Theme.GetColour("text");
        }

        /// <summary>
        /// Handles the dark mode switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set(darkModeKey, e.Value);
            Theme.UseDarkMode = e.Value;
            UpdateUI();
            MainPage.UpdateUI();
        }

        /// <summary>
        /// Handles the device dark mode switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceDarkModeSwitch_Toggled(object sender, ToggledEventArgs e) => Preferences.Set(deviceDarkModeKey, e.Value);

        /// <summary>
        /// Handles the autoplay switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoplaySwitch_Toggled(object sender, ToggledEventArgs e) => Preferences.Set(autoplayKey, e.Value);

    }
}