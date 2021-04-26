using KnOwl.Models.Storage;
using KnOwl.Repositories.Article;
using System.Collections.Generic;
using System.Threading.Tasks;
using KnOwl.Entities.Authentication;
using KnOwl.Entities.Storage;
using System.Linq;

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
            if (request.Article.Author.Role == Role.Default && request.Article.Type == ArticleType.Shared)
                return null;

            return await _articleRepository.Add(request);
        }

        public async Task<Entities.Storage.Article> Get(int id)
        {
            return await _articleRepository.Get(id);
        }

        public async Task<List<ShortedArticle>> Take(int? count)
        {
            var articles = await _articleRepository.Take(count);

            var shortedArticles = articles
                .Select(article => new ShortedArticle(article))
                .ToList();

            return shortedArticles;
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
