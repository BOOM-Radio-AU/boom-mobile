using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoomRadio.Model
{
    public class NewsCollection
    {
        public List<NewsArticle> articles;
        private DateTime lastUpdated;
        public Api.Service service { get; set; } = Api.Service.News;
        
        public NewsCollection()
        {
            articles = new List<NewsArticle>();
        }

        /// <summary>
        /// Merges a new list of news articles with the current list articles, reusing
        /// existing articles where possible. This prevents needlessly repeating media api
        /// querires.
        /// </summary>
        /// <param name="newsArticles">List of new articles</param>
        public void MergeArticles(List<NewsArticle> newsArticles)
        {
            List<NewsArticle> freshArticles = new List<NewsArticle>();
            foreach (NewsArticle article in newsArticles)
            {
                // Check if existing item in articles
                NewsArticle existingArticle = articles.Find(a => a.ID == article.ID);
                // Can re-use the exisiting article if it is present, and has the same last-modified date
                if (existingArticle != null && existingArticle.DateModified == article.DateModified)
                {
                    freshArticles.Add(existingArticle);
                }
                else
                {
                    freshArticles.Add(article);
                }
            }
            articles = freshArticles;
        }

        /// <summary>
        /// Updates the collection from the web API
        /// </summary>
        /// <returns>Changes were made</returns>
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
                List<NewsArticle> newsArticles = null;
                if (service == Api.Service.News)
                {
                    newsArticles = await Api.GetNewsArticlesAsync();
                } else if (service == Api.Service.About)
                {
                    newsArticles = await Api.GetAboutArticlesAsync();
                }
                if (newsArticles == null || newsArticles.Count == 0)
                {
                    // Nothing returned from API, so don't update
                    return false;
                }
                MergeArticles(newsArticles);
                lastUpdated = DateTime.Now;
                return true;
            }
            catch (Exception e)
            {
                DependencyService.Get<ILogging>().Error(this, e);
                Console.WriteLine("Error updating articles\n" + e.Message);
                return false;
            }
        }
    }
}
