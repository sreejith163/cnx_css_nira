using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.DataAccess.Repository.UnitOfWork;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Reporting.Business.Factories;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Business.Strategies;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Repository;
using Css.Api.Reporting.Repository.Interfaces;
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

            services
                .AddFactories()
                .AddStrategies()
                .AddImportServices()
                .AddRepositories();

            return services;
        }

        /// <summary>
        /// The extension method to configure all factories
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<IEncrypter, Encrypter>();
            services.AddSingleton<IFTPService, FTPService>();
            services.AddSingleton<IServiceFactory, ServiceFactory>();
            services.AddSingleton<IServiceResolver, ServiceResolver>();
            return services;
        }

        /// <summary>
        /// The extension method to configure all strategies
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddStrategies(this IServiceCollection services)
        {
            services.AddSingleton<IImportStrategy, ImportStrategy>();
            services.AddSingleton<IExportStrategy, ExportStrategy>();
            return services;
        }

        /// <summary>
        /// The extension method to configure all import services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddImportServices(this IServiceCollection services)
        {
            services.AddSingleton<IImporter, UDWImport>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAgentRepository, AgentRepository>();
            return services;
        }
    }
}
