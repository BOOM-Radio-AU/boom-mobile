using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeadingBox : Frame
    {
        private string text;
        private Color borderColour;
        private Color backgroundColour;
        private Color textColour;
        private string fontfamily = "CG-B";

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        public Color BorderColour
        {
            get => borderColour;
            set
            {
                borderColour = value;
                OnPropertyChanged("BorderColour");
            }
        }
        public Color BackgroundColour
        {
            get => backgroundColour;
            set
            {
                backgroundColour = value;
                OnPropertyChanged("BackgroundColour");
            }
        }
        public Color TextColour
        {
            get => textColour;
            set
            {
                textColour = value;
                OnPropertyChanged("TextColour");
            }
        }

        public string FontFamily
        {
            get => fontfamily;
            set
            {
                fontfamily = value;
                OnPropertyChanged("FontFamily");
            }
        }

        public HeadingBox()
        {
            InitializeComponent();
            BindingContext = this;
            UpdateColours();
        }

        public void UpdateColours()
        {
            BorderColour = Theme.GetColour("accent");
            BackgroundColour = Theme.GetColour("background");//.MultiplyAlpha(0.9);
            TextColour = Theme.GetColour("accent");
            
        }
    }
}