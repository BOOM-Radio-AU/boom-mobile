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
        /// <summary>
        /// Constructor
        /// </summary>
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
            SubmitButton.BackgroundColor = Theme.GetColour("accent");
            SubmitButton.TextColor = Color.White;
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
            ShowEmailFormButton.BackgroundColor = Theme.GetColour("accent");
            ShowEmailFormButton.TextColor = Color.White;
            HeadingBox.UpdateColours();
            ContactsGrid.BackgroundColor = Theme.GetColour("background");
            pdContactLabel.TextColor = Theme.GetColour("text");
            apdContactLabel.TextColor = Theme.GetColour("text");
            phoneNumContactLabel.TextColor = Theme.GetColour("text");
            pdEmailLabel.TextColor = Theme.GetColour("accent");
            apdEmailLabel.TextColor = Theme.GetColour("accent");
            phoneNumLinkLabel.TextColor = Theme.GetColour("accent");
            altPhoneNumLinkLabel.TextColor = Theme.GetColour("accent");
            nmtIntroLabel.TextColor = Theme.GetColour("text");
            nmtUrlLabel.TextColor = Theme.GetColour("accent");
            nmtEmailLabel.TextColor = Theme.GetColour("accent");
            nmtPhoneLabel.TextColor = Theme.GetColour("accent");

            // Collapses the "email us" form if it is empty (and not focused), and shows the button instead
            if (SubjectEntry.Text == string.Empty && MessageEditor.Text == string.Empty 
                && !SubjectEntry.IsFocused && !MessageEditor.IsFocused)
            {
                ShowEmailFormButton.IsVisible = true;
                EmailFormStacklayout.IsVisible = false;
            }
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
            facebookButton.FontSize = 32;
            instagramButton.FontSize = 32;
            twitterButton.FontSize = 32;
        }

        /// <summary>
        /// Sets the view for vertical display
        /// </summary>
        public void SetVerticalDisplay()
        {
            this.Margin = new Thickness(10);
            facebookButton.FontSize = 25;
            instagramButton.FontSize = 25;
            twitterButton.FontSize = 25;
        }

        /// <summary>
        /// Handles taps on the program director email address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PdEmail_Tapped(object sender, EventArgs e)
        {
            await Email.ComposeAsync(string.Empty, string.Empty, "pd@boomradio.com.au");
        }

        /// <summary>
        /// Handles taps on the assistant program director email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ApdEmail_Tapped(object sender, EventArgs e)
        {
            await Email.ComposeAsync(string.Empty, string.Empty, "assistant.pd@boomradio.com.au");
        }

        /// <summary>
        /// Handles taps on the first phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PhoneNum_Tapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("tel:+61894432236");
        }

        /// <summary>
        /// Handles taps on the alternate phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AltPhoneNum_Tapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("tel:+61892024816");
        }

        /// <summary>
        /// Shows the "email us" form, and hides the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowEmailFormButton_Clicked(object sender, EventArgs e)
        {
            ShowEmailFormButton.IsVisible = false;
            EmailFormStacklayout.IsVisible = true;
        }

        /// <summary>
        /// Handles taps on North Metro Tafe's website url 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NmtUrl_Tapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("https://www.northmetrotafe.wa.edu.au/");
        }

        /// <summary>
        /// Handles taps on North Metro Tafe's email 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NmtEmail_Tapped(object sender, EventArgs e)
        {
            await Email.ComposeAsync(string.Empty, string.Empty, "enquiry@nmtafe.wa.edu.au");
        }

        /// <summary>
        /// Handles taps on North Metro Tafe's phone number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NmtPhone_Tapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("tel:+611300300822");
        }
    }
}