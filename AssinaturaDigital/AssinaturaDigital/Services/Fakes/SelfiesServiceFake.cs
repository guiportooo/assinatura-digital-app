using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Selfies;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class SelfiesServiceFake : ISelfiesService
    {
        public int IdUser { get; private set; }
        public MediaFile Photo { get; private set; }
        public Dictionary<PhotoTypes, MediaFile> Photos { get; private set; }

        public SelfiesServiceFake() => Photos = new Dictionary<PhotoTypes, MediaFile>();

        public Task<bool> SaveSelfie(MediaFile photo)
        {
            Photo = photo;
            return Task.FromResult(true);
        }

        public Task<bool> SaveSelfie(int idUser, MediaFile photo)
        {
            IdUser = idUser;
            Photo = photo;
            return Task.FromResult(true);
        }
    }
}
