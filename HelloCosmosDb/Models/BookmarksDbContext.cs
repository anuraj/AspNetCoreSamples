using Microsoft.EntityFrameworkCore;

namespace HelloCosmosDb.Models
{
    public class BookmarksDbContext : DbContext
    {
        public BookmarksDbContext(DbContextOptions options) : base(options)
        {
        }

        protected BookmarksDbContext()
        {
        }
        public DbSet<Bookmark> Bookmarks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bookmark>();
            var bookmarks = modelBuilder.Entity<Bookmark>().Metadata;
            bookmarks.CosmosSql().CollectionName = nameof(Bookmarks);
        }
    }
}