using KnOwl.Database.Contexts;
using KnOwl.Entities.Storage;
using KnOwl.Models.Storage;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnOwl.Repositories.Article
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationContext _context;

        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Entities.Storage.Article> Add(ArticleRequest request)
        {
            var author = request.Article.Author;

            if (await _context.Set<Entities.Storage.Article>().AnyAsync(a => a.Title == request.Article.Title))
                return null;

            _context.Set<Entities.Storage.Article>().Add(request.Article);
            await _context.SaveChangesAsync();

            return request.Article;
        }

        public async Task<Entities.Storage.Article> Get(int id)
        {
            return await _context.Set<Entities.Storage.Article>().Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
        }

        public List<Entities.Storage.Article> GetBookmarks(Entities.Authentication.User user)
        {
            var articles = new List<Entities.Storage.Article>();

            user.FavoriteArticles
                .ForEach(async a => articles.Add(await Get(a)));

            return articles;
        }

        public async Task<List<Entities.Storage.Article>> GetAll(ArticleType type)
        {
            return await _context.Set<Entities.Storage.Article>().Include(a => a.Author).Where(a => a.Type == type).ToListAsync();
        }

        public async Task<Entities.Storage.Article> Get(ArticleRequest request)
        {
            return await _context.Set<Entities.Storage.Article>().FirstOrDefaultAsync(a => a.Title.ToLower().Contains(request.Article.Title.ToLower()));
        }

        public async Task<List<Entities.Storage.Article>> Take(int? count)
        {
            if (!count.HasValue)
                return null;

            return (await _context.Set<Entities.Storage.Article>().Where(a => a.Type == ArticleType.Shared).Include(a => a.Author).ToListAsync()).TakeLast(count.Value).ToList();
        }

        public async Task<List<Entities.Storage.Article>> GetAll()
        {
            return await _context.Set<Entities.Storage.Article>().Include(a => a.Author).ToListAsync();
        }

        public async Task<Entities.Storage.Article> Remove(int id)
        {
            var entity = await _context.Set<Entities.Storage.Article>().FindAsync(id);

            if (entity == null)
                return null;

            _context.Set<Entities.Storage.Article>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Entities.Storage.Article> Update(ArticleRequest request)
        {
            _context.Entry(request.Article).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return request.Article;
        }
    }
}
