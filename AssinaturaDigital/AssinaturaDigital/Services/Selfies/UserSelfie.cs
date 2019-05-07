namespace AssinaturaDigital.Services.Selfies
{
    public class UserSelfie
    {
        public int IdUser { get; }
        public string Base64Photo { get; }

        public UserSelfie(int idUser, string base64Photo)
        {
            IdUser = idUser;
            Base64Photo = base64Photo;
        }
    }
}
