﻿using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Business.Extensions
{
    /// <summary>
    /// An extension class for all generic job extensions
    /// </summary>
    public static class ScheduledServiceExtension
    {
        /// <summary>
        /// A generic extension method to add a cron job
        /// </summary>
        /// <typeparam name="T">A class of base type 'CronJobService'</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="options">The cron configuration details</param>
        /// <returns></returns>
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options)
            where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
            return services;
        }
    }
}
