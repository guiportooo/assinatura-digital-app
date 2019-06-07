using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Validations
{
    public interface IValidationsService
    {
        Task<bool> ValidateUser(int idUser, MediaFile video);
    }
}
