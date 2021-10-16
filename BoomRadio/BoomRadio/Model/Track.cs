using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoomRadio
{
    public class Track
    {
        public string Artist { get; set; } = "BOOM Radio";
        public string Title { get; set; } = "Not Just Noise";
        public string ImageUri { get; set; } = "https://cdn-radiotime-logos.tunein.com/s195836q.png";

    }
}
