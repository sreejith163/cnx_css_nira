using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.SetupMenu.Extensions
{
    /// <summary>
    /// Extension for adding health checks
    /// </summary>
    public static class HealthCheckExtension
    {
        /// <summary>
        /// Adds the health checks.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks();
            return services;
        }
    }
}
