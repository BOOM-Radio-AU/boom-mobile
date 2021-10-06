using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.Components
{
    /// <summary>
    /// A snippet view of a news story, including image and headline
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsSnippet : Frame
    {
        /// <summary>
        /// News article for the snippet view
        /// </summary>
        NewsArticle Article;
        MainPage MainPage;

        /// <summary>
        /// Initialises a new instance
        /// </summary>
        /// <param name="article">News article for the snippet view</param>
        public NewsSnippet(NewsArticle article, MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            Article = article;
            TitleLabel.Text = Article.Title;
            ExcertLabel.Text = Article.Excerpt;
            GetImageAsync();
        }

        /// <summary>
        /// Sets the image source from the news article
        /// </summary>
        private async void GetImageAsync()
        {
            // If the article already has an image url specified, just use that
            if (Article.ImageUrl != null)
            {
                NewsImage.Source = ImageSource.FromUri(new Uri(Article.ImageUrl));
                return;
            }

            // Otherwise, wait for the url to be fetched and then use it
            try
            {
                string imageUrl = await Article.UpdateImageUrl();
                if (imageUrl != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        NewsImage.Source = ImageSource.FromUri(new Uri(Article.ImageUrl));
                    });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with news article image\n " + e.Message);
            }
        }

        /// <summary>
        /// Handles tap events to open the news article 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MainPage.NavigateToNewsArticle(Article);
        }
    }
}