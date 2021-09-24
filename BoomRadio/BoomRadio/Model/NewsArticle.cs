using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoomRadio.Model
{
    public class NewsArticle
    {
        public DateTime DatePublished { get; private set; }
        public DateTime DateModified { get; private set; }
        public string Title { get; private set; }
        public string ContentHTML { get; private set; }
        public string Excerpt { get; private set; }
        public string MediaID { get; private set; }
        public string ImageUrl { get; private set; } = null;

        private readonly HttpClient client = new HttpClient();
        public string MediaApiPrefix = "https://boomradio.com.au/wp-json/wp/v2/media/";

        public string TextFromHTML(string html)
        {
            // Regex to match any character between '<' and '>', even when end tag is missing
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            // Remove all tags
            string text = stripFormattingRegex.Replace(html, string.Empty);
            // Decode any encoded entities
            text = System.Net.WebUtility.HtmlDecode(text);
            // Trim off any extra whitespace
            return text.Trim();
        }


        public NewsArticle(string title, string content, string excerpt, string published, string modified, string mediaId)
        {
            Title = title;
            ContentHTML = content;
            Excerpt = TextFromHTML(excerpt);
            DatePublished = DateTime.Parse(published);
            DateModified = DateTime.Parse(modified);
            MediaID = mediaId;
        }


        public async Task<string> FetchImage()
        {
            // Fetch data from api
            var response = await client.GetAsync(MediaApiPrefix + MediaID);

            // Check for errors
            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
            {
                throw new Exception(string.Format("Data could not be retrieved from the server (code: {0})", response.StatusCode));
            }


            // Extract the response
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public void ParseImageUrl(string apiResponse)
        {
            JObject response = JsonConvert.DeserializeObject<JObject>(apiResponse);
            ImageUrl = response.Value<string>("source_url");
        }

        public async Task<string> UpdateImageUrl()
        {
            try
            {
                string response = await FetchImage();
                ParseImageUrl(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating articles\n" + e.Message);
            }
            return ImageUrl;
        }
    }
}
