using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoomRadio.Components;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.Components
{
    /// <summary>
    /// The frame for a show, including schedule and description
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowFrame : ContentView
    {
        /// <summary>
        /// Show for the frame
        /// </summary>
        Shows Show ;
        MainPage MainPage;

        /// <summary>
        /// Initialises a new instance
        /// </summary>
        /// <param name="show">Show for the ShowFrame view</param>
        public ShowFrame(Shows show, MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            Show = show;
            TitleLabel.Text = show.ShowTitle;
            Description.Text = show.ShowDescription;
            TimeLabel.Text = show.ShowSchedule;
            GetImageAsync();
        }

        /// <summary>
        /// Sets the image source from the news article
        /// </summary>
        private async void GetImageAsync()
        {

            // If the article already has an image url specified, just use that
            if (Show.ShowImageUrl != null)
            {
                ShowImage.Source = ImageSource.FromUri(new Uri(Show.ShowImageUrl));
                return;
            }

            // If the internet is not connected, silently skip trying to show the image
            if (!MainPage.HasInternet(true))
            {
                ShowImage.IsVisible = false;
                return;
            }

            // Otherwise, wait for the url to be fetched and then use it
            try
            {
                string imageUrl = await Show.UpdateImageUrl();
                if (imageUrl != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ShowImage.Source = ImageSource.FromUri(new Uri(Show.ShowImageUrl));
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with show image\n " + e.Message);
            }
        }

    }
}