namespace AssinaturaDigital.Models
{
    public class ContractData
    {
        public int Id { get; set; }
        public string Identification { get; private set; }
        public string CPF { get; private set; }
        public bool IsSigned { get; private set; }
        public string Data { get; private set; }

        public ContractData(int id, string identification, string cpf, string data, bool isSigned)
        {
            Id = id;
            Identification = identification;
            CPF = cpf;
            IsSigned = isSigned;
            Data = data;
        }

        public void Sign() => IsSigned = true;
    }
}
