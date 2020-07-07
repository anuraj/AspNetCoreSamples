using HealthDemo;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiHealthCheckExtensions
    {
        public static IHealthChecksBuilder AddApiHealth(this IHealthChecksBuilder healthChecksBuilder,
            string name = "ApiHealth")
        {
            return healthChecksBuilder.AddCheck<ApiHealthCheck>(name);
        }
    }
}
