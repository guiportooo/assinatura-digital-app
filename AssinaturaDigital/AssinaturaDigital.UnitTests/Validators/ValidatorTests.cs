using AssinaturaDigital.Validations;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.Validators
{
    public class ValidatorTests
    {
        private CPFValidator _cpfValidator;
        private EmailValidator _emailValidator;
        private PhoneNumberValidator _phoneNumberValidator;
        private RequiredValidator _requiredValidator;
        private TextValidator _textValidator;

        [SetUp]
        public void Setup()
        {
            _cpfValidator = new CPFValidator();
            _emailValidator = new EmailValidator();
            _phoneNumberValidator = new PhoneNumberValidator();
            _requiredValidator = new RequiredValidator();
            _textValidator = new TextValidator();
        }

        [Test]
        public void ShouldValidateCPF()
        {
            var invalidCPF = "1111111111";
            var validCPF = "308.600.080-53";

            _cpfValidator.Check(invalidCPF).Should().BeFalse();
            _cpfValidator.Check(validCPF).Should().BeTrue();
            _cpfValidator.Check(string.Empty).Should().BeFalse();
        }

        [Test]
        public void ShouldValidateEmail()
        {
            var invalidEmail = "1@1";
            var validEmail = "usuarioteste@tecnobank.com";

            _emailValidator.Check(invalidEmail).Should().BeFalse();
            _emailValidator.Check(validEmail).Should().BeTrue();
            _emailValidator.Check(string.Empty).Should().BeFalse();
        }

        [Test]
        public void ShouldValidatePhoneNumber()
        {
            var invalidPhone = "654654";
            var validPhone = "(81) 96586-3215";

            _phoneNumberValidator.Check(invalidPhone).Should().BeFalse();
            _phoneNumberValidator.Check(validPhone).Should().BeTrue();
            _phoneNumberValidator.Check(string.Empty).Should().BeFalse();
        }

        [Test]
        public void ShouldValidateRequiredFields()
        {
            _requiredValidator.Check(string.Empty).Should().BeFalse();
            _requiredValidator.Check("test").Should().BeTrue();
        }

        [Test]
        public void ShouldValidateText()
        {
            var invalidName = "1nv4l1d N4m3";
            var validName = "Test Name";

            _textValidator.Check(invalidName).Should().BeFalse();
            _textValidator.Check(validName).Should().BeTrue();
            _textValidator.Check(string.Empty).Should().BeFalse();
        }
    }
}
