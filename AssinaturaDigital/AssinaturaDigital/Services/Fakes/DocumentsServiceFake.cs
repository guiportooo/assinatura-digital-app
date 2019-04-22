using AssinaturaDigital.Models;
using AssinaturaDigital.Services.Interfaces;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services
{
    public class DocumentsServiceFake : IDocumentsService
    {
        public MediaFile Photo { get; private set; }
        public Dictionary<PhotoTypes, MediaFile> Photos { get; private set; }

        public DocumentsServiceFake() => Photos = new Dictionary<PhotoTypes, MediaFile>();

        public Task SaveRG(MediaFile photo, PhotoTypes type)
        {
            Photos.Add(type, photo);
            return Task.CompletedTask;
        }

        public Task SaveCNH(MediaFile photo, PhotoTypes type)
        {
            Photos.Add(type, photo);
            return Task.CompletedTask;
        }

        public Task<bool> SaveSelfie(MediaFile photo)
        {
            Photo = photo;
            return Task.FromResult(true);
        }
    }
}
