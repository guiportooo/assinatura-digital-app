using AssinaturaDigital.Models;
using AssinaturaDigital.UnitTests.Mocks;
using FluentAssertions;
using NUnit.Framework;

namespace AssinaturaDigital.UnitTests.Models
{
    public class TokenDigitTests
    {
        [Test]
        public void ShouldCreateTokenDigit()
        {
            const string expectedDigit = "2";

            var tokenDigit = new TokenDigit(expectedDigit);

            tokenDigit.Digit.Should().Be(expectedDigit);
        }

        [Test]
        public void WhenSettingDigitShouldNotifyCommand()
        {
            var tokenDigit = new TokenDigit(string.Empty);
            var commandToNotify = new DelegateCommandMock();
            tokenDigit.SetCommandToNotify(commandToNotify);

            tokenDigit.Digit = "1";

            commandToNotify.CanExecuteChangeRaised.Should().BeTrue();
        }
    }
}
