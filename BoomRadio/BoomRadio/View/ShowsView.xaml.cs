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
    /// <summary>
    /// View for BOOM Radio's shows
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowsView : StackLayout, IUpdatableUI
    {

        public ShowsCollection ShowC;
        MainPage MainPage;

        public ObservableCollection<Shows> Show { get; set; } = new ObservableCollection<Shows>();

        // Some colour fields and properties for data binding
        private Color textColour = Theme.GetColour("text");
        private Color bgColour = Theme.GetColour("background");
        private StackOrientation showsOrientation = StackOrientation.Vertical;
        private double showsTextWidth = -1;
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
        public StackOrientation ShowsOrientation
        {
            get => showsOrientation;
            private set {
                showsOrientation = value;
                OnPropertyChanged("ShowsOrientation");
            }
        }
        public double ShowsTextWidth
        {
            get => showsTextWidth;
            private set {
                showsTextWidth = value;
                OnPropertyChanged("ShowsTextWidth");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="show">Shows collection</param>
        /// <param name="mainPage">Main (parent) page</param>
        public ShowsView(ShowsCollection show, MainPage mainPage)

        {
            InitializeComponent();
            ShowC = show;
            MainPage = mainPage;
            BindingContext = this;
        }

        /// <summary>
        /// Updates the UI
        /// </summary>
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

        /// <summary>
        /// Sets the view for horizontal display
        /// </summary>
        public async void SetHorizontalDisplay()
        {
            while (this.Width == -1)
            {
                await Task.Delay(10);
            }
            ShowsOrientation = StackOrientation.Horizontal;
            ShowsTextWidth = this.Width / 2;
        }

        /// <summary>
        /// Sets the view for vertical display
        /// </summary>
        public void SetVerticalDisplay()
        {
            ShowsOrientation = StackOrientation.Vertical;
            ShowsTextWidth = -1;
        }
    }
}