using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.Core.Utilities.Extensions
{
    /// <summary>
    ///  Extension for adding CORS to the API projects
    /// </summary>
    public static class CORSExtension
    {
        /// <summary>
        ///  The methods adds CORS Policy 'CorsPolicy' to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }
    }
}
