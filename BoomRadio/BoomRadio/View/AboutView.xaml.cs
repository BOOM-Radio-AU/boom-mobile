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
    public partial class AboutView : StackLayout
    {

        //public SponsorsCollection SponsC;
       // public ObservableCollection<Sponsors> Sponsor { get; set; } = new ObservableCollection<Sponsors>();

        public AboutView()
        {
            InitializeComponent();
           // SponsC = Sponsor;
          //  BindingContext = this;
        }

      
    }
}