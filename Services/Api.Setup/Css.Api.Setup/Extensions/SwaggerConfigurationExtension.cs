using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace Css.Api.SetupMenu.Extensions
{
    /// <summary>
    /// Extension for adding swagger documentation
    /// </summary>
    public static class SwaggerConfigurationExtension
    {
        /// <summary>
        /// Creates the response compression.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(configuration["Version"],  new OpenApiInfo { Title = configuration["Title"], Version = configuration["Version"] });
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", GetSwaggerSecurityScheme());
                c.OperationFilter<SecurityRequirementsOperationFilter>("Bearer");
            });

            return services;
        }

        /// <summary>
        /// Gets the swagger security scheme.
        /// </summary>
        /// <returns></returns>
        private static OpenApiSecurityScheme GetSwaggerSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header. Example: " + "{token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT"
            };
        }
    }
}
