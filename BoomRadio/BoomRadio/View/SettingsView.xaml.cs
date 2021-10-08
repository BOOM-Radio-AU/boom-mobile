using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace BoomRadio.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : StackLayout, IUpdatableUI
    {
        // Keys used to store preferences
        readonly string darkModeKey = "darkMode";
        readonly string deviceDarkModeKey = "deviceDarkMode";
        readonly string autoplayKey = "autoplay";

        public SettingsView()
        {
            InitializeComponent();
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
        }

        /// <summary>
        /// Handles the dark mode switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e) => Preferences.Set(darkModeKey, e.Value);

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