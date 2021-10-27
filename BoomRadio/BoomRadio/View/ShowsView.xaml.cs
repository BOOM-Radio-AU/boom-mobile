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


        public ShowsView(ShowsCollection show, MainPage mainPage)

        {
            InitializeComponent();
            ShowC = show;
            MainPage = mainPage;
            BindingContext = this;
            

        }

        public async void UpdateUI()
        {


            // Don't try to update without internet connection
            if (!MainPage.HasInternet())
            {
                return;
            }

            await ShowC.UpdateAsync();
            
            // Show the loading indicator
            ShowsLoadingIndicator.IsVisible = true;
            ShowsLoadingIndicator.IsRunning = true;
            // Update the News collection, and then the UI if needed
            Show.Clear();
            List<Task> imageFetches = new List<Task>();
          
            foreach (var showItem in ShowC.shows)
            {
                if(showItem.ShowImageUrl == null)
                {
                    imageFetches.Add(showItem.UpdateImageUrl());
                }
           
            }
            await Task.WhenAll(imageFetches.ToArray());

            foreach (var showItem in ShowC.shows)
            {
                Show.Add(showItem);
            }

            // Hide the loading indicator
            ShowsLoadingIndicator.IsVisible = false;
            ShowsLoadingIndicator.IsRunning = false;
        }




    }
}