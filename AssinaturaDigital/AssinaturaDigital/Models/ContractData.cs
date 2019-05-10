using System;
namespace AssinaturaDigital.Models
{
    public class ContractData
    {
        public string Identification { get; private set; }
        public string CPF { get; private set; }
        public bool IsSigned { get; private set; }

        public ContractData(string identification, string cpf, bool isSigned)
        {
            Identification = identification;
            CPF = cpf;
            IsSigned = isSigned;
        }

        public void SignContract()
        {
            IsSigned = true;
        }
    }
}
