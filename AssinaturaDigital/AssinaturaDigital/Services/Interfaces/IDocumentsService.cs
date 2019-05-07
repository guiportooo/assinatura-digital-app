using AssinaturaDigital.Models;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Interfaces
{
    public interface IDocumentsService
    {
        Task SaveRG(MediaFile photo, PhotoTypes type);
        Task SaveCNH(MediaFile photo, PhotoTypes type);
    }
}
