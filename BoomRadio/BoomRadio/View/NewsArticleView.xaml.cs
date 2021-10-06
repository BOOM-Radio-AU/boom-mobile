using BoomRadio.Model;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.View
{
    /// <summary>
    /// View for displaying a news article in full
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsArticleView : StackLayout, IUpdatableUI
    {
        public NewsArticle Article { get; set; }
        MainPage MainPage;
        public NewsArticleView(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
        }

        /// <summary>
        /// Updates the user interface
        /// </summary>
        public void UpdateUI()
        {
            if (Article == null) return;

            if (Article.ImageUrl != null)
            {
                NewsImage.Source = ImageSource.FromUri(new Uri(Article.ImageUrl));
                NewsImage.IsVisible = true;
            }
            else
            {
                NewsImage.IsVisible = false;
            }
            HeadlineLabel.Text = Article.Title;

            // Note: WebView cannot be used as height does not autofit content and scrolling is not possible,
            // see https://github.com/xamarin/Xamarin.Forms/issues/1711 and https://github.com/xamarin/Xamarin.Forms/issues/6351
            // Instead, the content is displayed in a Label with TextType="Html". However, this does not
            // display images, so they need to be stripped out.

            // Regex to match any character between '<img' and '>', even when end tag is missing
            Regex stripFormattingRegex = new Regex(@"<img[^>]*(>|$)", RegexOptions.Multiline);
            // Remove all img tags
            string content = stripFormattingRegex.Replace(Article.ContentHTML, string.Empty);
            // Display the content (whithout images) in html-formatted label
            ContentLabel.Text = content;

            // TODO: A better option would be to parse the content for image urls, and then
            // add Image elements with the source set to those urls. Multiple Label elements with TextType="Html"
            // would need to be used for content between each image.
        }

        /// <summary>
        /// Handles the back button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            MainPage.Navigate("news");
        }
    }
}