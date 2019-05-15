using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Fakes
{
    public class DocumentsServiceFake : IDocumentsService
    {
        public IList<Document> Documents { get; private set; }

        public DocumentsServiceFake() => Documents = new List<Document>();

        public Task SaveDocument(Document document)
        {
            Documents.Add(document);
            return Task.CompletedTask;
        }
    }
}
