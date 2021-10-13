using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BoomRadio.Model
{
    public class ShowsCollection
    {

        private string url = "https://boomradio.com.au/wp-json/wp/v2/schedule";
        private readonly HttpClient client = new HttpClient();
        public List<Shows> shows;
        private DateTime lastUpdated;

        public ShowsCollection()
        {
            shows = new List<Shows>();
        }

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
            List<Shows> freshShowList = new List<Shows>();
            JArray response;
            try
            {

                response = JsonConvert.DeserializeObject<JArray>(responseString);
                foreach (var item in response)
                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string title = item.Value<JObject>("title").Value<string>("rendered"); ;
                    string time = item.Value<JObject>("content").Value<string>("rendered");
                    string description = item.Value<JObject>("excerpt")?.Value<string>("rendered");
                    string imageURL = (item.Value<JObject>("_links").Value<JArray>("wp:featuredmedia")[0] as JObject).Value<string>("href");

                    Shows show = new Shows(id, title, time, description, imageURL);

                    // Check if existing item in articles
                    Shows existingShow = shows.Find(a => a.ID == id);
                    if (existingShow != null && existingShow.ShowTitle == existingShow.ShowTitle)
                    {
                        freshShowList.Add(existingShow);
                    }
                    else
                    {
                        freshShowList.Add(show);
                    }

                }
                shows = freshShowList;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing articles\n" + e.Message);
            }
        }


        public async Task<bool> UpdateAsync()
        {
            // Don't update if already updated recently
            if (lastUpdated != null)
            {
                TimeSpan timeSinceLastUpdate = DateTime.Now - lastUpdated;
                if (timeSinceLastUpdate < TimeSpan.FromMinutes(2))
                {
                    return false;
                }
            }

            try
            {
                string response = await Fetch();
                Parse(response);
                lastUpdated = DateTime.Now;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating shows\n" + e.Message);
                return false;
            }

        }
    }
}

