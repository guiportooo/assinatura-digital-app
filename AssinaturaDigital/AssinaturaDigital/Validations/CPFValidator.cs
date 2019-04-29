using System.Text.RegularExpressions;

namespace AssinaturaDigital.Validations
{
    public class CPFValidator : IValidator
    {
        public string Message { get; set; } = "CPF Inválido!";

        public bool Check(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var cpfRule = @"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})";
                return new Regex(cpfRule).IsMatch(value);
            }
            return false;
        }
    }
}
