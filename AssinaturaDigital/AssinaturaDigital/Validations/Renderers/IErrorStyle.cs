using Xamarin.Forms;

namespace AssinaturaDigital.Validations.Renderers
{
    public interface IErrorStyle
    {
        void ShowError(View view, string message);
        void RemoveError(View view);
    }
}
