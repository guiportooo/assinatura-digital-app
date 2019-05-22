using System;

namespace AssinaturaDigital.Models
{
    public class ManifestInfos
    {
        public string GeoLocation { get; set; }
        public DateTime SignatureDate { get; set; }

        public ManifestInfos(string geolocation, DateTime signaturaDate)
        {
            GeoLocation = geolocation;
            SignatureDate = signaturaDate;
        }
    }
}
