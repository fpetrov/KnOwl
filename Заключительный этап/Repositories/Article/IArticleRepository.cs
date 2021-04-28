using KnOwl.Models.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Repositories.Article
{
    public interface IArticleRepository
    {
        Task<List<Entities.Storage.Article>> GetAll();
        Task<Entities.Storage.Article> Get(int id);
        List<Entities.Storage.Article> GetBookmarks(Entities.Authentication.User user);
        Task<List<Entities.Storage.Article>> GetAll(Entities.Storage.ArticleType id);
        Task<Entities.Storage.Article> Get(ArticleRequest request);
        Task<List<Entities.Storage.Article>> Take(int? count);
        Task<Entities.Storage.Article> Add(ArticleRequest request);
        Task<Entities.Storage.Article> Update(ArticleRequest request);
        Task<Entities.Storage.Article> Remove(int id);
    }
}
