using Css.Api.AdminOps.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.AdminOps.Extensions
{
    /// <summary>
    /// Extension for adding the fluent validation assemblies
    /// </summary>
    public static class FluentValidationExtension
    {
        /// <summary>
        /// Adds the fluent abstract validators.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddFluentAbstractValidators(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            })
           .AddFluentValidation(options =>
           {
               options.RegisterValidatorsFromAssemblyContaining<Startup>();
           });

            return services;
        }
    }
}
