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
        public NewsView(NewsCollection news)
        {
            InitializeComponent();
            News = news;
        }

        public async void UpdateUI()
        {
            bool shouldUpdate = await News.UpdateAsync();
            if (shouldUpdate)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    NewsStacklayout.Children.Clear();
                    foreach (NewsArticle article in News.articles)
                    {
                        NewsSnippet item = new NewsSnippet(article);
                        NewsStacklayout.Children.Add(item);
                    }
                });
            }
        }
    }
}