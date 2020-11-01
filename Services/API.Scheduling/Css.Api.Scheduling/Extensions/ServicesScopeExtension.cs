using AutoMapper;
using Css.Api.Core.Utilities;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Scheduling.Business;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.Interface;
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<ISortHelper<Client>, SortHelper<Client>>();
            services.AddTransient<ISortHelper<SchedulingCode>, SortHelper<SchedulingCode>>();

            services.AddTransient<IDataShaper<Client>, DataShaper<Client>>();
            services.AddTransient<IDataShaper<SchedulingCode>, DataShaper<SchedulingCode>>();

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ISchedulingCodeService, SchedulingCodeService>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            return services;

        }
    }
}
