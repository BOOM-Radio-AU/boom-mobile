using System;
using System.Collections.Generic;
using System.Text;

namespace BoomRadio
{
    /// <summary>
    /// Styles the status bar
    /// </summary>
    public interface IStatusBarStyler
    {
        /// <summary>
        /// Styles the status bar in a light theme
        /// </summary>
        void SetLightTheme();
        /// <summary>
        /// Styles the status bar in a dark theme
        /// </summary>
        void SetDarkTheme();
        /// <summary>
        /// Checks if the current theme is the dark theme
        /// </summary>
        /// <returns>Current theme is dark</returns>
        bool IsDarkTheme();
    }
}
