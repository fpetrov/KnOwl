namespace KnOwl.Models.Authentication
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = "Some Issuer";
        public string Audience { get; set; } = "Some Audience";
        public string ServerSecret { get; set; } = "Some Secret";
        public int LifeTime { get; set; } = 10;
        public int RefreshTokenLifeTime { get; set; } = 1;
    }
}