using BoomRadio.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoomRadio
{
    public class Api : UnitTestable
    {
        bool useTestServer = false;
        bool use2021Website = false;

        private string websiteApiBaseUrl;
        private readonly HttpClient client = new HttpClient();
        public enum Service { LiveTrack, News, Media, Shows, Sponsor, About };
        private Dictionary<Service, string> Url;

        static Api instance;
        public Api()
        {
            if (useTestServer)
            {
                websiteApiBaseUrl = "https://chapterseventyseven.com/boomradio2/wordpress/wp-json/";
            }
            else
            {
                websiteApiBaseUrl = "https://boomradio.com.au/wp-json/";
            }
            if (use2021Website)
            {
                Url = new Dictionary<Service, string>() {
                    {Service.LiveTrack, "https://feed.tunein.com/profiles/s195836/nowPlaying"},
                    {Service.News, websiteApiBaseUrl+"wp/v2/trends" },
                    {Service.Media, websiteApiBaseUrl + "wp/v2/media/" },
                    {Service.Shows, websiteApiBaseUrl + "wp/v2/programs" },
                    {Service.Sponsor, websiteApiBaseUrl + "wp/v2/sponsors" },
                    {Service.About, websiteApiBaseUrl + "wp/v2/about" }
                };
            }
            else
            {
                Url = new Dictionary<Service, string>() {
                    {Service.LiveTrack, "https://feed.tunein.com/profiles/s195836/nowPlaying"},
                    {Service.News, websiteApiBaseUrl + "wp/v2/news" },
                    {Service.Media, websiteApiBaseUrl + "wp/v2/media/" },
                    {Service.Shows, websiteApiBaseUrl + "wp/v2/schedule" },
                    {Service.Sponsor, websiteApiBaseUrl + "wp/v2/sponsors" },
                    {Service.About, websiteApiBaseUrl + "wp/v2/about" }
                };
            }
        }

        static Api()
        {
            instance = new Api();
        }

        /// <summary>
        /// Fetches data from an API
        /// </summary>
        /// <param name="service">Service type</param>
        /// <exception cref="Exception">Data could not be retrieved from the server</exception>
        /// <returns>Response</returns>
        private async Task<string> FetchAsync(Service service) => await FetchAsync(Url[service]);

        /// <summary>
        /// Fetches data from an API
        /// </summary>
        /// <param name="url">API url</param>
        /// <exception cref="Exception">Data could not be retrieved from the server</exception>
        /// <returns>Response</returns>
        private async Task<string> FetchAsync(string url)
        {
            // Fetch data from server's api
            var response = await client.GetAsync(url);

            // Check for errors
            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
            {
                throw new Exception(string.Format("Data could not be retrieved from the server (code: {0}) for: {1}", response.StatusCode, url));
            }

            // Extract the response
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        /// <summary>
        /// Parses the the track from the <see cref="Service.LiveTrack"/> API response
        /// </summary>
        /// <param name="responseString">API response</param>
        /// <exception cref="Exception">JSON parsing errors</exception>
        /// <returns>Track information</returns>
        private Track ParseTrackResponse(string responseString)
        {
            string artist;
            string title;
            string imageUrl;

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
                artist = parts[0];
                title = parts[1] == parts[0] ? "" : parts[1];
            }
            else
            {
                artist = info;
                title = track.Value<string>("Subtitle");
            }
            imageUrl = track.Value<string>("Image").Replace("http://", "https://");
            return new Track() { Artist = artist, Title = title, ImageUri = imageUrl };
        }

        /// <summary>
        /// Fetches the live stream tack information from the API
        /// </summary>
        /// <returns>Live stream track</returns>
        public static async Task<Track> GetLiveStreamTrackAsync()
        {
            try
            {
                string response = await instance.FetchAsync(Service.LiveTrack);
                return instance.ParseTrackResponse(response);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogging>().Error("Api.GetLiveStreamTrackAsync", ex);
                Console.WriteLine("[API] LiveStream error: " + ex.Message);
                // Use default info
                return new Track();
            }
        }

        /// <summary>
        /// Parses news articles from the <see cref="Service.News"/> API response
        /// </summary>
        /// <param name="response">API response</param>
        /// <returns>News articles</returns>
        private List<NewsArticle> ParseNewsResponse(string response)
        {

            List<NewsArticle> newsArticles = new List<NewsArticle>();
            JArray responseItems = JsonConvert.DeserializeObject<JArray>(response);
            foreach (JToken item in responseItems)
            {
                try
                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string title = item.Value<JObject>("title").Value<string>("rendered"); ;
                    string content = item.Value<JObject>("content").Value<string>("rendered");
                    string excerpt = item.Value<JObject>("excerpt").Value<string>("rendered");
                    string published = item.Value<string>("date");
                    string modified = item.Value<string>("modified");
                    string mediaId = item.Value<int>("featured_media").ToString();
                    if (mediaId == "0")
                    {
                        mediaId = null;
                    }

                    NewsArticle article = new NewsArticle(id, title, content, excerpt, published, modified, mediaId);
                    newsArticles.Add(article);
                }
                catch (Exception ex)
                {
                    DependencyService.Get<ILogging>().Warn("Api.ParseNewsResponse", "Error parsing news article: " + ex.Message);
                }
            }
            return newsArticles;
        }



        /// <summary>
        /// Parses the about articles from the <see cref="Service.About"/> API Response
        /// </summary>
        /// <param name="response">API response</param>
        /// <returns>About articles</returns>
        private List<NewsArticle> ParseAboutResponse(string response)
        {

            List<NewsArticle> aboutArticles = new List<NewsArticle>();
            JArray responseItems = JsonConvert.DeserializeObject<JArray>(response);
            foreach (JToken item in responseItems)
            {
                try
                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string title = item.Value<JObject>("title").Value<string>("rendered"); ;
                    string content = item.Value<JObject>("content").Value<string>("rendered");
                    string excerpt = "";
                    string published = item.Value<string>("date");
                    string modified = item.Value<string>("modified");
                    string mediaId = item.Value<int>("featured_media").ToString();
                    if (mediaId == "0")
                    {
                        mediaId = null;
                    }

                    NewsArticle article = new NewsArticle(id, title, content, excerpt, published, modified, mediaId);
                    aboutArticles.Add(article);
                }
                catch (Exception ex)
                {
                    DependencyService.Get<ILogging>().Warn("Api.ParseAboutResponse", "Error parsing about article: " + ex.Message);
                }
            }
            return aboutArticles;
        }



        /// <summary>
        /// Fetches news articles from the API
        /// </summary>
        /// <returns>News articles</returns>
        public static async Task<List<NewsArticle>> GetNewsArticlesAsync()
        {
            try
            {
                string response = await instance.FetchAsync(Service.News);
                return instance.ParseNewsResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[API] News error: " + ex.Message);
                // Use default info
                return new List<NewsArticle>();
            }
        }



        /// <summary>
        /// Fetches about article from the API
        /// </summary>
        /// <returns>About info</returns>
        public static async Task<List<NewsArticle>> GetAboutArticlesAsync()
        {
            try
            {
                string response = await instance.FetchAsync(Service.About);
                return instance.ParseAboutResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[API] News error: " + ex.Message);
                // Use default info
                return new List<NewsArticle>();
            }
        }



        /// <summary>
        /// Parses the image url from the <see cref="Service.Media"/> API response
        /// </summary>
        /// <param name="response">API response</param>
        /// <returns>image url</returns>
        private string ParseMediaResponse(string response)
        {
            JObject responseObj = JsonConvert.DeserializeObject<JObject>(response);
            JObject sizes = responseObj.Value<JObject>("media_details").Value<JObject>("sizes");
            // Use a medium size image url if present
            if (sizes.ContainsKey("medium"))
            {
                return sizes.Value<JObject>("medium").Value<string>("source_url");
            }
            // Otherwise use the default full-size image url
            return responseObj.Value<string>("source_url");
        }

        public static async Task<string> GetImageUrlAsync(string mediaId)
        {
            return await Api.GetImageFromQueryAsync(instance.Url[Service.Media] + mediaId);
        }

        public static async Task<string> GetImageFromQueryAsync(string queryUrl)
        {
            try
            {
                string response = await instance.FetchAsync(queryUrl);
                return instance.ParseMediaResponse(response);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogging>().Error("Api.GetImageUrlAsync", ex);
                Console.WriteLine("[API] Media error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Parses shows from the <see cref="Service.Shows"/> API response
        /// </summary>
        /// <param name="response">API Response</param>
        /// <returns>Shows</returns>
        private List<Shows> ParseShowsResponse(string response)
        {

            List<Shows> showList = new List<Shows>();
            JArray responseItems = JsonConvert.DeserializeObject<JArray>(response);
            foreach (JToken item in responseItems)
            {
                try
                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string title = item.Value<JObject>("title").Value<string>("rendered");
                    string time = item.Value<JObject>("content")?.Value<string>("rendered");
                    string description = "";
                    if ((item as JObject).ContainsKey("ACF"))
                    {
                        // NEW WEBSITE
                        description = item.Value<JObject>("ACF").Value<string>("show_description");
                    }
                    else
                    {
                        // OLD WEBSITE
                        description = item.Value<JObject>("excerpt")?.Value<string>("rendered");
                    }

                    string imageURL = (item.Value<JObject>("_links").Value<JArray>("wp:featuredmedia")[0] as JObject).Value<string>("href");

                    Shows show = new Shows(id, title, time, description, imageURL);

                    showList.Add(show);
                }
                catch (Exception ex)
                {
                    DependencyService.Get<ILogging>().Warn("Api.ParseShowsResponse", "Error parsing show: " + ex.Message);
                }
            }
            return showList;
        }

        /// <summary>
        /// Fetches shows from the API
        /// </summary>
        /// <returns>Shows</returns>
        public static async Task<List<Shows>> GetShowsAsync()
        {
            try
            {
                string response = await instance.FetchAsync(Service.Shows);
                return instance.ParseShowsResponse(response);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogging>().Error("Api.GetShowsAsync", ex);
                Console.WriteLine("[API] Shows error: " + ex.Message);
                return new List<Shows>();
            }
        }

        private List<Sponsors> ParseSponsorsResponse(string response)
        {
            List<Sponsors> sponsorList = new List<Sponsors>();
            JArray responseItems = JsonConvert.DeserializeObject<JArray>(response);
            foreach (JToken item in responseItems)
            {
                try

                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string sponsorName = item.Value<JObject>("title").Value<string>("rendered");
                    string sponsorDescription = "";
                    if (use2021Website)
                    {
                        // NEW WEBSITE
                        sponsorDescription = item.Value<JObject>("excerpt").Value<string>("rendered");

                    }
                    else
                    {
                        // OLD WEBSITE
                        sponsorDescription = item.Value<JObject>("content").Value<string>("rendered");
                    }
                    string imageURL = (item.Value<JObject>("_links").Value<JArray>("wp:featuredmedia")[0] as JObject).Value<string>("href");

                    Sponsors sponsor = new Sponsors(id, sponsorName, sponsorDescription, imageURL);
                    sponsorList.Add(sponsor);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error parsing articles\n" + e.Message);
                }
            }
            return sponsorList;

        }

        public static async Task<List<Sponsors>> GetSponsorsAsync()
        {
            try
            {
                string response = await instance.FetchAsync(Service.Sponsor);
                return instance.ParseSponsorsResponse(response);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogging>().Error("Api.GetSponsorsAsync", ex);
                Console.WriteLine("[API] Sponsors error: " + ex.Message);
                return new List<Sponsors>();
            }
        }


    }
}
