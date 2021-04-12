using KnOwl.Models.Storage;
using KnOwl.Repositories.Article;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Services.Article
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Entities.Storage.Article> Add(ArticleRequest request)
        {
            return await _articleRepository.Add(request);
        }

        public async Task<Entities.Storage.Article> Get(int id)
        {
            return await _articleRepository.Get(id);
        }

        public async Task<List<Entities.Storage.Article>> GetAll()
        {
            return await _articleRepository.GetAll();
        }

        public async Task<Entities.Storage.Article> Remove(int id)
        {
            return await _articleRepository.Remove(id);
        }

        public async Task<Entities.Storage.Article> Update(ArticleRequest request)
        {
            return await _articleRepository.Update(request);
        }
    }
}
