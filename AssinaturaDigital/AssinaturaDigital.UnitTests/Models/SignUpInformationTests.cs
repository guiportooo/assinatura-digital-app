using AssinaturaDigital.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.Models
{
    public class SignUpInformationTests
    {
        [Test]
        public void ShouldCreateValidSignUpInformation()
        {
            const string fullName = "Test User";
            const string cpf = "22222222222";
            const string cellphoneNumber = "987654321";
            const string email = "test.user@appsign.com";

            var signUpInformation = new SignUpInformation(fullName,
                cpf,
                cellphoneNumber,
                email);

            signUpInformation.FullName.Should().Be(fullName);
            signUpInformation.CPF.Should().Be(long.Parse(cpf));
            signUpInformation.CellphoneNumber.Should().Be(long.Parse(cellphoneNumber));
            signUpInformation.Email.Should().Be(email);
        }

        [Test]
        public void WhenPassingInvalidCPFShouldCreateSignUpInformationWithNoCPF()
        {
            const string invalidCpf = "CPF with letters";
            const string fullName = "Test User";
            const string cellphoneNumber = "987654321";
            const string email = "test.user@appsign.com";

            var signUpInformation = new SignUpInformation(fullName,
                invalidCpf,
                cellphoneNumber,
                email);

            signUpInformation.CPF.Should().Be(0);
        }

        [Test]
        public void WhenPassingInvalidCellphoneNumberShouldCreateSignUpInformationWithNoCellphoneNumber()
        {
            const string invalidCellphoneNumber = "CellphoneNumber with letters";
            const string fullName = "Test User";
            const string cpf = "22222222222";
            const string email = "test.user@appsign.com";

            var signUpInformation = new SignUpInformation(fullName,
                cpf,
                invalidCellphoneNumber,
                email);

            signUpInformation.CellphoneNumber.Should().Be(0);
        }
    }
}
