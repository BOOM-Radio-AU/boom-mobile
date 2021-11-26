using Foundation;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using BoomRadio.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoggingService))]
namespace BoomRadio.iOS
{
    /// <summary>
    /// Logs messages
    /// </summary>
    public class LoggingService : ILogging
    {
        /// <inheritdoc/>
        public void Error(object sender, Exception exception)
        {
            Crashes.TrackError(exception);
        }

        /// <inheritdoc/>
        public void Info(object sender, string message)
        {
            Analytics.TrackEvent("log", new Dictionary<string, string>
            {
                {"type", "info" },
                {"sender", sender.ToString() },
                {"message", message },
            });
        }

        /// <inheritdoc/>
        public void Warn(object sender, string message)
        {
            Analytics.TrackEvent("log", new Dictionary<string, string>
            {
                {"type", "warning" },
                {"sender", sender.ToString() },
                {"message", message },
            });
        }
    }
}