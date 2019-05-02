using AssinaturaDigital.Extensions;

namespace AssinaturaDigital.Validations
{
    public class EmailValidator : IValidator
    {
        public string Message { get; set; } = "Email inválido!";

        public bool Check(string value) => value.IsValidEmail();
    }
}
