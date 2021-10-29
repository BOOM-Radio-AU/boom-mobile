using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoomRadio.Model
{
    public class SponsorsCollection
    {
        public List<Sponsors> sponsorlist;

        public SponsorsCollection()
        {
            sponsorlist = new List<Sponsors>();
        }


        public void ParseSponsors(string responseString)
        {
            List<Sponsors> freshSponsorList = new List<Sponsors>();
            JArray response;
            try
            {

                response = JsonConvert.DeserializeObject<JArray>(responseString);
                foreach (var item in response)
                {
                    // Parse values from JSON
                    int id = item.Value<int>("id");
                    string sponsorName = item.Value<JObject>("title").Value<string>("rendered"); ;
                    string sponsorDescription = item.Value<JObject>("content").Value<string>("rendered");
                    string imageURL = (item.Value<JObject>("_links").Value<JArray>("wp:featuredmedia")[0] as JObject).Value<string>("href");

                    Sponsors sponsor = new Sponsors(id, sponsorName, sponsorDescription);

                    // Check if existing item in articles
                    Sponsors existingSponsor = sponsorlist.Find(a => a.ID == id);
                    if (existingSponsor != null && existingSponsor.SponsorName == sponsor.SponsorName)
                    {
                        freshSponsorList.Add(existingSponsor);
                    }
                    else
                    {
                        freshSponsorList.Add(sponsor);
                    }

                }
                sponsorlist = freshSponsorList;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing articles\n" + e.Message);
            }
        }

    }

}
