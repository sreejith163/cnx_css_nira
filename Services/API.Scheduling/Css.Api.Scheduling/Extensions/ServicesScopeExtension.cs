using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Scheduling.Business;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ITimezoneService, TimezoneService>();
            services.AddTransient<IActivityLogService, ActivityLogService>();
            services.AddTransient<IAgentAdminService, AgentAdminService>();
            services.AddTransient<IAgentScheduleService, AgentScheduleService>();
            services.AddTransient<IAgentScheduleManagerService, AgentScheduleManagerService>();
            services.AddTransient<IEntityHierarchyService, EntityHierarchyService>();

            services.AddTransient<IForecastScreenService, ForecastScreenService>();
            services.AddScoped<ITimezoneRepository, TimezoneRepository>();
            services.AddScoped<IAgentAdminRepository, AgentAdminRepository>();
            services.AddScoped<IAgentScheduleManagerRepository, AgentScheduleManagerRepository>();
            services.AddScoped<IAgentScheduleRepository, AgentScheduleRepository>();
            services.AddScoped<IForecastScreenRepository, ForecastScreenRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientLobGroupRepository, ClientLobGroupRepository>();
            services.AddScoped<ISkillGroupRepository, SkillGroupRepository>();
            services.AddScoped<ISkillTagRepository, SkillTagRepository>();
            services.AddScoped<IAgentSchedulingGroupRepository, AgentSchedulingGroupRepository>();
            services.AddScoped<ISchedulingCodeRepository, SchedulingCodeRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

            return services;

        }
    }
}
