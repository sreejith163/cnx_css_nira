using Css.Api.Job.Business.CronJobs;
using Css.Api.Job.Business.Extensions;
using Css.Api.Job.Models.DTO.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Job.Extensions
{
    /// <summary>
    /// An extension class to add scheduled jobs within the project
    /// </summary>
    public static class ScheduledJobsExtension
    {
        /// <summary>
        /// An extension  method to add all jobs in the application
        /// </summary>
        /// <param name="services">The app service collection</param>
        /// <param name="configuration">The app configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Jobs>(configuration.GetSection("Jobs"));
            ServiceProvider provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<Jobs>>();

            services
                .AddJobFramework(configuration)
                .ConfigureCronJobs(options.Value.Cron);

            return services;
        }

        /// <summary>
        /// A extension method to configure the cron jobs using the configuration json values
        /// </summary>
        /// <param name="services">The app service collection</param>
        /// <param name="cronJobs">the list of cron jobs read from the configuration json</param>
        /// <returns></returns>
        private static IServiceCollection ConfigureCronJobs(this IServiceCollection services, List<CronJob> cronJobs)
        {
            services.AddCronJob<UDWImportJob>(c =>
            {
                c.TimeZoneInfo = GetTimeZoneInfo("UDWImport", cronJobs);
                c.CronExpression = GetCronExpression("UDWImport", cronJobs);
            });

            services.AddCronJob<EStartImportJob>(c =>
            {
                c.TimeZoneInfo = GetTimeZoneInfo("EStartImport", cronJobs);
                c.CronExpression = GetCronExpression("EStartImport", cronJobs);
            });

            services.AddCronJob<EStartExportJob>(c =>
            {
                c.TimeZoneInfo = GetTimeZoneInfo("EStartExport", cronJobs);
                c.CronExpression = GetCronExpression("EStartExport", cronJobs);
            });

            services.AddCronJob<EStartExportIntraDayJob>(c =>
            {
                c.TimeZoneInfo = GetTimeZoneInfo("EStartExportIntraDay", cronJobs);
                c.CronExpression = GetCronExpression("EStartExportIntraDay", cronJobs);
            });

            services.AddCronJob<EStartEmpExportJob>(c =>
            {
                c.TimeZoneInfo = GetTimeZoneInfo("EStartEmpExport", cronJobs);
                c.CronExpression = GetCronExpression("EStartEmpExport", cronJobs);
            });

            return services;
        }
        
        /// <summary>
        /// A method to get the cron expression mapped in the configuration json for the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="cronJobs">The list of all cron jobs present in the configuration json</param>
        /// <returns></returns>
        private static string GetCronExpression(string key, List<CronJob> cronJobs)
        {
            return cronJobs.First(x => x.Key.Equals(key)).CronExpression;
        }

        /// <summary>
        /// A method to get the timezone mapped in the configuration json for the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="cronJobs">The list of all cron jobs present in the configuration json</param>
        /// <returns></returns>
        private static TimeZoneInfo GetTimeZoneInfo(string key, List<CronJob> cronJobs)
        {
            var timeZone = cronJobs.First(x => x.Key.Equals(key)).TimeZone;
            if (string.IsNullOrWhiteSpace(timeZone))
                timeZone = "UTC";
            return TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        }
    }
}
