using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BoomRadio.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(StreamingService))]
namespace BoomRadio.Droid
{
    /// <summary>
    /// Streaming for Android. Based on https://ilanolkies.com/post/Xamarin-Radio-Streaming-Player
    /// </summary>
    public class StreamingService : IStreaming
    {
        MediaPlayer player;
        //Tell our player to stream music
        bool IsPrepared = false;
        string dataSource = "http://pollux.shoutca.st:8132/stream";

        /// <summary>
        /// Starts playing, creating or reseting the native MediaPlayer if needed
        /// </summary>
        public void Play()
        {
            if (IsPrepared)
            {
                // Player is prepared, so just call the native MediaPlayer method
                player.Start();
            }
            else
            {
                // Create or reset the player
                if (player == null)
                {
                    CreatePlayer();
                }
                else
                {
                    player.Reset();
                }
                // Set the data source
                player.SetDataSource(dataSource);
                // Prepare the player (this will call the Prepared event handler)
                player.PrepareAsync();
            }
        }

        /// <summary>
        /// Creates a new MediaPlayer object and attaches event handlers
        /// </summary>
        private void CreatePlayer()
        {
            player = new MediaPlayer();

            // Attach event handler for errors during preparation or playback
            player.Error += (sender, args) =>
            {
                Console.WriteLine("Error in playback resetting: " + args.What);
                // Clean up and reset the player.
                Stop();
            };

            // Attach event handler to start playing once prepared
            player.Prepared += (sender, args) =>
            {
                Console.WriteLine("@@@ Starting player...");
                player.Start();
                IsPrepared = true;
            };
        }

        /// <summary>
        /// Pauses playback
        /// </summary>
        public void Pause()
        {
            if (IsPrepared)
            {
                player.Pause();
            }
            else
            {
                // The player can't be paused before it has been prepared, so
                // just reset it instead
                player.Reset();
            }
        }

        /// <summary>
        /// Stops playback
        /// </summary>
        public void Stop()
        {
            if (IsPrepared)
            {
                player.Stop();
                IsPrepared = false;
            }
            else
            {
                // The player can't be stopped before it has been prepared, so
                // just reset it instead
                player.Reset();
            }
        }

        /// <summary>
        /// Sets the data source for the player, then starts playing
        /// </summary>
        /// <param name="uri">Data source URI</param>
        public void PlayFromUri(string uri)
        {
            // Stop anything previously played or paused
            if (player != null) Stop();
            dataSource = uri;
            Play();
        }
    }
}