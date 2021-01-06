using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Reporting.Business.Extensions;
using Css.Api.Reporting.Models.DTO.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System;

namespace Css.Api.Reporting.Extensions
{
    /// <summary>
    /// The extension service for all service additions and configurations
    /// </summary>
    public static class ServiceScopeExtension
    {
        /// <summary>
        /// An extension method to configure all the reporting business services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="HostingEnvironment"></param>
        /// <param name="configuration"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        public static IServiceCollection AddServicesScope(this IServiceCollection services,
                                                               IWebHostEnvironment HostingEnvironment,
                                                               IConfiguration configuration)
        {
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

            services.Configure<MapperSettings>(configuration.GetSection("Mappers"));

            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddReportingFramework();

            return services;
        }

        
    }
}
