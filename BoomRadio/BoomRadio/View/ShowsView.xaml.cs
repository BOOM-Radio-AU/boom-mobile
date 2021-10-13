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
    public partial class ShowsView : StackLayout, IUpdatableUI
    {

        public ShowsCollection ShowC;
        MainPage MainPage;
        List<ShowFrame> frameList = new List<ShowFrame>();


        private int index = 0;

        public int Index
        {
            get
            {
                return index;
            }
            private set
            {
                if(value >=0 && value < frameList.Count)
                {
                    index = value;
                    OnPropertyChanged("Index");
                }
            }
        }

        public ShowsView(ShowsCollection show, MainPage mainPage)

        {
            InitializeComponent();
            ShowC = show;
            MainPage = mainPage;
        }

        public async void UpdateUI()
        {

            // Don't try to update without internet connection
            if (!MainPage.HasInternet())
            {
                return;
            }
            // Show the loading indicator
            ShowsLoadingIndicator.IsVisible = true;
            ShowsLoadingIndicator.IsRunning = true;
            // Update the News collection, and then the UI if needed
            bool shouldUpdateUI = await ShowC.UpdateAsync();
            if (shouldUpdateUI)
            {

                Device.BeginInvokeOnMainThread(() =>
                {


                    foreach (Shows show in ShowC.shows)
                    {
                        ShowFrame item = new ShowFrame(show, MainPage);
                        frameList.Add(item);

                    }

                    ShowCarousel.Children.Add(frameList[Index]);



                });
            }




            // Hide the loading indicator
            ShowsLoadingIndicator.IsVisible = false;
            ShowsLoadingIndicator.IsRunning = false;
        }

        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            ShowCarousel.TranslationX = ShowCarousel.TranslationX + e.TotalX;

            if (e.StatusType == GestureStatus.Completed && ShowCarousel.TranslationX > (ShowCarousel.Width / 4))
            {
                if (Index == 0)
                {
                    await ShowCarousel.TranslateTo(0, 0, 250);
                    return;
                }

                SwipeLeft();
                await ShowCarousel.TranslateTo(0, 0, 250);
                ShowCarousel.IsVisible = false;
                ShowCarousel.Children.Clear();
                ShowCarousel.Children.Add(frameList[Index]);
                ShowCarousel.IsVisible = true;
                ShowCarousel.TranslationX = -ShowCarousel.Width;
                await ShowCarousel.TranslateTo(0, 0, 250);



            }

            if (e.StatusType == GestureStatus.Completed && ShowCarousel.TranslationX < -(ShowCarousel.Width / 4))
            {
                if (Index == frameList.Count - 1)
                {
                    await ShowCarousel.TranslateTo(ShowCarousel.Width, 0, 250);
                    return;
                }
                SwipeRight();
                await ShowCarousel.TranslateTo(0, 0, 250);
                ShowCarousel.IsVisible = false;
                ShowCarousel.Children.Clear();

                ShowCarousel.Children.Add(frameList[Index]);
                ShowCarousel.IsVisible = true;

                ShowCarousel.TranslationX = -ShowCarousel.Width;
                await ShowCarousel.TranslateTo(0, 0, 250);
            }

        }

        private void SwipeLeft()
        {
            Index--;


        }

        private void SwipeRight()
        {
            Index++;



        }
    }
}