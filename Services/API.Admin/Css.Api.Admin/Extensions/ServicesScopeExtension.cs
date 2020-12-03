using AutoMapper;
using Css.Api.Admin.Business;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Repository;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace Css.Api.Admin.Extensions
{
    /// <summary>
    /// Extension for adding service lide scopes
    /// </summary>
    public static class ServicesScopeExtension
    {
        /// <summary>
        /// Adds the services scope.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="HostingEnvironment">The hosting environment.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddServicesScope(this IServiceCollection services,
                                                               IWebHostEnvironment HostingEnvironment,
                                                               IConfiguration configuration)
        {
            //Configure logger
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSingleton(Log.Logger);
            services.AddSingleton(HostingEnvironment);
            services.AddSingleton(configuration);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();

            services.AddTransient<ISchedulingCodeService, SchedulingCodeService>();
            services.AddTransient<ISchedulingCodeIconService, SchedulingCodeIconService>();
            services.AddTransient<ISchedulingCodeTypeService, SchedulingCodeTypeService>();
            services.AddTransient<IAgentCategoryService, AgentCategoryService>();

            services.AddTransient<ICssLanguageService, CssLanguageService>();
            services.AddTransient<ICssMenuService, CssMenuService>();
            services.AddTransient<ICssVariableService, CssVariableService>();
            services.AddTransient<ILanguageTranslationService, LanguageTranslationService>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            return services;

        }
    }
}
