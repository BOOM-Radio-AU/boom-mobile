using BoomRadio.Components;
using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.View
{
    /// <summary>
    /// View for the collection of news stories, displayed as snippets
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsView : StackLayout, IUpdatableUI
    {
        public NewsCollection News;
        MainPage MainPage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="news">News stories collection</param>
        /// <param name="mainPage">Main (parent) page</param>
        public NewsView(NewsCollection news, MainPage mainPage)
        {
            InitializeComponent();
            News = news;
            MainPage = mainPage;
        }

        /// <summary>
        /// Updates the UI
        /// </summary>
        public async void UpdateUI()
        {
            HeadingBox.UpdateColours();
            // Don't try to update without internet connection
            if (!MainPage.HasInternet())
            {
                return;
            }
            // Show the loading indicator
            NewsLoadingIndicator.IsVisible = true;
            NewsLoadingIndicator.IsRunning = true;
            // Update the News collection, and then the UI if needed
            bool shouldUpdateUI = await News.UpdateAsync();
            if (shouldUpdateUI)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    NewsStacklayout.Children.Clear();
                    foreach (NewsArticle article in News.articles)
                    {
                        NewsSnippet item = new NewsSnippet(article, MainPage);

                        NewsStacklayout.Children.Add(item);
                    }
                });
            }
            else
            {
                // Just update the colours
                foreach (NewsSnippet snippet in NewsStacklayout.Children)
                {
                    snippet.UpdateColours();
                }
            }
            // Hide the loading indicator
            NewsLoadingIndicator.IsVisible = false;
            NewsLoadingIndicator.IsRunning = false;
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
            NewsStacklayout.Margin = new Thickness(this.Width / 8, 5);
        }

        /// <summary>
        /// Sets the view for vertical display
        /// </summary>
        public void SetVerticalDisplay()
        {
            NewsStacklayout.Margin = new Thickness(5);
        }
    }
}