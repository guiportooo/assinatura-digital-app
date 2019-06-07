using AssinaturaDigital.Configuration;
using AssinaturaDigital.Services.Interfaces;
using AssinaturaDigital.Utilities;
using Flurl;
using Flurl.Http;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace AssinaturaDigital.Services.Validations
{
    public class ValidationsService : IValidationsService
    {
        private readonly string _urlApi;
        private readonly IErrorHandler _errorHandler;
        private readonly IThumbnailGenerator _thumbnailGenerator;

        public ValidationsService(IConfigurationManager configurationManager,
            IErrorHandler errorHandler,
            IThumbnailGenerator thumbnailGenerator)
        {
            _errorHandler = errorHandler;
            _thumbnailGenerator = thumbnailGenerator;

            var config = configurationManager.Get();
            _urlApi = config.UrlApi;
        }

        public async Task<bool> ValidateUser(int idUser, MediaFile video)
        {
            try
            {
                using (var videoStream = video.GetStream())
                {
                    var selfieStream = _thumbnailGenerator.GenerateThumbImage(video.AlbumPath, 1);

                    var response = await _urlApi
                    .AppendPathSegment("users")
                    .AppendPathSegment(idUser)
                    .AppendPathSegment("validations")
                    .PostMultipartAsync(x => x
                        .AddFile("video", videoStream, $"{idUser}_Video.mp4")
                        .AddFile("selfie", selfieStream, $"{idUser}_Selfie.png"));
                }

                return true;
            }
            catch (FlurlHttpException ex)
            {
                _errorHandler.HandleError(ex);
                return false;
            }
        }
    }
}
