using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Microsoft.AppCenter.Analytics;

namespace BoomRadio.Model
{
    /// <summary>
    /// Media player for streaming, playing, pausing the live stream
    /// </summary>
    public class MediaPlayer
    {
        readonly Track defaultTrack = new Track();

        private IStreaming NativePlayer { get; set; }
        private string LiveStreamURI = "http://pollux.shoutca.st:8132/stream";
        public bool IsLive = false;
        public bool IsPlaying = false;
        public bool IsPaused = false;
        public string Artist { get; private set; }
        public string Title { get; private set; }
        public string CoverURI { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MediaPlayer()
        {
            NativePlayer = DependencyService.Get<IStreaming>();
            Artist = defaultTrack.Artist;
            Title = defaultTrack.Title;
            CoverURI = defaultTrack.ImageUri;
        }

        /// <summary>
        /// Plays the live stream, live
        /// </summary>
        public void PlayLive()
        {
            Analytics.TrackEvent("play", new Dictionary<string, string>{
                { "live", "true"}
            });
            Artist = defaultTrack.Artist;
            Title = defaultTrack.Title;
            CoverURI = defaultTrack.ImageUri;
            NativePlayer.PlayFromUri(LiveStreamURI);
            IsPlaying = true;
            IsPaused = false;
            IsLive = true;
        }

        /// <summary>
        /// Checks if it is possible to go live, i.e. not already playing live and either playing or paused
        /// </summary>
        /// <returns>Can go live</returns>
        public bool CanGoLive()
        {
            return !IsLive && (IsPaused || IsPlaying);
        }

        /// <summary>
        /// Plays a podcast
        /// </summary>
        /// <param name="artist">Podcast artist</param>
        /// <param name="trackTitle">Podcast title</param>
        /// <param name="audioUrl">Podcast media url</param>
        /// <param name="imageUrl">Cover art ur</param>
        public void PlayPodcast(string artist, string trackTitle, string audioUrl, string imageUrl)
        {
            Artist = artist;
            Title = trackTitle;
            CoverURI = imageUrl;
            NativePlayer.PlayFromUri(audioUrl);
            IsPlaying = true;
            IsPaused = false;
            IsLive = false;
        }

        /// <summary>
        /// Plays the player, either resuming from paused state, or else playing the live stream
        /// </summary>
        public void Play()
        {
            if (IsPaused)
            {
                Analytics.TrackEvent("play", new Dictionary<string, string>{
                    { "live", "false"}
                });
                NativePlayer.Play();
                IsPlaying = true;
                IsPaused = false;
                IsLive = false;
            }
            else
            {
                PlayLive();
            }
        }

        /// <summary>
        /// Pauses the player
        /// </summary>
        public void Pause()
        {
            NativePlayer.Pause();
            IsPlaying = false;
            IsPaused = true;
            IsLive = false;
        }

        /// <summary>
        /// Sets the track information to show there is no connection
        /// </summary>
        public void SetNotConnected()
        {
            Artist = "No connection";
            Title = "";
        }

        /// <summary>
        /// Updates the live track information from the API
        /// </summary>
        public async Task UpdateLiveTrackInfo()
        {
            if (IsLive && IsPlaying)
            {
                Track liveStreamTrack = await Api.GetLiveStreamTrackAsync();
                Artist = liveStreamTrack.Artist;
                Title = liveStreamTrack.Title;
                CoverURI = liveStreamTrack.ImageUri;
            }
        }


    }
}
