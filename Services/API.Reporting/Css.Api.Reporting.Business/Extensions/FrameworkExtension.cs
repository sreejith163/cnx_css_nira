using Css.Api.Reporting.Business.Factories;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Resolvers;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Business.Strategies;
using Css.Api.Reporting.Repository;
using Css.Api.Reporting.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Extensions
{
    /// <summary>
    /// An extension class for resolving all framework services
    /// </summary>
    public static class FrameworkExtension
    {
        /// <summary>
        /// The extension method to add all services pertaining to the reporting framework
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddReportingFramework(this IServiceCollection services)
        {
            services
                .AddBaseServices()
                .AddFactories()
                .AddStrategies()
                .AddImportServices()
                .AddRepositories();

            return services;
        }

        /// <summary>
        /// The extension method to add the base services in the reporting framework
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            services.AddSingleton<IEncrypter, Encrypter>();
            services.AddSingleton<IFTPService, FTPService>();
            services.AddScoped<IServiceResolver, ServiceResolver>();
            return services;
        }

        /// <summary>
        /// The extension method to configure all factories
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddScoped<IServiceFactory, ServiceFactory>();
            return services;
        }

        /// <summary>
        /// The extension method to configure all strategies
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddStrategies(this IServiceCollection services)
        {
            services.AddScoped<IImportStrategy, ImportStrategy>();
            services.AddScoped<IExportStrategy, ExportStrategy>();
            return services;
        }

        /// <summary>
        /// The extension method to configure all import services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddImportServices(this IServiceCollection services)
        {
            services.AddScoped<IImporter, UDWImport>();
            return services;
        }

        /// <summary>
        /// The extension method to add repository services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAgentRepository, AgentRepository>();
            return services;
        }
    }
}
