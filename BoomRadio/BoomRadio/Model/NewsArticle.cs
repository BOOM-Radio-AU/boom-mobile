using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BoomRadio.Model
{
    public class NewsArticle
    {
        public DateTime DatePublished { get; private set; }
        public DateTime DateModified { get; private set; }
        public string Title { get; private set; }
        public string ContentHTML { get; private set; }
        public string Excerpt { get; private set; }

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

        public NewsArticle(string title, string content, string excerpt, string published, string modified)
        {
            Title = title;
            ContentHTML = content;
            Excerpt = TextFromHTML(excerpt);
            DatePublished = DateTime.Parse(published);
            DateModified = DateTime.Parse(modified);
        }
    }
}
