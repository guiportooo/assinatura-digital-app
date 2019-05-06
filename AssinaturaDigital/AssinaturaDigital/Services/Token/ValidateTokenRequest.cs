namespace AssinaturaDigital.Services.Token
{
    public class ValidateTokenRequest
    {
        public int IdUser { get; }
        public string Token { get; }

        public ValidateTokenRequest(int idUser, string token)
        {
            IdUser = idUser;
            Token = token;
        }
    }
}
