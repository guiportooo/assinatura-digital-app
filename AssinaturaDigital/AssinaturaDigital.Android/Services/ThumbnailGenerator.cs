using Android.App;
using Android.Graphics;
using Android.Media;
using AssinaturaDigital.Services.Interfaces;
using System;
using System.IO;

namespace AssinaturaDigital.Droid.Services
{
    public class ThumbnailGenerator : IThumbnailGenerator
    {
        public System.IO.Stream GenerateThumbImage(string url, long usecond)
        {
            try
            {
                var context = Application.Context;
                var uri = Android.Net.Uri.Parse(url);

                var retriever = new MediaMetadataRetriever();
                retriever.SetDataSource(context, uri);

                var bitmap = retriever.GetFrameAtTime(usecond);

                if (bitmap == null)
                    return null;

                var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                var bitmapData = stream.ToArray();
                return new MemoryStream(bitmapData);
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return null;
            }
        }
    }
}
