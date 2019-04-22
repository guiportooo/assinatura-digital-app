using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class MediaMock : IMedia
    {
        private readonly bool _isCameraAvailable;
        private readonly bool _isTakePhotoSupported;
        public StoreCameraMediaOptions Options { get; private set; }

        public MediaMock(bool isCameraAvailable, bool isTakePhotoSupported)
        {
            _isCameraAvailable = isCameraAvailable;
            _isTakePhotoSupported = isTakePhotoSupported;
        }

        public bool IsCameraAvailable => _isCameraAvailable;

        public bool IsTakePhotoSupported => _isTakePhotoSupported; 

        public bool IsPickPhotoSupported => throw new NotImplementedException();

        public bool IsTakeVideoSupported => throw new NotImplementedException();

        public bool IsPickVideoSupported => throw new NotImplementedException();

        public Task<bool> Initialize() => Task.FromResult(true);

        public Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<MediaFile> PickVideoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
        {
            Options = options;
            return Task.FromResult(new MediaFile(options.Directory, null));
        }

        public Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
