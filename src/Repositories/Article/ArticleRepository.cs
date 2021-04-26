using KnOwl.Database.Contexts;
using KnOwl.Models.Storage;
using KnOwl.Services.User;
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
            return await _context.Set<Entities.Storage.Article>().FindAsync(id);
        }

        public async Task<List<Entities.Storage.Article>> Take(int? count)
        {
            if (!count.HasValue)
                return null;

            return (await _context.Set<Entities.Storage.Article>().Where(a => a.Type == Entities.Storage.ArticleType.Shared).ToListAsync()).TakeLast(count.Value).ToList();
        }

        public async Task<List<Entities.Storage.Article>> GetAll()
        {
            return await _context.Set<Entities.Storage.Article>().ToListAsync();
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
