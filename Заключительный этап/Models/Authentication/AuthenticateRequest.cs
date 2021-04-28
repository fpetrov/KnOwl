using System.ComponentModel.DataAnnotations;

namespace KnOwl.Models.Authentication
{
    public class AuthenticateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Fingerprint { get; set; }
    }
}
