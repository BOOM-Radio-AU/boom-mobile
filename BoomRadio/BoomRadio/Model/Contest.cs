using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoomRadio.Model
{
    public class Contest
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public Uri Link { get; private set; }
        public string MediaID { get; private set; }
        public string ImageUrl { get; private set; } = null;

        public Contest(int id, string title, string link, string mediaID)
        {
            Id = id;
            Title = title;
            Link = new Uri(link);
            MediaID = mediaID;
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
    }
}
