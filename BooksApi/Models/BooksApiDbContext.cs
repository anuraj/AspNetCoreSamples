using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Models
{
    public class BooksApiDbContext : DbContext
    {
        public BooksApiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected BooksApiDbContext()
        {
        }
        public DbSet<Book> Books { get; set; }
    }
}