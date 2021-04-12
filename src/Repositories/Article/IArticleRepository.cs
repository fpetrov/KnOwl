using KnOwl.Models.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Repositories.Article
{
    public interface IArticleRepository
    {
        Task<List<Entities.Storage.Article>> GetAll();
        Task<Entities.Storage.Article> Get(int id);
        Task<Entities.Storage.Article> Add(ArticleRequest request);
        Task<Entities.Storage.Article> Update(ArticleRequest request);
        Task<Entities.Storage.Article> Remove(int id);
    }
}
