using AssinaturaDigital.Models;
using AssinaturaDigital.Services.SignUp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class SignUpServiceFake : ISignUpService
    {
        const string existingCpf = "111.111.111-11";

        private bool _shouldDelay;
        private bool _shouldValidateExistingCpf;

        public SignUpInformation SignUpInformation { get; private set; }

        public void ShouldDelay(bool shouldDelay) => _shouldDelay = shouldDelay;

        public void ShouldValidateExistingCpf(bool shouldValidateExistingCpf)
            => _shouldValidateExistingCpf = shouldValidateExistingCpf;

        public async Task<SignUpResponse> SignUp(SignUpInformation signUpInformation)
        {
            if (_shouldDelay)
                await Task.Delay(1000);

            SignUpInformation = signUpInformation;

            if (_shouldValidateExistingCpf || signUpInformation.CPF == existingCpf)
                return new SignUpResponse()
                {
                    Errors = new List<ResponseError>
                    {
                        new ResponseError { Field = "CPF", Message = "CPF já cadastrado." }
                    }
                };

            return new SignUpResponse(new User(1,
                signUpInformation.FullName,
                signUpInformation.CPF,
                signUpInformation.CellPhoneNumber,
                signUpInformation.Email));
        }
    }
}
