using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Microsoft.AppCenter.Analytics;

namespace BoomRadio.Model
{
    public class MediaPlayer
    {
        readonly string defaultArtist = "BOOM Radio";
        readonly string defaultTrack = "Not Just Noise";
        readonly string defaultCoverURI = "https://cdn-radiotime-logos.tunein.com/s195836q.png";

        readonly LiveStreamTrack liveStreamTrack = new LiveStreamTrack();

        private IStreaming NativePlayer { get; set; }
        private string LiveStreamURI = "http://pollux.shoutca.st:8132/stream";
        public bool IsLive = false;
        public bool IsPlaying = false;
        public bool IsPaused = false;
        public string Artist { get; private set; }
        public string Track { get; private set; }
        public string CoverURI { get; private set; }
        

        public MediaPlayer()
        {
            NativePlayer = DependencyService.Get<IStreaming>();
            Artist = defaultArtist;
            Track = defaultTrack;
            CoverURI = defaultCoverURI;
        }

        /// <summary>
        /// Plays the live stream, live
        /// </summary>
        public void PlayLive()
        {
            Analytics.TrackEvent("play", new Dictionary<string, string>{
                { "live", "true"}
            });
            Artist = defaultArtist;
            Track = defaultTrack;
            CoverURI = defaultCoverURI;
            NativePlayer.PlayFromUri(LiveStreamURI);
            IsPlaying = true;
            IsPaused = false;
            IsLive = true;
        }

        public bool CanGoLive()
        {
            return !IsLive && (IsPaused || IsPlaying);
        }

        public void PlayPodcast(string artist, string trackTitle, string audioUrl, string imageUrl)
        {
            Artist = artist;
            Track = trackTitle;
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

        public void Pause()
        {
            NativePlayer.Pause();
            IsPlaying = false;
            IsPaused = true;
            IsLive = false;
        }

        public void SetNotConnected()
        {
            //NativePlayer.Stop();
            Artist = "No connection";
            Track = "";
            //CoverURI = "";
        }

        public async Task UpdateLiveTrackInfo()
        {
            if (IsLive && IsPlaying)
            {
                await liveStreamTrack.Update();
                Artist = liveStreamTrack.Artist;
                Track = liveStreamTrack.Title;
                CoverURI = liveStreamTrack.ImageUri;
            }
        }


    }
}
