namespace AssinaturaDigital.Configuration
{
    public class Configuration
    {
        public string IOSAppCenterSecret { get; set; }
        public string AndroidAppCenterSecret { get; set; }
        public int SecondsToGenerateToken { get; set; }
        public bool UseFakes { get; set; }
        public string UrlApi { get; set; }
    }
}
