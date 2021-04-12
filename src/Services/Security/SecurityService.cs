namespace KnOwl.Services
{
    public class SecurityService : ISecurityService
    {
        public bool Verify(string content, string hash) => BCrypt.Net.BCrypt.Verify(content, hash);

        public string Hash(string content) => BCrypt.Net.BCrypt.HashPassword(content);
    }
}