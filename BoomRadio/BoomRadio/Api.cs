﻿using BoomRadio.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoomRadio
{
    public class Api
    {
        private readonly HttpClient client = new HttpClient();
        public enum Service { LiveTrack, News, Media };
        private Dictionary<Service, string> Url = new Dictionary<Service, string>() {
            {Service.LiveTrack, "https://feed.tunein.com/profiles/s195836/nowPlaying"},
            {Service.News, "https://boomradio.com.au/wp-json/wp/v2/news" },
            {Service.Media, "https://boomradio.com.au/wp-json/wp/v2/media/" }
        };

        static Api instance;
        private Api() { }
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
                throw new Exception(string.Format("Data could not be retrieved from the server (code: {0})", response.StatusCode));
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
        public Track ParseTrackResponse(string responseString)
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

                    NewsArticle article = new NewsArticle(id, title, content, excerpt, published, modified, mediaId);
                    newsArticles.Add(article);
                }
                catch { }
            }
            return newsArticles;
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
        /// Parses the image url from the <see cref="Service.Media"/> API response
        /// </summary>
        /// <param name="response">API response</param>
        /// <returns>image url</returns>
        public string ParseMediaResponse(string response)
        {
            JObject responseObj = JsonConvert.DeserializeObject<JObject>(response);
            return responseObj.Value<string>("source_url");
        }

        public static async Task<string> GetImageUrlAsync(string mediaId)
        {
            try
            {
                string mediaUrl = instance.Url[Service.Media] + mediaId;
                string response = await instance.FetchAsync(mediaUrl);
                return instance.ParseMediaResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[API] Media error: " + ex.Message);
                return null;
            }
        }
    }
}
