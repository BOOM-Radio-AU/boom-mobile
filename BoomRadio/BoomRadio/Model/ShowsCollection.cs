using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoomRadio.Model
{
    public class ShowsCollection
    {
        public List<Shows> shows;
        private DateTime lastUpdated;

        public ShowsCollection()
        {
            shows = new List<Shows>();
        }

        /// <summary>
        /// Merges a new list of news articles with the current list articles, reusing
        /// existing articles where possible. This prevents needlessly repeating media api
        /// querires.
        /// </summary>
        /// <param name="newsArticles">List of new articles</param>
        public void MergeShows(List<Shows> newShows)
        {
            List<Shows> freshShows = new List<Shows>();
            foreach (Shows show in newShows)
            {
                // Check if existing item in articles
                Shows existingshow = shows.Find(s => s.ID == show.ID);
                // Can re-use the exisiting article if it is present
                if (existingshow != null)
                {
                    freshShows.Add(existingshow);
                }
                else
                {
                    freshShows.Add(show);
                }
            }
            shows = freshShows;
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
                List<Shows> fetchedShows = await Api.GetShowsAsync();
                if (fetchedShows.Count == 0)
                {
                    // Nothing returned from API
                    return false;
                }
                MergeShows(fetchedShows);
                lastUpdated = DateTime.Now;
                return true;
            }
            catch (Exception e)
            {
                DependencyService.Get<ILogging>().Error(this, e);
                Console.WriteLine("Error updating shows\n" + e.Message);
                return false;
            }

        }
    }
}

