using System.Text.RegularExpressions;

namespace AssinaturaDigital.Validations
{
    public class EmailValidator : IValidator
    {
        public string Message { get; set; } = "Email inválido!";

        public bool Check(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return new Regex(@"^.+@[^\.].*\.[a-z]{2,}$").IsMatch(value);
            return false;
        }
    }
}
