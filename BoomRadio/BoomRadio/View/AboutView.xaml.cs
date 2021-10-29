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

namespace BoomRadio.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : StackLayout , IUpdatableUI
    {

        public SponsorsCollection SponsC;
        public ObservableCollection<Sponsors> Sponsor { get; set; } = new ObservableCollection<Sponsors>();

        public AboutView(SponsorsCollection Sponsor)
        {
            InitializeComponent();
            SponsC = Sponsor;
            BindingContext = this;
        }


        public async void UpdateUI()
        {

            await SponsC.UpdateAsync();


            SponsorsLoading.IsVisible = true;
            SponsorsLoading.IsRunning = true;



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