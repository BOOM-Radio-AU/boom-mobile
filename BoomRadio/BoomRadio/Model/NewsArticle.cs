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
        public int ID { get; private set; }
        public DateTime DatePublished { get; private set; }
        public DateTime DateModified { get; private set; }
        public string Title { get; private set; }
        public string ContentHTML { get; private set; }
        public string Excerpt { get; private set; }
        public string MediaID { get; private set; }
        public string ImageUrl { get; private set; } = null;

        private readonly HttpClient client = new HttpClient();
        public string MediaApiPrefix = "https://boomradio.com.au/wp-json/wp/v2/media/";

        /// <summary>
        /// Extracts text content from a HTML string
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <returns>Text content</returns>
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


        public NewsArticle(int id, string title, string content, string excerpt, string published, string modified, string mediaId)
        {
            ID = id;
            Title = title;
            ContentHTML = content;
            Excerpt = TextFromHTML(excerpt);
            DateTime dPub;
            if (DateTime.TryParse(published, out dPub)) {
                DatePublished = dPub;
            }
            DateTime dMod;
            if (DateTime.TryParse(modified, out dMod)) {
                DatePublished = dMod;
            }
            MediaID = mediaId;
        }

        /// <summary>
        /// Fetches data for the image associated with the news article
        /// </summary>
        /// <returns>Image data</returns>
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

        /// <summary>
        /// Sets the image url by parsing an api response of image data 
        /// </summary>
        /// <param name="apiResponse">image data from <see cref="FetchImage"/></param>
        public void ParseImageUrl(string apiResponse)
        {
            JObject response = JsonConvert.DeserializeObject<JObject>(apiResponse);
            ImageUrl = response.Value<string>("source_url");
        }

        /// <summary>
        /// Fetches and parses image data from the server, and sets the image url
        /// </summary>
        /// <returns>image url</returns>
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

        /// <summary>
        /// Splits the content html into chunks of text (non-image) content, removing image tags
        /// </summary>
        /// <returns>text (non-image) content chunks</returns>
        public Queue<string> ContentTextChunks()
        {
            Regex imagesRegex = new Regex(@"<img[^>]*>", RegexOptions.IgnoreCase);
            return new Queue<string>(imagesRegex.Split(ContentHTML));
            
        }
        /// <summary>
        /// Parses the content for image source urls
        /// </summary>
        /// <returns>Image source urls</returns>
        public Queue<string> ContentImageUrls()
        {
            Queue<string> imgUrls = new Queue<string>();
            Regex imagesRegex = new Regex(@"<img[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection imgTags = imagesRegex.Matches(ContentHTML);
            foreach (Match imgTagMatch in imgTags) {
                // Parse url from src attribute. Based on https://stackoverflow.com/questions/4257359/regular-expression-to-get-the-src-of-images-in-c-sharp
                string srcUrl = Regex.Match(imgTagMatch.Value, "<img.+?src=[\"'](.+?)[\"'][^>]*>", RegexOptions.IgnoreCase).Groups[1].Value;
                imgUrls.Enqueue(srcUrl);
            }
            return imgUrls;
        }
    }
}
