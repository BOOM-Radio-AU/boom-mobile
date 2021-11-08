using BoomRadio.Components;
using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowsView : StackLayout, IUpdatableUI
    {

        public ShowsCollection ShowC;
        MainPage MainPage;

        public ObservableCollection<Shows> Show { get; set; } = new ObservableCollection<Shows>();

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
            private set {
                bgColour = value;
                OnPropertyChanged("BgColour");
            }
        }

        public ShowsView(ShowsCollection show, MainPage mainPage)

        {
            InitializeComponent();
            ShowC = show;
            MainPage = mainPage;
            BindingContext = this;


        }

        public async void UpdateUI()
        {
            // Update colours
            TextColour = Theme.GetColour("text");
            BgColour = Theme.GetColour("background");
            HeadingBox.UpdateColours();

            // Don't try to update without internet connection
            if (!MainPage.HasInternet())
            {
                return;
            }

            // Show the loading indicator
            ShowsLoadingIndicator.IsVisible = true;
            ShowsLoadingIndicator.IsRunning = true;

            await ShowC.UpdateAsync();

            // Update the News collection, and then the UI if needed
            if (Show.Count != ShowC.shows.Count)
            {
                Show.Clear();
                List<Task> imageFetches = new List<Task>();


                foreach (var showItem in ShowC.shows)
                {
                    if (showItem.ShowImageUrl == null)
                    {
                        imageFetches.Add(showItem.UpdateImageUrl());
                    }

                }
                await Task.WhenAll(imageFetches.ToArray());

                foreach (var showItem in ShowC.shows)
                {
                    Show.Add(showItem);
                }

            }

            // Hide the loading indicator
            ShowsLoadingIndicator.IsVisible = false;
            ShowsLoadingIndicator.IsRunning = false;
        }




    }
}