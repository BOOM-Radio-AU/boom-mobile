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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsSnippet : Frame
    {
        NewsArticle Article;
        public NewsSnippet(NewsArticle article)
        {
            InitializeComponent();
            Article = article;
            TitleLabel.Text = Article.Title;
            ExcertLabel.Text = Article.Excerpt;
            GetImage();
        }

        private async void GetImage()
        {
            if (Article.ImageUrl != null)
            {
                NewsImage.Source = ImageSource.FromUri(new Uri(Article.ImageUrl));
                return;
            }

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

    }
}