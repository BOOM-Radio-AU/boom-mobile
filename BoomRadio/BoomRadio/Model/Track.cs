using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoomRadio
{
    /// <summary>
    /// Track played on the radio stream
    /// </summary>
    public class Track
    {
        public string Artist { get; set; } = "BOOM Radio";
        public string Title { get; set; } = "Not Just Noise";
        public string ImageUri { get; set; } = "https://cdn-radiotime-logos.tunein.com/s195836q.png";

        public static string defaultArtist { get; } = "BOOM Radio";
        public static string defaultTitle { get; } = "Not Just Noise";
        public static string defaultImageUri { get; } = "https://cdn-radiotime-logos.tunein.com/s195836q.png";

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            Track otherTrack = obj as Track;
            return otherTrack?.Artist == Artist && otherTrack?.Title == Title && otherTrack?.ImageUri == ImageUri;
        }
    }
}
