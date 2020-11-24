using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.Admin.Extensions
{
    /// <summary>
    /// Extension for adding the mvc configuration
    /// </summary>
    public static class CustomMVCConfigurationExtension
    {
        /// <summary>
        /// Adds the swagger configuration.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddMVCConfiguration(this IServiceCollection services)
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

            services.AddMvc().AddNewtonsoftJson();
            services.AddMvcCore().AddApiExplorer();

            return services;
        }
    }
}
