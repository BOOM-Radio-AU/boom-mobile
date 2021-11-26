using System;
using System.Collections.Generic;
using System.Text;

namespace BoomRadio
{
    /// <summary>
    /// Interface for streaming (which needs device-dependent code).
    /// Based on https://ilanolkies.com/post/Xamarin-Radio-Streaming-Player
    /// </summary>
    public interface IStreaming
    {
        /// <summary>
        /// Plays the stream
        /// </summary>
        void Play();
        /// <summary>
        /// Pauses the stream
        /// </summary>
        void Pause();
        /// <summary>
        /// Stops playback
        /// </summary>
        void Stop();
        /// <summary>
        /// Plays from a media/stream URI 
        /// </summary>
        /// <param name="uri">Media/stream URI</param>
        void PlayFromUri(string uri);
    }
}
