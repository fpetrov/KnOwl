using KnOwl.Models.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Services.Article
{
    public interface IArticleService
    {
        Task<List<Entities.Storage.Article>> GetAll();
        Task<Entities.Storage.Article> Get(int id);
        Task<Entities.Storage.ShortedArticle> Get(ArticleRequest request);
        List<Entities.Storage.ShortedArticle> GetBookmarks(Entities.Authentication.User user);
        List<Entities.Storage.ShortedArticle> Get(Entities.Authentication.User user);
        Task<List<Entities.Storage.ShortedArticle>> Take(int? count);
        Task<Entities.Storage.Article> Add(ArticleRequest request);
        Task<Entities.Storage.Article> Update(ArticleRequest request);
        Task<Entities.Storage.Article> Remove(int id);
    }
}