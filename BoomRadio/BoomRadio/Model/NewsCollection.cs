using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BoomRadio.Model
{
    public class NewsCollection
    {
        private string url = "https://boomradio.com.au/wp-json/wp/v2/news";
        private readonly HttpClient client = new HttpClient();
        public List<NewsArticle> articles;
        
        public NewsCollection()
        {
            articles = new List<NewsArticle>();
        }

        /// <summary>
        /// Fetches latest news articles from the server api
        /// </summary>
        /// <returns>String containing the JSON response</returns>
        public async Task<string> Fetch()
        {
            // Fetch data from api
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


        public void Parse(string responseString)
        {
            List<NewsArticle> freshArticles = new List<NewsArticle>();
            JArray response;
            try
            {

                response = JsonConvert.DeserializeObject<JArray>(responseString);
                foreach (var item in response)
                {
                    string title = item.Value<JObject>("title").Value<string>("rendered"); ;
                    string content = item.Value<JObject>("content").Value<string>("rendered");
                    string excerpt = item.Value<JObject>("excerpt").Value<string>("rendered");
                    string published = item.Value<string>("date");
                    string modified = item.Value<string>("modified");

                    NewsArticle article = new NewsArticle(title, content, excerpt, published, modified);
                    freshArticles.Add(article);
                }
                articles = freshArticles;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing articles\n" + e.Message);
            }
        }

        /// <summary>
        /// Updates the collection from the web API
        /// </summary>
        public async Task UpdateAsync()
        {
            try
            {
                string response = await Fetch();
                Parse(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating articles\n" + e.Message);
            }
        }
    }
}
