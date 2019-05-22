using System.Linq;
using Xamarin.Forms;

namespace AssinaturaDigital.Views
{
    public partial class TokenPage : ContentPage
    {
        public TokenPage()
        {
            InitializeComponent();
        }

        void TokenDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                FocusPreviousNotEmptyTokenDigit();
                return;
            }
            FocusNextEmptyTokenDigit((Entry)sender);
        }

        void FocusNextEmptyTokenDigit(Entry actualTokenDigit)
        {
            var nextEmptyTokenDigit = (Entry)tokenDigits
                .Children
                .ToList()
                .FirstOrDefault(x => string.IsNullOrEmpty(((Entry)x).Text));

            if (nextEmptyTokenDigit == null)
                return;

            if(actualTokenDigit.Text.Length > 1)
            {
                actualTokenDigit.Text = actualTokenDigit.Text.Substring(0, 1);
                nextEmptyTokenDigit.Text = actualTokenDigit.Text.Substring(1, 1);
                nextEmptyTokenDigit.Focus();
            }
        }

        void FocusPreviousNotEmptyTokenDigit()
        {
            var previousNotEmptyTokenDigit = tokenDigits
                .Children
                .ToList()
                .LastOrDefault(x => !string.IsNullOrEmpty(((Entry)x).Text));

            if (previousNotEmptyTokenDigit == null)
                return;

            previousNotEmptyTokenDigit.Focus();
        }
    }
}
