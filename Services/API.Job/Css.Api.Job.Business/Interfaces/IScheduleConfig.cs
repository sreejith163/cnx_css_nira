using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Business.Interfaces
{
    /// <summary>
    /// An interface for configuring schedules
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduleConfig<T>
        where T : class
    {
        /// <summary>
        /// The cron expression
        /// </summary>
        string CronExpression { get; set; }

        /// <summary>
        /// The timezone of the system
        /// </summary>
        TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
