using Microsoft.Extensions.DependencyInjection;

namespace FeatureToggle.Infrastructure
{
    public static class FeatureExtensions
    {
        public static void AddFeatureToggle(this IServiceCollection services)
        {
            services.AddTransient(typeof(IFeature), typeof(Feature));
        }
    }
}