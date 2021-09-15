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
        void Play();
        void Pause();
        void Stop();
        void PlayFromUri(string uri);
    }
}
