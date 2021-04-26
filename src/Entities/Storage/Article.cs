using KnOwl.Entities.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KnOwl.Entities.Storage
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public ArticleType Type { get; set; }
        public string Created { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Image> Images { get; set; }
    }

    public enum ArticleType
    {
        Private = 1,
        Shared = 2,
        Moderating = 3
    }
}