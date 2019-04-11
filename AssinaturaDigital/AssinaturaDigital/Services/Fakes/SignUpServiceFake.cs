using System;
using System.Threading.Tasks;
using AssinaturaDigital.Models;

namespace AssinaturaDigital.Services.Fakes
{
    public class SignUpServiceFake : ISignUpService
    {
        const long existingCpf = 11111111111;

        private bool _shouldDelay;
        private bool _shouldValidateExistingCpf;

        public SignUpInformation SignUpInformation { get; private set; }

        public SignUpServiceFake() => _shouldDelay = true;

        public void ShouldDelay(bool shouldDelay) => _shouldDelay = shouldDelay;

        public void ShouldValidateExistingCpf(bool shouldValidateExistingCpf) 
            => _shouldValidateExistingCpf = shouldValidateExistingCpf;

        public async Task SignUp(SignUpInformation signUpInformation)
        {
            if(_shouldDelay)
                await Task.Delay(1000);

            SignUpInformation = signUpInformation;

            if (_shouldValidateExistingCpf || signUpInformation.CPF == existingCpf)
                throw new InvalidOperationException("CPF já cadastrado.");
        }
    }
}
