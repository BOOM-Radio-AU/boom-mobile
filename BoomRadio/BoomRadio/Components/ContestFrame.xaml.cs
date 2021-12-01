using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.Components
{
    /// <summary>
    /// A snippet view of a contest, including image and title
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContestFrame : Frame
    {
        /// <summary>
        /// Contest for the snippet view
        /// </summary>
        Contest Contest;
        MainPage MainPage;

        /// <summary>
        /// Initialises a new instance
        /// </summary>
        /// <param name="contest">Contest for the snippet view</param>
        /// <param name="mainPage">Main page</param>
        public ContestFrame(Contest contest, MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            Contest = contest;
            TitleLabel.Text = Contest.Title;
            UpdateColours();
            GetImageAsync();
        }

        /// <summary>
        /// Updates the colours used
        /// </summary>
        public void UpdateColours()
        {
            this.BackgroundColor = Theme.GetColour("background");
            TitleLabel.TextColor = Theme.GetColour("text");
            LinkButton.BackgroundColor = Theme.GetColour("accent");
        }

        /// <summary>
        /// Sets the image source from the news article
        /// </summary>
        private async void GetImageAsync()
        {

            // If the article already has an image url specified, just use that
            if (Contest.ImageUrl != null)
            {
                ContestImage.Source = ImageSource.FromUri(new Uri(Contest.ImageUrl));
                return;
            }

            // If the internet is not connected, silently skip trying to show the image
            if (!MainPage.HasInternet(true))
            {
                ContestImage.IsVisible = false;
                return;
            }

            // Otherwise, wait for the url to be fetched and then use it
            try
            {
                await Contest.UpdateImageUrl();
                if (Contest.ImageUrl != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContestImage.Source = ImageSource.FromUri(new Uri(Contest.ImageUrl));
                        ContestImage.IsVisible = true;
                    });
                }
            }
            catch(Exception e)
            {
                DependencyService.Get<ILogging>().Error(this, e);
                Console.WriteLine("Error with news article image\n " + e.Message);
            }
        }

        /// <summary>
        /// Handles tap events to open the contest in the device's browser 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LinkButton_Clicked(object sender, EventArgs e) => await Launcher.OpenAsync(Contest.Link);
    }
}