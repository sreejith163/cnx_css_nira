using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Factories;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Middlewares;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Business.Sources;
using Css.Api.Reporting.Business.Strategies;
using Css.Api.Reporting.Business.Targets;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository;
using Css.Api.Reporting.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Css.Api.Reporting.Business.Extensions
{
    /// <summary>
    /// An extension class for resolving all framework services
    /// </summary>
    public static class FrameworkExtension
    {
        /// <summary>
        /// The extension method to add framework middlewares to the request pipeline
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseReportingFramework(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MappingMiddleware>();
        }

        /// <summary>
        /// The extension method to add all services pertaining to the reporting framework
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddReportingFramework(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services
                .AddBaseServices()
                .AddFactories()
                .AddStrategies()
                .AddActivityServices()
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
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IBatchService, BatchService>();
            services.AddScoped<IMapperService, MapperService>();
            services.AddScoped<IFTPService, FTPService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IDispatchService, DispatchService>();
            services.AddMongoConfiguration();
            return services;
        }

        /// <summary>
        /// The extension method to add Mongo service to the scope
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddMongoConfiguration(this IServiceCollection services)
        {
            var mongoSettings = services.BuildServiceProvider().GetRequiredService<IOptions<MapperSettings>>().Value
                            .DataOptions.FirstOrDefault(x => x.Type.Equals(DataOptions.Mongo.GetDescription()));
            if (mongoSettings != null)
            {
                services.AddScoped<IMongoDbSettings>(o => new MongoDbSettings()
                {
                    ConnectionString = mongoSettings.Options["ConnectionString"],
                    DatabaseName = mongoSettings.Options["DatabaseName"]
                });
            }

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
            services.AddScoped<IActivityStrategy, ActivityStrategy>();
            services.AddScoped<IDispatchStrategy, DispatchStrategy>();
            return services;
        }

        /// <summary>
        /// The extension method to configure all activity services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The same instance of IServiceCollection</returns>
        private static IServiceCollection AddActivityServices(this IServiceCollection services)
        {
            services.AddScoped<ISource, UDWImportSource>();
            services.AddScoped<ITarget, UDWImportTarget>();
            services.AddScoped<ISource, EStartExportSource>();
            services.AddScoped<ITarget, EStartExportTarget>();
            services.AddScoped<ISource, EStartImportSource>();
            services.AddScoped<ITarget, EStartImportTarget>();
            services.AddScoped<ISource, CNX1ExportSource>();
            services.AddScoped<ITarget, CNX1ImportTarget>();
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
            services.AddScoped<IAgentSchedulingGroupRepository, AgentSchedulingGroupRepository>();
            services.AddScoped<IAgentSchedulingGroupHistoryRepository, AgentSchedulingGroupHistoryRepository>();
            services.AddScoped<IAgentScheduleRepository, AgentScheduleRepository>();
            services.AddScoped<IAgentScheduleManagerRepository, AgentScheduleManagerRepository>();
            services.AddScoped<ISchedulingCodeRepository, SchedulingCodeRepository>();
            services.AddScoped<IAgentCategoryRepository, AgentCategoryRepository>();
            services.AddScoped<ITimezoneRepository, TimezoneRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            return services;
        }
    }
}
