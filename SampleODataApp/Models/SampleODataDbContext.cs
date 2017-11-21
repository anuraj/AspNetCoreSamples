using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace SampleODataApp.Models
{
    public class SampleODataDbContext : DbContext
    {
        public SampleODataDbContext(DbContextOptions options) : base(options)
        {
        }

        protected SampleODataDbContext()
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}