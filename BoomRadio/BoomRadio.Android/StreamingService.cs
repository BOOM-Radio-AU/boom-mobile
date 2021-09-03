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

        public void Play()
        {
            if (IsPrepared)
            {
                player.Start();
            }
            else
            {
                if (player == null)
                {
                    player = new MediaPlayer();
                    player.Error += (sender, args) =>
                    {
                        //playback error
                        Console.WriteLine("Error in playback resetting: " + args.What);
                        Stop();//this will clean up and reset properly.
                    };
                    player.Prepared += (sender, args) =>
                    {
                        Console.WriteLine("@@@ Starting player...");
                        player.Start();
                        IsPrepared = true;
                    };
                }
                else
                {
                    player.Reset();
                }

                player.SetDataSource(dataSource);
                player.PrepareAsync();
            }
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Stop()
        {
            player.Stop();
            IsPrepared = false;
        }
    }
}