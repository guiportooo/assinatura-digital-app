namespace AssinaturaDigital.Models
{
    public class SignUpInformation
    {
        public string FullName { get; set; }
        public long CPF { get; set; }
        public long CellphoneNumber { get; set; }
        public string Email { get; set; }

        public SignUpInformation(string fullName, string cpf, string cellphoneNumber, string email)
        {
            long.TryParse(cpf, out var longCpf);
            long.TryParse(cellphoneNumber, out var longCellphoneNumber);

            FullName = fullName;
            CPF = longCpf;
            CellphoneNumber = longCellphoneNumber;
            Email = email;
        }
    }
}
