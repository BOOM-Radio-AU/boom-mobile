using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoomRadio.Model
{
    public class Sponsors
    {
        public int ID { get; set; }
        public string SponsorName { get; set; }
        public string SponsorDescription { get; set; }
        public string SponsorImageQueryUrl { get; set; }
        public string SponsorImageUrl { get; set; }

        public Sponsors(int id, string sponsorName, string sponsorDescription, string sponsorImageQueryURL, string sponsorImageURL)
        {

            ID = id;
            SponsorName = sponsorName;
            SponsorDescription = sponsorDescription;
            SponsorImageQueryUrl = sponsorImageQueryURL;
            SponsorImageUrl = sponsorImageURL;

        }

        public Sponsors(int id, string sponsorName, string sponsorDescription, string imageURL)
        {
            ID = id;
            SponsorName = sponsorName;
            SponsorDescription = sponsorDescription;
            SponsorImageQueryUrl = imageURL;

        }

        public string TextFromHTML(string html)
        {
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            string text = stripFormattingRegex.Replace(html, string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            return text.Trim();
        }

        public async Task UpdateImageUrl()
        {
            string url = await Api.GetImageFromQueryAsync(SponsorImageQueryUrl);
            if (url != null)
            {
                SponsorImageUrl = url;
            }
        }


    }
}
