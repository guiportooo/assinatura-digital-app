using AssinaturaDigital.Extensions;

namespace AssinaturaDigital.Validations
{
    public class TextValidator : IValidator
    {
        public string Message { get; set; } = "Apenas letras são permitidas";

        public bool Check(string value) => value.IsValidName();
    }
}
