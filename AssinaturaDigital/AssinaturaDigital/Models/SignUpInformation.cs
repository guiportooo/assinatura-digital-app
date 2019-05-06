namespace AssinaturaDigital.Models
{
    public class SignUpInformation
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CPF { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }

        public SignUpInformation(string fullName, string cpf, string cellPhoneNumber, string email)
        {
            FullName = fullName;
            CPF = cpf;
            CellPhoneNumber = cellPhoneNumber;
            Email = email;
        }

        public SignUpInformation(int id,
            string fullName,
            string cpf,
            string cellPhoneNumber,
            string email)
        {
            Id = id;
            FullName = fullName;
            CPF = cpf;
            CellPhoneNumber = cellPhoneNumber;
            Email = email;
        }
    }
}
