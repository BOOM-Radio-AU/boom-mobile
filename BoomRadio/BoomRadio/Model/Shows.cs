using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BoomRadio.Model
{
    /// <summary>
    /// BOOM Radio show
    /// </summary>
    public class Shows
    {
        public int ID { get; set; }
        public string ShowTitle { get; set; }
        public string ShowSchedule { get; set; }
        public string ShowDescription { get; set; }
        public string ShowImageQueryUrl { get; set; }
        public string ShowImageUrl { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Show ID</param>
        /// <param name="title">Show title</param>
        /// <param name="time">Show time/scheduke</param>
        /// <param name="description">Show description</param>
        /// <param name="imageURL">Image URL</param>
        public Shows(int id, string title, string time, string description, string imageURL)
        {
            ID = id;
            ShowTitle = title;
            ShowSchedule = time;
            ShowDescription = description;
            ShowImageQueryUrl = imageURL;
        }

        /// <summary>
        /// Extracts text content from a HTML snippet, stripping any tags and decoding HTML entities
        /// </summary>
        /// <param name="html">HTML snippet</param>
        /// <returns></returns>
        public string TextFromHTML(string html)
        {
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            string text = stripFormattingRegex.Replace(html, string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            return text.Trim();
        }

        /// <summary>
        /// Updates the image url, after getting it from from the API
        /// </summary>
        /// <returns>image url</returns>
        public async Task UpdateImageUrl()
        {
            string url = await Api.GetImageFromQueryAsync(ShowImageQueryUrl);
            if (url != null)
            {
                ShowImageUrl = url;
            }
        }
    }
}
