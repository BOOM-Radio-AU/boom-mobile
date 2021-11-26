using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using BoomRadio.Model;
using System.Threading.Tasks;

namespace BoomRadio.View
{
    /// <summary>
    /// View for controlling app settings
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : StackLayout, IUpdatableUI
    {
        MainPage MainPage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPage">Main (parent) page</param>
        public SettingsView(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            UpdateTheme();
            
        }

        /// <summary>
        /// Updates the Theme (and UIs) to use dark/light mode based on the saved preference
        /// </summary>
        public void UpdateTheme()
        {
            Theme.UpdateFromPreferences();
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
            DarkModeSwitch.IsToggled = Preferences.Get(Theme.DarkModeKey, false);
            DeviceDarkModeSwitch.IsToggled = Preferences.Get(Theme.DeviceDarkModeKey, false);
            AutoplaySwitch.IsToggled = Preferences.Get(Theme.AutoplayKey, false);
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
            Preferences.Set(Theme.DarkModeKey, e.Value);
            UpdateTheme();
        }

        /// <summary>
        /// Handles the device dark mode switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceDarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set(Theme.DeviceDarkModeKey, e.Value);
            UpdateTheme();
        }

        /// <summary>
        /// Handles the autoplay switch being toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoplaySwitch_Toggled(object sender, ToggledEventArgs e) => Preferences.Set(Theme.AutoplayKey, e.Value);

        /// <summary>
        /// Sets the view for horizontal display
        /// </summary>
        public async void SetHorizontalDisplay()
        {
            while (this.Width == -1)
            {
                await Task.Delay(10);
            }
            ContainerGrid.Margin = new Thickness(this.Width / 8, 5);
        }

        /// <summary>
        /// Sets the view for verticla display
        /// </summary>
        public void SetVerticalDisplay()
        {
            ContainerGrid.Margin = new Thickness(5);
        }
    }
}