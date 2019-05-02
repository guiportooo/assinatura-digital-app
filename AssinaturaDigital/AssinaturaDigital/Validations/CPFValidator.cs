using AssinaturaDigital.Extensions;

namespace AssinaturaDigital.Validations
{
    public class CPFValidator : IValidator
    {
        public string Message { get; set; } = "CPF Inválido!";

        public bool Check(string value) => value.IsValidCPF();
    }
}
