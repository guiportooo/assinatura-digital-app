using AssinaturaDigital.Extensions;

namespace AssinaturaDigital.Validations
{
    public class PhoneNumberValidator : IValidator
    {
        public string Message { get; set; } = "Telefone inválido!";

        public bool Check(string value) => value.IsValidCellPhoneNumber();
    }
}
