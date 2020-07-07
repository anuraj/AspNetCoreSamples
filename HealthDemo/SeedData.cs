using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HealthDemo
{
    public static class SeedData
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<WeatherForecastDbContext>();
            context.Database.EnsureCreated();
            if (!context.WeatherForecasts.Any())
            {
                var rng = new Random();
                var weatherForecasts = Enumerable.Range(1, 15).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
                context.WeatherForecasts.AddRange(weatherForecasts);
                context.SaveChanges();
            }
        }
    }
}