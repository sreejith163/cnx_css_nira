using AutoMapper;
using Css.Api.Scheduling.Business;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace Css.Api.Scheduling.Extensions
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

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IClientLOBGroupService, ClientLOBGroupService>();
            services.AddTransient<ISkillTagService, SkillTagService>();
            services.AddTransient<ISchedulingCodeService, SchedulingCodeService>();
            services.AddTransient<ISchedulingCodeIconService, SchedulingCodeIconService>();
            services.AddTransient<ISchedulingCodeTypeService, SchedulingCodeTypeService>();
            services.AddTransient<ITimezoneService, TimezoneService>();
            services.AddTransient<ISkillGroupService, SkillGroupService>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            return services;

        }
    }
}
