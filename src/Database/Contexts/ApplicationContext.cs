using KnOwl.Entities.Authentication;
using KnOwl.Entities.Storage;
using Microsoft.EntityFrameworkCore;

namespace KnOwl.Database.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}