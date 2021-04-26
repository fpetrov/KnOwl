using System.Linq;

namespace KnOwl.Entities.Storage
{
    public class ShortedArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public ShortedArticle(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            Content = string.Join(" ", article.Content.Split(' ').Take(30)) + "...";
        }
    }
}