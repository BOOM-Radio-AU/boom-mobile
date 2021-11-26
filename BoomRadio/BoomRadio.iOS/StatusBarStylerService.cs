using BoomRadio;
using BoomRadio.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBarStylerService))]
namespace BoomRadio.iOS
{
    /// <summary>
    /// Styles the top status bar text in dark or light theme colours.
    /// Based on https://evgenyzborovsky.com/2018/08/20/dynamically-changing-the-status-bar-appearance-in-xamarin-forms/
    /// </summary>
    public class StatusBarStylerService : IStatusBarStyler
    {
        public StatusBarStylerService() { }
        private bool isDarkTheme;

        /// <inheritdoc/>
        public bool IsDarkTheme() => isDarkTheme;

        /// <inheritdoc/>
        public void SetDarkTheme()
        {
            isDarkTheme = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        /// <inheritdoc/>
        public void SetLightTheme()
        {
            isDarkTheme = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        /// <summary>
        /// Gets the current view controller
        /// </summary>
        /// <returns>current view controller</returns>
        UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
    }
}