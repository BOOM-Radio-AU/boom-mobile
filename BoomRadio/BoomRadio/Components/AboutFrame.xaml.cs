using System;
using System.Collections.Generic;
using BoomRadio.Model;
using Xamarin.Forms;

namespace BoomRadio.Components
{
    public partial class AboutFrame : Frame
    {

        NewsArticle Article;

        public AboutFrame(NewsArticle article)
        {
            InitializeComponent();
            Article = article;
            TitleLabel.Text = Article.Title;
            UpdateBox();
            UpdateColours();

        }


        public void UpdateColours()
        {
            this.BackgroundColor = Theme.GetColour("background");
            BackPanel.BackgroundColor = Theme.GetColour("background");
            TitleLabel.TextColor = Theme.GetColour("text");
        }

        public async void UpdateBox()
        {

            await Article.UpdateImageUrl();
            Device.BeginInvokeOnMainThread(() =>
            {

                if (Article != null && Article.ImageUrl != null)
                {
                    
                    MainImage.Source = ImageSource.FromUri(new Uri(Article.ImageUrl));
                    MainImage.IsVisible = true;
                }
                else
                {
                    MainImage.IsVisible = false;
                }

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
                            FontFamily = "MET-L"
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
         );
        }
    }
}

