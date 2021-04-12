using KnOwl.Entities.Authentication;

namespace KnOwl.Models.Authentication
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwt, string refreshToken)
        {
            Id = user.Id;
            Jwt = jwt;
            RefreshToken = refreshToken;
        }
    }
}