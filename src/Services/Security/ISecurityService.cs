namespace KnOwl.Services
{
    public interface ISecurityService
    {
        bool Verify(string content, string hash);
        string Hash(string content);
    }
}