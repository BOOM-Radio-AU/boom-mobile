using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        public string ShowImage { get; set; }


        private readonly HttpClient client = new HttpClient();
        public string MediaApiPrefix = "https://boomradio.com.au/wp-json/wp/v2/schedule/";



        public Shows(int id, string title, string time, string description, string imageURL)
        {
            ID = id;
            ShowTitle = title;
            ShowSchedule = time;
            ShowDescription = description;
            ShowImage = imageURL;
        }



        public string TextFromHTML(string html)
        {
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            string text = stripFormattingRegex.Replace(html, string.Empty);
            text = System.Net.WebUtility.HtmlDecode(text);
            return text.Trim();
        }






    }
}
