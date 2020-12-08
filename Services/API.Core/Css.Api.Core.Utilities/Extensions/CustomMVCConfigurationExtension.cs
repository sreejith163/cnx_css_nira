using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.Core.Utilities.Extensions
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
            services.AddMvc().AddNewtonsoftJson();
            services.AddMvcCore().AddApiExplorer();

            return services;
        }
    }
}
