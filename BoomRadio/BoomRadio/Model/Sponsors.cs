using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoomRadio.Model
{
    /// <summary>
    /// A sponsor of BOOM Radio
    /// </summary>
    public class Sponsors
    {
        public int ID { get; set; }
        public string SponsorName { get; set; }
        public string SponsorDescription { get; set; }
        public string SponsorImageQueryUrl { get; set; }
        public string SponsorImageUrl { get; set; }

        /// <summary>
        /// Sponsor for BOOM Radio
        /// </summary>
        /// <param name="id">Sponsor ID</param>
        /// <param name="sponsorName">Sponsor's name</param>
        /// <param name="sponsorDescription">Sponsor's description</param>
        /// <param name="imageURL"> Media api url to query to obtain the sponsor's image</param>
        public Sponsors(int id, string sponsorName, string sponsorDescription, string imageURL)
        {
            ID = id;
            SponsorName = sponsorName;
            SponsorDescription = sponsorDescription;
            SponsorImageQueryUrl = imageURL;

        }

        /// <summary>
        /// Extracts text content from a HTML snippet
        /// </summary>
        /// <param name="html">HTML snippet</param>
        /// <returns>Text content</returns>
        public string TextFromHTML(string html)
        {
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            string text = stripFormattingRegex.Replace(html, string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            return text.Trim();
        }

        /// <summary>
        /// Retrieves the sposnor's image url from the api
        /// </summary>
        /// <returns></returns>
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
