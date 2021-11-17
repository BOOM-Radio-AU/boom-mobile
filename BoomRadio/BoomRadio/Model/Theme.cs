using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BoomRadio.Model
{
    public static class Theme
    {
        private static Dictionary<string, Color> lightModeColors = new Dictionary<string, Color>();
        private static Dictionary<string, Color> darkModeColors = new Dictionary<string, Color>();

        public static bool UseDarkMode { get; set; } = false;

        // Key names for storing theme/settings in preferences 
        public static string DarkModeKey { get; } = "darkMode";
        public static string DeviceDarkModeKey { get; } = "deviceDarkMode";
        public static string AutoplayKey { get; } = "autoplay";

        static Theme()
        {
            // Initialise colours for each mode
            lightModeColors["text"] = Color.Black;
            lightModeColors["background"] = Color.White;
            lightModeColors["player-bg"] = Color.FromHex("889191");
            lightModeColors["nav-bg"] = Color.LightGray;
            lightModeColors["not-live"] = Color.Gray;
            lightModeColors["is-live"] = Color.Red;
            lightModeColors["accent"] = Color.FromHex("F27405");
            lightModeColors["highlight"] = Color.FromHex("F3712A");

            darkModeColors["text"] = Color.White;
            darkModeColors["background"] = Color.FromHex("474747");
            darkModeColors["player-bg"] = Color.FromHex("3F4F4F");
            darkModeColors["nav-bg"] = Color.FromHex("5b5b5b");
            darkModeColors["not-live"] = Color.LightGray;
            darkModeColors["is-live"] = Color.FromHex("f95454");
            darkModeColors["accent"] = Color.FromHex("F3712A");
            darkModeColors["highlight"] = Color.FromHex("F27405");
            UpdateFromPreferences();

            // Check if dark mode should be applied
            if (Preferences.Get(DeviceDarkModeKey, false))
            {
                UseDarkMode = Application.Current.RequestedTheme == OSAppTheme.Dark;
            }
            else
            {
                UseDarkMode = Preferences.Get(DarkModeKey, false);
            }
        }

        /// <summary>
        /// Gets a Color from the current colour mode
        /// </summary>
        /// <param name="name">Name of the colour</param>
        /// <returns>Color</returns>
        public static Color GetColour(string name)
        {
            return (UseDarkMode ? darkModeColors : lightModeColors)[name];
        }

        /// <summary>
        /// Updates the theme to use dark/light mode based on the saved preference
        /// </summary>
        public static void UpdateFromPreferences()
        {
            if (Preferences.Get(DeviceDarkModeKey, false))
            {
                UseDarkMode = Application.Current.RequestedTheme == OSAppTheme.Dark;
            }
            else
            {
                UseDarkMode = Preferences.Get(DarkModeKey, false);
            }
        }
    }
}
