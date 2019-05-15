using Plugin.Media.Abstractions;

namespace AssinaturaDigital.Models
{
    public class Document
    {
        public int IdUser { get; private set; }
        public DocumentType Type { get; private set; }
        public DocumentOrientation Orientation { get; private set; }
        public MediaFile Photo { get; private set; }
        public string Name => $"{IdUser}_{Type}_{Orientation}";

        public Document(int idUser, DocumentType type, DocumentOrientation orientation, MediaFile photo)
        {
            IdUser = idUser;
            Type = type;
            Orientation = orientation;
            Photo = photo;
        }
    }
}
