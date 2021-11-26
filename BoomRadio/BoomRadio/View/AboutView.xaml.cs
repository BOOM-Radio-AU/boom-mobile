using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit;
using BoomRadio.Model;
using BoomRadio.Components;

namespace BoomRadio.View
{
    /// <summary>
    /// View for information about BOOM Radio
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : StackLayout , IUpdatableUI
    {

        public NewsCollection collec = new NewsCollection();

        public SponsorsCollection SponsC;
        public ObservableCollection<Sponsors> Sponsor { get; set; } = new ObservableCollection<Sponsors>();

        // Some colour fields and properties for data binding
        private Color textColour = Theme.GetColour("text");
        private Color bgColour = Theme.GetColour("background");
        private double sponsorImageHeight = 300;
        public Color TextColour
        {
            get => textColour; private set
            {
                textColour = value;
                OnPropertyChanged("TextColour");
            }
        }
        public Color BgColour
        {
            get => bgColour;
            private set
            {
                bgColour = value;
                OnPropertyChanged("BgColour");
            }
        }
        public double SponsorImageHeight
        {
            get => sponsorImageHeight;
            private set
            {
                sponsorImageHeight = value;
                OnPropertyChanged("SponsorImageHeight");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Sponsor">BOOM RAdio's sponsors</param>
        public AboutView(SponsorsCollection Sponsor)
        {
            InitializeComponent();
            SponsC = Sponsor;
            BindingContext = this;
            collec.service = Api.Service.About;
        }

        /// <summary>
        /// Updates the UI
        /// </summary>
        public async void UpdateUI()
        {
            // Update colours
            TextColour = Theme.GetColour("text");
            BgColour = Theme.GetColour("background");
            AboutHeadingBox.UpdateColours();
            SponsorsHeadingBox.UpdateColours();

            await collec.UpdateAsync();

            if (collec.articles.Count > 0)
            {
                BoxesHome.Children.Clear();

                foreach (NewsArticle box in collec.articles)
                {
                    AboutFrame item = new AboutFrame(box);
                    BoxesHome.Children.Add(item);
                }
                    
            }

            SponsorsLoading.IsVisible = true;
            SponsorsLoading.IsRunning = true;

            await SponsC.UpdateAsync();

            if (Sponsor.Count != SponsC.sponsors.Count)
            {
                Sponsor.Clear();
                List<Task> imageFetches = new List<Task>();



                foreach (var sponsorItem in SponsC.sponsors)
                {
                    if (sponsorItem.SponsorImageUrl == null)
                    {
                        imageFetches.Add(sponsorItem.UpdateImageUrl());
                    }

                }
                await Task.WhenAll(imageFetches.ToArray());

                foreach (var showItem in SponsC.sponsors)
                {
                    Sponsor.Add(showItem);
                }

            }

            SponsorsLoading.IsVisible = false;
            SponsorsLoading.IsRunning = false;
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
            this.Margin = new Thickness(this.Width / 8 + 10, 10);
            SponsorImageHeight = 150;
        }

        /// <summary>
        /// Sets the view for vertical display
        /// </summary>
        public async void SetVerticalDisplay()
        {
            await Task.Delay(10);
            this.Margin = new Thickness(5,10);
            SponsorImageHeight = 300;
        }
    }
}