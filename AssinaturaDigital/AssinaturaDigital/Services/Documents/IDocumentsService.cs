using AssinaturaDigital.Models;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Documents
{
    public interface IDocumentsService
    {
        Task SaveDocument(Document document);
    }
}
