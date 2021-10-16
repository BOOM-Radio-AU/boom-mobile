using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace BoomRadio.Model
{
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
            Artist = defaultTrack.Artist;
            Title = defaultTrack.Title;
            CoverURI = defaultTrack.ImageUri;
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
            Title = "";
            //CoverURI = "";
        }

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
