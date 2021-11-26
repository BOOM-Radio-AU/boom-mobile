using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoomRadio.Model
{
    /// <summary>
    /// Collection of <see cref="Sponsors"/>
    /// </summary>
    public class SponsorsCollection
    {
        public List<Sponsors> sponsors;
        private DateTime lastUpdated;

        /// <summary>
        /// Constructor
        /// </summary>
        public SponsorsCollection()
        {
            sponsors = new List<Sponsors>();
        }

        /// <summary>
        /// Merges a new list of sponsors with the current list of sponsors, reusing
        /// existing sponsors where possible. This prevents needlessly repeating media api
        /// querires.
        /// </summary>
        /// <param name="shows">List of shows</param>
        public void MergeSponsors(List<Sponsors> newSponsors)
        {
            List<Sponsors> freshSponsors = new List<Sponsors>();
            foreach (Sponsors sponsor in newSponsors)
            {
                // Check if existing item in articles
                Sponsors existingSponsor = sponsors.Find(s => s.ID == sponsor.ID);
                // Can re-use the exisiting article if it is present
                if (existingSponsor != null)
                {
                    freshSponsors.Add(existingSponsor);
                }
                else
                {
                    freshSponsors.Add(sponsor);
                }
            }
            sponsors = freshSponsors;
        }

        /// <summary>
        /// Updates the collection fro the API, if needed
        /// </summary>
        /// <returns>Collection was updated</returns>
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
                List<Sponsors> fetchedSponsors = await Api.GetSponsorsAsync();
                if (fetchedSponsors.Count == 0)
                {
                    // Nothing returned from API
                    return false;
                }
                MergeSponsors(fetchedSponsors);
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
