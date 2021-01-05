using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.Services
{
    /// <summary>
    /// The base abstract class for all cron jobs 
    /// </summary>
    public abstract class CronJobService : IHostedService, IDisposable
    {
        #region Private Properties

        /// <summary>
        /// the timer
        /// </summary>
        private System.Timers.Timer _timer;

        /// <summary>
        /// The cron expression
        /// </summary>
        private readonly CronExpression _expression;

        /// <summary>
        /// the timezone property
        /// </summary>
        private readonly TimeZoneInfo _timeZoneInfo;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="cronExpression">a cron expression string</param>
        /// <param name="timeZoneInfo">The timezone of the system</param>
        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The start method of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        /// <summary>
        /// The stop method of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        /// <summary>
        /// The dispose method
        /// </summary>
        public virtual void Dispose()
        {
            _timer?.Dispose();
        }

        /// <summary>
        /// The abstract method for business implementation of the hosted service when triggered
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task Process(CancellationToken cancellationToken);
        #endregion

        #region Protected Methods

        /// <summary>
        /// The scheduling method of the job
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await Process(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
        }
        #endregion
    }
}
