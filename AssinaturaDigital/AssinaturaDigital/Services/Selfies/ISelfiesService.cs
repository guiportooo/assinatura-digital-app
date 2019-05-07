using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Selfies
{
    public interface ISelfiesService
    {
        Task<bool> SaveSelfie(int idUser, MediaFile photo);
    }
}
