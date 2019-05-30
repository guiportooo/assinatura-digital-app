using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class AuthenticationServiceFake : IAuthenticationService
    {
        const string existingCpf = "111.111.111-11";

        private bool _shouldDelay;
        private bool _shouldFail;
        private bool _shouldValidateExistingCpf;
        private bool _shouldNotReturnUserWithCpf;
        private bool _shouldInformInterruptedSignUp;

        public SignUpInformation SignUpInformation { get; private set; }
        public User ReturningUser { get; private set; }

        public void ShouldDelay(bool shouldDelay) => _shouldDelay = shouldDelay;

        public void ShouldFail(bool shouldFail) => _shouldFail = shouldFail;

        public void ShouldValidateExistingCpf(bool shouldValidateExistingCpf)
            => _shouldValidateExistingCpf = shouldValidateExistingCpf;

        public void ShouldNotReturnUserWithCpf() => _shouldNotReturnUserWithCpf = true;

        public void ShouldInformInterruptedSignUp() => _shouldInformInterruptedSignUp = true;

        public async Task<AuthenticationResponse> SignUp(SignUpInformation signUpInformation)
        {
            if (_shouldFail)
                throw new Exception("Failed to SignUp.");

            if (_shouldDelay)
                await Task.Delay(1000);

            SignUpInformation = signUpInformation;

            if (_shouldValidateExistingCpf || signUpInformation.CPF == existingCpf)
                return new AuthenticationResponse()
                {
                    Errors = new List<ResponseError>
                    {
                        new ResponseError { Field = "CPF", Message = "CPF já cadastrado." }
                    }
                };

            ReturningUser = new User(1,
                signUpInformation.FullName,
                signUpInformation.CPF,
                signUpInformation.CellPhoneNumber,
                signUpInformation.Email);

            return new AuthenticationResponse(ReturningUser, (int)HttpStatusCode.OK);
        }

        public Task<AuthenticationResponse> SignIn(string cpf)
        {
            var status = 0;

            if (_shouldFail)
                throw new Exception("Failed to sign in.");

            if (!_shouldNotReturnUserWithCpf)
            {
                ReturningUser = ReturningUser = new User(1,
                    "FullName",
                    cpf,
                    "(11) 11111-111",
                    "email@email.com");

                status = (int)HttpStatusCode.OK;
            }

            if (_shouldInformInterruptedSignUp)
                status = (int)HttpStatusCode.Forbidden;

            return Task.FromResult(new AuthenticationResponse(ReturningUser, status));
        }
    }
}
