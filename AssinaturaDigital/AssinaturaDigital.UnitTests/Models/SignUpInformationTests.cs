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
            const string cpf = "222.222.222-22";
            const string cellPhoneNumber = "987654321";
            const string email = "test.user@appsign.com";

            var signUpInformation = new SignUpInformation(fullName,
                cpf,
                cellPhoneNumber,
                email);

            signUpInformation.FullName.Should().Be(fullName);
            signUpInformation.CPF.Should().Be(cpf);
            signUpInformation.CellPhoneNumber.Should().Be(cellPhoneNumber);
            signUpInformation.Email.Should().Be(email);
        }
    }
}
