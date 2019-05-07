using System;
using System.IO;
using System.Threading.Tasks;

namespace AssinaturaDigital.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<string> ToBase64(this Stream stream)
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            return Convert.ToBase64String(bytes);
        }
    }
}
