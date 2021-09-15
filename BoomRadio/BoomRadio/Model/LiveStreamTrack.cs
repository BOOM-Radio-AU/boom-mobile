using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoomRadio
{
    public class LiveStreamTrack
    {
        public string Artist { get; private set; } = "BOOM Radio";
        public string Title { get; private set; } = "Not Just Noise";
        public string ImageUri { get; set; } = "https://cdn-radiotime-logos.tunein.com/s195836q.png";

        private string url = "https://feed.tunein.com/profiles/s195836/nowPlaying";
        private readonly HttpClient client = new HttpClient();

        private async Task<string> Fetch()
        {
            Console.WriteLine(">>> Fetching track api");
            // Fetch data from server's api
            var response = await client.GetAsync(url);

            // Check for errors
            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
            {
                throw new Exception(string.Format("Data could not be retrieved from the server (code: {0})", response.StatusCode));
            }

            // Extract the response
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        private void Parse(string responseString)
        {
            try
            {
                // Parse relevant data out of the json response string
                JObject response = JsonConvert.DeserializeObject<JObject>(responseString);
                JObject track = response.Value<JObject>("Secondary");
                if (track == null)
                {
                    track = response.Value<JObject>("Primary");
                }
                string info = track.Value<string>("Title");
                if (info.Contains(" - "))
                {
                    string[] parts = info.Split(new string[] { " - " }, StringSplitOptions.None);
                    Artist = parts[0];
                    Title = parts[1] == parts[0] ? "" : parts[1];
                }
                else
                {
                    Artist = info;
                    Title = track.Value<string>("Subtitle");
                }
                ImageUri = track.Value<string>("Image").Replace("http://", "https://");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Parse error] " + ex.Message);
                Artist = "BOOM Radio";
                Title = "Not Just Noise";
                ImageUri = "https://cdn-radiotime-logos.tunein.com/s195836q.png";
            }
        }

        public async Task Update()
        {
            string response = await Fetch();
            Parse(response);
        }
    }
}
