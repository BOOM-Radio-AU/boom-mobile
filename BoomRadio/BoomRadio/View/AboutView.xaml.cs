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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : StackLayout , IUpdatableUI
    {

        public NewsCollection collec = new NewsCollection();

        public SponsorsCollection SponsC;
        public ObservableCollection<Sponsors> Sponsor { get; set; } = new ObservableCollection<Sponsors>();

        // Some colour fields and properties for data binding
        private Color textColour = Theme.GetColour("text");
        private Color bgColour = Theme.GetColour("background");
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

        public AboutView(SponsorsCollection Sponsor)
        {
            InitializeComponent();
            SponsC = Sponsor;
            BindingContext = this;
            collec.service = Api.Service.About;

        }


        public async void UpdateUI()
        {
            // Update colours
            TextColour = Theme.GetColour("text");
            BgColour = Theme.GetColour("background");
            AboutHeadingBox.UpdateColours();
            SponsorsHeadingBox.UpdateColours();

            //BUG HERE SOMEWHERE IDK HOW TO FIX IT COME BACK LATER
            //BOXES DONT OPEN WHEN PAGE IS RELOADED/CANT SCROLL PAGE

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

      
    }
}