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
                request.Article.Type = ArticleType.Moderating;

            return await _articleRepository.Add(request);
        }

        public async Task<Entities.Storage.Article> Get(int id)
        {
            return await _articleRepository.Get(id);
        }

        public async Task<ShortedArticle> Get(ArticleRequest request)
        {
            var article = await _articleRepository.Get(request);

            if (article == null)
                return null;

            if (article.Type == ArticleType.Private && article.Author != request.Article.Author)
                return null;

            return new ShortedArticle(article);
        }

        public List<ShortedArticle> GetBookmarks(Entities.Authentication.User user)
        {
            var shortedArticles = _articleRepository
                .GetBookmarks(user)
                .Select(article => new ShortedArticle(article))
                .ToList();

            return shortedArticles;
        }

        public List<ShortedArticle> Get(Entities.Authentication.User user)
        {
            var shortedArticles = user.Articles
                .Select(article => new ShortedArticle(article))
                .ToList();

            return shortedArticles;
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
