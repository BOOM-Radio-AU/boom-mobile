using Android.OS;
using Android.Views;
using BoomRadio.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBarStylerService))]
namespace BoomRadio.Droid
{
    /// <summary>
    /// Styles the top status bar in dark or light theme colours.
    /// Based on https://evgenyzborovsky.com/2018/08/20/dynamically-changing-the-status-bar-appearance-in-xamarin-forms/
    /// </summary>
    public class StatusBarStylerService : IStatusBarStyler
    {
        private bool isDarkTheme;

        /// <inheritdoc/>
        public bool IsDarkTheme() => isDarkTheme;

        /// <inheritdoc/>
        public void SetDarkTheme()
        {
            isDarkTheme = true;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor("#5b5b5b"));
                    try
                    {   // Obselete, but works on API <= 28
                        currentWindow.DecorView.SystemUiVisibility = 0;
                    }
                    catch { }
                });
            }
        }

        /// <inheritdoc/>
        public void SetLightTheme()
        {
            isDarkTheme = false;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor("#D3D3D3"));
                    try
                    {   // Obselete, but works on API <= 28
                        currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                    }
                    catch { }
                });
            }
        }

        /// <summary>
        /// Gets the current window object
        /// </summary>
        /// <returns>current window</returns>
        private Window GetCurrentWindow()
        {
            var window = Platform.CurrentActivity.Window;

            // clear FLAG_TRANSLUCENT_STATUS flag:
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);

            // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            return window;
        }
    }
}