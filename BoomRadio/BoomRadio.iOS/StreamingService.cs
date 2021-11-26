using AVFoundation;
using BoomRadio.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StreamingService))]
namespace BoomRadio.iOS
{
    /// <summary>
    /// Streaming for iOS. Based on https://ilanolkies.com/post/Xamarin-Radio-Streaming-Player
    /// </summary>
    public class StreamingService : IStreaming
    {
        AVPlayer player;
        bool isPrepared;
        string dataSource = "http://pollux.shoutca.st:8132/stream";

        /// <inheritdoc/>
        public void Play()
        {
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            if (!isPrepared || player == null)
            {
                player = AVPlayer.FromUrl(NSUrl.FromString(dataSource));
            }

            isPrepared = true;
            player.Play();
        }

        /// <inheritdoc/>
        public void Pause()
        {
            player.Pause();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            player.Dispose();
            isPrepared = false;
        }

        /// <inheritdoc/>
        public void PlayFromUri(string uri)
        {
            if (isPrepared || player != null)
            {
                Stop();
            }
            dataSource = uri;
            Play();
        }
    }
}