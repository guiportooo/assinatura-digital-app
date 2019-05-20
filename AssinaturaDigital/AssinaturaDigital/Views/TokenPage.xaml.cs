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
            FocusNextEmptyTokenDigit();
        }

        void FocusNextEmptyTokenDigit()
        {
            var nextEmptyTokenDigit = tokenDigits
                .Children
                .ToList()
                .FirstOrDefault(x => string.IsNullOrEmpty(((Entry)x).Text));

            if (nextEmptyTokenDigit == null)
                return;

            nextEmptyTokenDigit.Focus();
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
