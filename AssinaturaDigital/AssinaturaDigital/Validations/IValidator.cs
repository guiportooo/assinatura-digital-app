namespace AssinaturaDigital.Validations
{
    public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
}
