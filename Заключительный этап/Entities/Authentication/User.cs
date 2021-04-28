using KnOwl.Entities.Storage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KnOwl.Entities.Authentication
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
        public List<int> FavoriteArticles { get; set; }

        [JsonIgnore]
        public List<Article> Articles { get; set; } = new List<Article>();

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }

    public enum Role
    {
        Default,
        Moderator,
        Admin
    }
}