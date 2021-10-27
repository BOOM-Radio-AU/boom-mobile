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
    public class Shows
    {


        public int ID { get; set; }
        public string ShowTitle { get; set; }
        public string ShowSchedule { get; set; }
        public string ShowDescription { get; set; }
        public string ShowImageQueryUrl { get; set; }
        public string ShowImageUrl { get; set; }


        private readonly HttpClient client = new HttpClient();
        public string MediaApiPrefix = "https://boomradio.com.au/wp-json/wp/v2/schedule/";



        public Shows(int id, string title, string time, string description, string imageURL)
        {
            ID = id;
            ShowTitle = title;
            ShowSchedule = time;
            ShowDescription = description;
            ShowImageQueryUrl = imageURL;
        }


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

        public static implicit operator ObservableCollection<object>(Shows v)
        {
            throw new NotImplementedException();
        }
    }
}
