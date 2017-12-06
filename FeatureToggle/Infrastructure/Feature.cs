using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureToggle.Infrastructure
{
    public interface IFeature
    {
        bool IsFeatureEnabled(string feature);
    }
    public class Feature : IFeature
    {
        private readonly IConfiguration _configuration;
        public Feature(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsFeatureEnabled(string feature)
        {
            bool.TryParse(_configuration[$"Features:{feature}"], out bool result);
            return result;
        }
    }

    public static class FeatureExtensions
    {
        public static void AddFeatureToggle(this IServiceCollection services)
        {
            services.AddTransient(typeof(IFeature), typeof(Feature));
        }
    }
}