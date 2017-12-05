using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace webmarks.xyz.Models
{
    public class WebMarksDbContext : DbContext
    {
        public WebMarksDbContext(DbContextOptions options) : base(options)
        {
        }

        protected WebMarksDbContext()
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
    }
}