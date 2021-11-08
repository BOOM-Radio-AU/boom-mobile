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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsView : StackLayout, IUpdatableUI
    {
        public NewsCollection News;
        MainPage MainPage;
        public NewsView(NewsCollection news, MainPage mainPage)
        {
            InitializeComponent();
            News = news;
            MainPage = mainPage;
        }

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

        public void SetHorizontalDisplay()
        {
            // TODO;
        }

        public void SetVerticalDisplay()
        {
            // TODO;
        }
    }
}