using AssinaturaDigital.Services.Validations;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class ValidationsServiceFake : IValidationsService
    {
        private bool _shouldBeValid;
        public int IdUser { get; private set; }
        public MediaFile Video { get; private set; }

        public ValidationsServiceFake() => _shouldBeValid = true;

        public Task<bool> ValidateUser(int idUser, MediaFile video)
        {
            IdUser = idUser;
            Video = video;
            return Task.FromResult(_shouldBeValid);
        }

        public void ShouldNotBeValid() => _shouldBeValid = false;
    }
}
