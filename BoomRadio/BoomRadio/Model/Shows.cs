using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BoomRadio.Model
{
    public class Shows
    {


        public int ID { get; set; }
        public string ShowTitle { get; set; }
        public string ShowSchedule { get; set; }
        public string ShowDescription { get; set; }
        public string ShowImageQueryUrl { get; set; }
        public string ShowImageUrl { get; set; }


        private readonly HttpClient client = new HttpClient();
        public string MediaApiPrefix = "https://boomradio.com.au/wp-json/wp/v2/schedule/";



        public Shows(int id, string title, string time, string description, string imageURL)
        {
            ID = id;
            ShowTitle = title;
            ShowSchedule = time;
            ShowDescription = description;
            ShowImageQueryUrl = imageURL;
        }


        public string TextFromHTML(string html)
        {
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            string text = stripFormattingRegex.Replace(html, string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            return text.Trim();
        }



        /// <summary>
        /// Fetches data for the image associated with the news article
        /// </summary>
        /// <returns>Image data</returns>
        public async Task<string> FetchImage()
        {
            // Fetch data from api
            var response = await client.GetAsync(ShowImageQueryUrl);

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
            ShowImageUrl = response.Value<string>("source_url");
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
                Console.WriteLine("Error updating shows\n" + e.Message);
            }
            return ShowImageUrl;
        }

        public static implicit operator ObservableCollection<object>(Shows v)
        {
            throw new NotImplementedException();
        }
    }
}
