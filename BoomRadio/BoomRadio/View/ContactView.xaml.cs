using BoomRadio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoomRadio.View
{
    /// <summary>
    /// View for contacting BOOM Radio
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactView : StackLayout, IUpdatableUI
    {
        public ContactView()
        {
            InitializeComponent();
            UpdateUI();
        }

        /// <summary>
        /// Updates the user interface
        /// </summary>
        public void UpdateUI()
        {
            SubmitButton.IsEnabled = FormISValid();
            SubjectEntry.BackgroundColor = Theme.GetColour("background");
            SubjectEntry.TextColor = Theme.GetColour("text");
            MessageEditor.BackgroundColor = Theme.GetColour("background");
            MessageEditor.TextColor = Theme.GetColour("text");
            facebookButton.BackgroundColor = Theme.GetColour("accent");
            facebookButton.TextColor = Color.White;
            instagramButton.BackgroundColor = Theme.GetColour("accent");
            instagramButton.TextColor = Color.White;
            twitterButton.BackgroundColor = Theme.GetColour("accent");
            twitterButton.TextColor = Color.White;
            HeadingBox.UpdateColours();
        }

        /// <summary>
        /// Sets the value of the email submission form text inputs
        /// </summary>
        /// <param name="subject">Subject for email</param>
        /// <param name="message">Message body for email</param>
        public void SetForm(string subject, string message)
        {
            SubjectEntry.Text = subject;
            MessageEditor.Text = message;
            UpdateUI();
        }

        /// <summary>
        /// Resets the email submission form text inputs to empty strings
        /// </summary>
        public void ResetForm()
        {
            SetForm(string.Empty, string.Empty);
        }

        /// <summary>
        /// Validates the email submission form text inputs
        /// </summary>
        /// <returns>True if valid, false if subject or message is missing</returns>
        bool FormISValid()
        {
            return !string.IsNullOrWhiteSpace(SubjectEntry.Text) && !string.IsNullOrWhiteSpace(MessageEditor.Text);
        }

        /// <summary>
        /// Handles changes to the contact form text inputs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactForm_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Handles submit button clicks 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            // Compose email to send from the device's email app(s)
            await Email.ComposeAsync(SubjectEntry.Text, MessageEditor.Text, "online@boomradio.com.au");
            // Then reset the form
            Device.BeginInvokeOnMainThread(() => ResetForm());
        }

        /// <summary>
        /// Handles facebook button clicks 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void facebookButton_Clicked(object sender, EventArgs e) => Launcher.OpenAsync("https://www.facebook.com/boomradioau/");

        /// <summary>
        /// Handles instagram button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instagramButton_Clicked(object sender, EventArgs e) => Launcher.OpenAsync("https://www.instagram.com/boomradioau/");

        /// <summary>
        /// Handles twitter button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void twitterButton_Clicked(object sender, EventArgs e) => Launcher.OpenAsync("https://twitter.com/boomradioau");

        public async void SetHorizontalDisplay()
        {
            while (this.Width == -1)
            {
                await Task.Delay(10);
            }
            this.Margin = new Thickness(this.Width / 8 + 10, 10);
            facebookButton.FontSize = 32;
            instagramButton.FontSize = 32;
            twitterButton.FontSize = 32;
        }

        public void SetVerticalDisplay()
        {
            this.Margin = new Thickness(10);
            facebookButton.FontSize = 25;
            instagramButton.FontSize = 25;
            twitterButton.FontSize = 25;
        }
    }
}