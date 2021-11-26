using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPage">Main (parent) page</param>
        public NewsArticleView(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            if (mainPage.Width > mainPage.Height)
            {
                SetHorizontalDisplay();
            }
        }

        /// <summary>
        /// Resizes an image from the default size (width/height not set, AspectFit)
        /// to fill the available width and have a height that mantains the aspect ratio.
        /// The height needs to be calculated from the aspect ratio and the width since
        /// Image elements do not allow for "auto" sizing.
        /// </summary>
        /// <param name="image">Image to be resized</param>
        private async void ResizeImage(Image image)
        {
            // Wait for image to be layed out on page
            do
            {
                await Task.Delay(15);
            } while (image.Width <= 0 || image.Height <= 0 || this.Width <= 0);
            await Task.Delay(2); // Another short delay to be sure
            Device.BeginInvokeOnMainThread(() =>
            {
                // Get the aspect ratio and calculate the required width and height
                double aspectRatio = image.Width / image.Height;
                double width = this.Width - 20;
                double height = width / aspectRatio;
                // Set the image to the required size, on the main (UI) thread
                image.WidthRequest = width;
                image.HeightRequest = height;
                image.Aspect = Aspect.AspectFit;
            });
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
                ResizeImage(NewsImage);
                ImageFrame.IsVisible = true;
                ImageFrame.BackgroundColor = Theme.GetColour("accent");

            }
            else
            {
                NewsImage.IsVisible = false;
                ImageFrame.IsVisible = false;
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
                    Image img = new Image()
                    {
                        Source = ImageSource.FromUri(new Uri(imageUrls.Dequeue())),
                        HorizontalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFit,
                        BackgroundColor = Color.Black
                    };
                    ContentStackLayout.Children.Add(img);
                    ResizeImage(img);
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

        /// <summary>
        /// Resizes all images. Needed for when the view changes between vertical and horizontal
        /// </summary>
        private void ResizeAllImages()
        {
            if (Article?.ImageUrl != null)
            {
                NewsImage.WidthRequest = -1;
                NewsImage.HeightRequest = -1;
                ResizeImage(NewsImage);
            }
            foreach (var item in ContentStackLayout.Children)
            {
                Image img = item as Image;
                if (img != null)
                {
                    img.WidthRequest = -1;
                    img.HeightRequest = -1;
                    ResizeImage(img);
                }
            }
        }

        /// <summary>
        /// Sets the view for horizontal display
        /// </summary>
        public async void SetHorizontalDisplay()
        {
            while (this.Width == -1)
            {
                await Task.Delay(10);
            }
            Margin = new Thickness(this.Width / 8, 0, this.Width / 8, 10);
            ResizeAllImages();
        }

        /// <summary>
        /// Sets the veiw for vertical display
        /// </summary>
        public void SetVerticalDisplay()
        {
            Margin = new Thickness(10, 0, 10, 10);
            ResizeAllImages();
        }
    }
}