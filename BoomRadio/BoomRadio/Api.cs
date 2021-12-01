using BoomRadio.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoomRadio
{
    /// <summary>
    /// Handles fetching and parsing content from BOOM Radio's APIs
    /// </summary>
    public class Api : UnitTestable
    {
        bool useTestServer = true;
        bool use2021Website = true;

        private string websiteApiBaseUrl;
        private readonly HttpClient client = new HttpClient();
        public enum Service { LiveTrack, Album, CoverArt, News, Media, Shows, Sponsor, About, Contests };
        private Dictionary<Service, string> Url;

        static Api instance;
        /// <summary>
        /// Constructor
        /// </summary>
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
                    {Service.LiveTrack, "http://pollux.shoutca.st:8132/7.html"},
                    {Service.Album, "https://musicbrainz.org/ws/2/recording?fmt=json&limit=1&query="},
                    {Service.CoverArt, "https://coverartarchive.org/release/"},
                    {Service.News, websiteApiBaseUrl+"wp/v2/trends" },
                    {Service.Media, websiteApiBaseUrl + "wp/v2/media/" },
                    {Service.Shows, websiteApiBaseUrl + "wp/v2/programs" },
                    {Service.Sponsor, websiteApiBaseUrl + "wp/v2/sponsors" },
                    {Service.About, websiteApiBaseUrl + "wp/v2/about" },
                    {Service.Contests, websiteApiBaseUrl + "wp/v2/contests" },
                };
            }
            else
            {
                Url = new Dictionary<Service, string>() {
                    {Service.LiveTrack, "http://pollux.shoutca.st:8132/7.html"},
                    {Service.Album, "https://musicbrainz.org/ws/2/recording?fmt=json&limit=1&query="},
                    {Service.CoverArt, "https://coverartarchive.org/release/"},
                    {Service.News, websiteApiBaseUrl + "wp/v2/news" },
                    {Service.Media, websiteApiBaseUrl + "wp/v2/media/" },
                    {Service.Shows, websiteApiBaseUrl + "wp/v2/schedule" },
                    {Service.Sponsor, websiteApiBaseUrl + "wp/v2/sponsors" },
                    {Service.About, websiteApiBaseUrl + "wp/v2/about" }
                };
            }
        }

        /// <summary>
        /// Static constructor, for singleton
        /// </summary>
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
        /// Gets the url for cover art of a song (or if not found, the default cover art url)
        /// </summary>
        /// <param name="artist">Song artist</param>
        /// <param name="title">Song track</param>
        /// <returns>Cover art image url</returns>
        private async Task<string> GetCoverArtAsync(string artist, string title)
        {
            // First get the albums' ids, if available
            List<string> ids = new List<string>();

            // Remove "ft. <otherArtist>" from the artist/title, which may impact search results
            Regex featuringRegex = new Regex(@" ft\.? .*$");
            string searchArtist = featuringRegex.Replace(artist, "");
            string searchTitle = featuringRegex.Replace(title, "");
            try
            {
                string query = Uri.EscapeDataString($"\"{searchArtist}\" AND \"{searchTitle}\"");
                string albumResponse = await FetchAsync(Url[Service.Album] + query);
                JObject responseObj = JsonConvert.DeserializeObject<JObject>(albumResponse);
                // Check that the response contains a result
                if (responseObj.Value<int>("count") < 1)
                {
                    return Track.defaultImageUri;
                }
                JObject recording = responseObj.Value<JArray>("recordings")[0] as JObject;
                JArray releases = recording.Value<JArray>("releases");
                foreach (JObject release in releases)
                {
                    ids.Add(release.Value<string>("id"));

                }
            } catch (Exception ex)
            {
                return Track.defaultImageUri;
            }

            // Then get the cover art
            try
            {
                string coverResponse = "";
                // Sometimes an id doesn't have any artwork, and the url gives a 'not found' exception,
                // so loop through all available ids until a valid response is found 
                foreach (string id in ids) {
                    try
                    {
                        coverResponse = await FetchAsync(Url[Service.CoverArt] + id);
                    }
                    catch {
                        Console.WriteLine("%%% No response for id: " + id);
                    }
                }
                // If there were no valid responses, use the default image
                if (coverResponse == "")
                {
                    return Track.defaultImageUri;
                }
                
                JObject coverResponseObj = JsonConvert.DeserializeObject<JObject>(coverResponse);
                JObject images = coverResponseObj.Value<JArray>("images")[0] as JObject;
                // Use a small thumbnail image if possible
                if (images != null && images.ContainsKey("thumbnails"))
                {
                    JObject thumbnails = images.Value<JObject>("thumbnails");
                    if (thumbnails.ContainsKey("small"))
                    {
                        return thumbnails.Value<string>("small");
                    }
                    else if (thumbnails.ContainsKey("250"))
                    {
                        return thumbnails.Value<string>("250");
                    }
                }
                // Otherwise use the full-sized image from the 'image' key, if present
                else if (images != null && images.ContainsKey("image"))
                {
                    return images.Value<string>("image");
                }
            }
            catch (Exception ex)
            {
                return Track.defaultImageUri;
            }
            return Track.defaultImageUri;
        }

        /// <summary>
        /// Parses the the track from the <see cref="Service.LiveTrack"/> API response
        /// </summary>
        /// <param name="responseString">API response</param>
        /// <exception cref="Exception">JSON parsing errors</exception>
        /// <returns>Track information</returns>
        private async Task<Track> ParseTrackResponse(string responseString, Track currentTrack)
        {
            string artist;
            string title;
            string imageUrl;

            // Parse relevant data out of the response string, which is something like:
            // "<html><head></head><body>10,1,27,9999,8,128,Hope D - Miscommunicate</body></html>"
            // The artist and title, if present, will appear after the sixth comma and be separated by a dash character
            Regex infoRegex = new Regex(@"(?:\d*,){6}(?<artist>[^-]*) - (?<title>.*)(?:</body></html>)");
            var match = infoRegex.Match(responseString);
            if (match.Success)
            {
                // Un-encode ampersands
                artist = match.Groups["artist"].Value.Replace("&amp;", "&");
                title = match.Groups["title"].Value.Replace("&amp;", "&");

                // Set title to empty if it duplicates artist (sometimes happens for adverts)
                if (title == artist)
                {
                    title = "";
                    // Also use the default image
                    imageUrl = Track.defaultImageUri;
                }
                else
                {
                    // If the artist and title are unchanged, just return the current track
                    if (artist == currentTrack.Artist && title == currentTrack.Title)
                    {
                        return currentTrack;
                    }
                    // Lookup the cover image by artists and title (leaving ampersands encoded)
                    imageUrl = await GetCoverArtAsync(match.Groups["artist"].Value, match.Groups["title"].Value);
                }
            }
            else
            {
                artist = Track.defaultArtist;
                title = Track.defaultTitle;
                imageUrl = Track.defaultImageUri;
            }
            imageUrl = imageUrl.Replace("http://", "https://");
            return new Track() { Artist = artist, Title = title, ImageUri = imageUrl };
        }

        /// <summary>
        /// Fetches the live stream tack information from the API
        /// </summary>
        /// <returns>Live stream track</returns>
        public static async Task<Track> GetLiveStreamTrackAsync(Track currentTrack)
        {
            try
            {
                string response = await instance.FetchAsync(Service.LiveTrack);
                return await instance.ParseTrackResponse(response, currentTrack);
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

        /// <summary>
        /// Gets an image url from a media ID
        /// </summary>
        /// <param name="mediaId">ID of media</param>
        /// <returns>Image url</returns>
        public static async Task<string> GetImageUrlAsync(string mediaId)
        {
            return await Api.GetImageFromQueryAsync(instance.Url[Service.Media] + mediaId);
        }

        /// <summary>
        /// Gets an image url from a media api query url
        /// </summary>
        /// <param name="queryUrl">media api query url</param>
        /// <returns>Image url</returns>
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

        /// <summary>
        /// Parses the sponsors from the <see cref="Service.Sponsor"/> API response
        /// </summary>
        /// <param name="response">API response</param>
        /// <returns>Sponsors</returns>
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

        /// <summary>
        /// Fetches sponsors from the API
        /// </summary>
        /// <returns>Sponsors</returns>
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


        /// <summary>
        /// Parses the sponsors from the <see cref="Service.Contests"/> API response
        /// </summary>
        /// <param name="response">API response</param>
        /// <returns>Contests</returns>
        private List<Contest> ParseContestsResponse(string response)
        {
            List<Contest> contests = new List<Contest>();
            JArray responseItems = JsonConvert.DeserializeObject<JArray>(response);
            foreach (JToken item in responseItems)
            {
                try
                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string title = item.Value<JObject>("title").Value<string>("rendered");
                    string link = item.Value<string>("link");
                    string mediaId = item.Value<int>("featured_media").ToString();
                    if (mediaId == "0")
                    {
                        mediaId = null;
                    }
                    Contest contest = new Contest(id, title, link, mediaId);
                    contests.Add(contest);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error parsing articles\n" + e.Message);
                }
            }
            return contests;
        }

        /// <summary>
        /// Fetches contests from the API
        /// </summary>
        /// <returns>Contests</returns>
        public static async Task<List<Contest>> GetContestsAsync()
        {
            if (!instance.use2021Website)
            {
                // Old website doesn't have api for contests
                return new List<Contest>();
            }
            try
            {
                string response = await instance.FetchAsync(Service.Contests);
                return instance.ParseContestsResponse(response);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogging>().Error("Api.GetContestsAsync", ex);
                Console.WriteLine("[API] Contests error: " + ex.Message);
                return new List<Contest>();
            }
        }
    }
}
