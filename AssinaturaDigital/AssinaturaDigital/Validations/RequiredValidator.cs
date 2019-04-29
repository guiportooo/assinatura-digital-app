namespace AssinaturaDigital.Validations
{
    public class RequiredValidator : IValidator
    {
        public string Message { get; set; } = "Campo obrigatório";

        public bool Check(string value) =>
            !string.IsNullOrWhiteSpace(value);
    }
}
