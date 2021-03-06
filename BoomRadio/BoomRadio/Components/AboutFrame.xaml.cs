using System;
using System.Collections.Generic;
using BoomRadio.Model;
using Xamarin.Forms;

namespace BoomRadio.Components
{
    /// <summary>
    /// Collapsbale box for a section describing BOOM Radio
    /// </summary>
    public partial class AboutFrame : Frame
    {

        NewsArticle Article;
        private bool isExpanded;

        public bool IsExpanded {
            get => isExpanded;
            set
            {
                isExpanded = value;
                ButtonLabel.Text = value ? "Chevron-Up" : "Chevron-Down";
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="article">Article for a section describing BOOM Radio</param>
        public AboutFrame(NewsArticle article)
        {
            InitializeComponent();
            BindingContext = this;
            Article = article;
            TitleLabel.Text = Article.Title;
            UpdateBox();
            UpdateColours();

        }

        /// <summary>
        /// Updates the colors used
        /// </summary>
        public void UpdateColours()
        {
            this.BackgroundColor = Theme.GetColour("background");
            BackPanel.BackgroundColor = Theme.GetColour("background");
            BackFrame2.BackgroundColor = Theme.GetColour("background");
            BackFrame.BackgroundColor = Color.Transparent;
            TitleLabel.TextColor = Theme.GetColour("text");
            ButtonLabel.TextColor = Theme.GetColour("accent");
        }

        /// <summary>
        /// Updates the box content
        /// </summary>
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
        
        /// <summary>
        /// Handles clicks on the chevron button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonLabel_Clicked(System.Object sender, System.EventArgs e)
        {
            if (IsExpanded)
            {
                ButtonLabel.Text = "Chevron-Down";
            }
            else
            {
                ButtonLabel.Text = "Chevron-Up";

            }
        }
    }
}

