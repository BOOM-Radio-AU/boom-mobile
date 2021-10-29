using BoomRadio.Model;
using System;
using System.Collections.Generic;
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
            // Instead, the content can displayed in Labels with TextType="Html". However, this does not
            // display images. Therefore, the content is separarted into chunks of text (non-image) content and
            // urls of images to be inserted between each chunk. 
            Queue<string> textChunks = Article.ContentTextChunks();
            Queue<string> imageUrls = Article.ContentImageUrls();
            // Clear any previous content
            ContentStackLayout.Children.Clear();
            ContentStackLayout.BackgroundColor = Theme.GetColour("background");
            // Iteratively insert a chunk of text and then an image until all content has been inserted.
            while (textChunks.Count + imageUrls.Count > 0)
            {
                if (textChunks.Count > 0)
                {
                    ContentStackLayout.Children.Add(new Label()
                    {
                        Text = textChunks.Dequeue(),
                        TextType = TextType.Html,
                        FontSize = 16,
                        TextColor = Theme.GetColour("text"),
                        FontFamily ="MET-L"
                    });
                }
                if (imageUrls.Count > 0)
                {
                    ContentStackLayout.Children.Add(new Image()
                    {
                        Source = ImageSource.FromUri(new Uri(imageUrls.Dequeue())),
                        MinimumHeightRequest = 150,
                        Aspect = Aspect.AspectFit
                    });
                }
            }
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