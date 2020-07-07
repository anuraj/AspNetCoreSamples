using Microsoft.EntityFrameworkCore;

namespace HealthDemo
{
    public class WeatherForecastDbContext : DbContext
    {
        public WeatherForecastDbContext(DbContextOptions options) : base(options)
        {
        }

        protected WeatherForecastDbContext()
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
