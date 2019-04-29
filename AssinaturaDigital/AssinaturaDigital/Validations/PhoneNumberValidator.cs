using System.Text.RegularExpressions;

namespace AssinaturaDigital.Validations
{
    public class PhoneNumberValidator : IValidator
    {
        public string Message { get; set; } = "Telefone inválido!";

        public bool Check(string value)
        {
            var phoneRule = @"^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$";
            return !string.IsNullOrEmpty(value) && new Regex(phoneRule).IsMatch(value);
        }
    }
}
