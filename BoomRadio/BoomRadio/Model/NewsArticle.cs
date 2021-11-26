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
    /// <summary>
    /// A news article or similar post 
    /// </summary>
    public class NewsArticle
    {
        public int ID { get; private set; }
        public DateTime DatePublished { get; private set; }
        public DateTime DateModified { get; private set; }
        public string Title { get; private set; }
        public string ContentHTML { get; private set; }
        public string Excerpt { get; private set; }
        public string MediaID { get; private set; }
        public string ImageUrl { get; private set; } = null;

        /// <summary>
        /// Extracts text content from a HTML string
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <returns>Text content</returns>
        public string TextFromHTML(string html)
        {
            // Regex to match any character between '<' and '>', even when end tag is missing
            Regex stripFormattingRegex = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            // Remove all tags
            string text = stripFormattingRegex.Replace(html, string.Empty);
            // Decode any encoded entities
            text = System.Net.WebUtility.HtmlDecode(text);
            // Trim off any extra whitespace
            return text.Trim();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Article ID</param>
        /// <param name="title">Article title</param>
        /// <param name="content">Article content</param>
        /// <param name="excerpt">Short excerpt</param>
        /// <param name="published">Publication date or information</param>
        /// <param name="modified">Modification date or information</param>
        /// <param name="mediaId">ID of feratured media to fetch from the media API</param>
        public NewsArticle(int id, string title, string content, string excerpt, string published, string modified, string mediaId)
        {
            ID = id;
            Title = title;
            ContentHTML = content;
            Excerpt = TextFromHTML(excerpt);
            DateTime dPub;
            if (DateTime.TryParse(published, out dPub)) {
                DatePublished = dPub;
            }
            DateTime dMod;
            if (DateTime.TryParse(modified, out dMod)) {
                DatePublished = dMod;
            }
            MediaID = mediaId;
        }

        /// <summary>
        /// Updates the image url, after getting it from from the API
        /// </summary>
        public async Task UpdateImageUrl()
        {
            if (MediaID != null)
            {
                string url = await Api.GetImageUrlAsync(MediaID);
                if (url != null)
                {
                    ImageUrl = url;
                }
            }
        }

        /// <summary>
        /// Splits the content html into chunks of text (non-image) content, removing image tags
        /// </summary>
        /// <returns>text (non-image) content chunks</returns>
        public Queue<string> ContentTextChunks()
        {
            Regex imagesRegex = new Regex(@"<img[^>]*>", RegexOptions.IgnoreCase);
            return new Queue<string>(imagesRegex.Split(ContentHTML));
            
        }

        /// <summary>
        /// Parses the content for image source urls
        /// </summary>
        /// <returns>Image source urls</returns>
        public Queue<string> ContentImageUrls()
        {
            Queue<string> imgUrls = new Queue<string>();
            Regex imagesRegex = new Regex(@"<img[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection imgTags = imagesRegex.Matches(ContentHTML);
            foreach (Match imgTagMatch in imgTags) {
                // Parse url from src attribute. Based on https://stackoverflow.com/questions/4257359/regular-expression-to-get-the-src-of-images-in-c-sharp
                string srcUrl = Regex.Match(imgTagMatch.Value, "<img.+?src=[\"'](.+?)[\"'][^>]*>", RegexOptions.IgnoreCase).Groups[1].Value;
                imgUrls.Enqueue(srcUrl);
            }
            return imgUrls;
        }
    }
}
