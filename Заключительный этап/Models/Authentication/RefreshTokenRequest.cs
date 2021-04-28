using System.ComponentModel.DataAnnotations;

namespace KnOwl.Models.Authentication
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }

        [Required]
        public string Fingerprint { get; set; }
    }
}