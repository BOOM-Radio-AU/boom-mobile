using System.Collections.Generic;
using Android.Util;
using System;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using BoomRadio.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoggingService))]
namespace BoomRadio.Droid
{
    public class LoggingService : ILogging
    {
        /// <inheritdoc/>
        public void Error(object sender, Exception exception)
        {
            Log.Error($"[{sender}]", exception.Message);
            Crashes.TrackError(exception, new Dictionary<string, string>
            {
                {"sender", sender.ToString()}
            });
        }

        /// <inheritdoc/>
        public void Info(object sender, string message)
        {
            Log.Info($"[{sender}]", message);
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
            Log.Warn($"[{sender}]", message);
            Analytics.TrackEvent("log", new Dictionary<string, string>
            {
                {"type", "warning" },
                {"sender", sender.ToString() },
                {"message", message },
            });
        }
    }
}