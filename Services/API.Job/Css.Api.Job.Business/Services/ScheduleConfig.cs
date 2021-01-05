using Css.Api.Job.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Business.Services
{
    /// <summary>
    /// The class with the configurations of the schedule
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScheduleConfig<T> : IScheduleConfig<T>
        where T : class
    {
        /// <summary>
        /// The cron expression
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// The timezone of the system
        /// </summary>
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
