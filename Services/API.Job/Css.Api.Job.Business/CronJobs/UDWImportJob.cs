using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Business.Services;
using Css.Api.Job.Models.DTO.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.CronJobs
{
    /// <summary>
    /// The cron job for UDW import
    /// </summary>
    public class UDWImportJob : CronJobService
    {
        #region Private Properties

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UDWImportJob> _logger;

        /// <summary>
        /// The Http helper service
        /// </summary>
        private readonly IHttpService _httpService;

        /// <summary>
        /// The cron job configurations
        /// </summary>
        private readonly CronJob _job;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize the properties
        /// </summary>
        /// <param name="config">The schedule configurations</param>
        /// <param name="logger">The logger</param>
        /// <param name="httpService">The http helper service</param>
        /// <param name="jobs">The jobs configuration</param>
        public UDWImportJob(IScheduleConfig<UDWImportJob> config, ILogger<UDWImportJob> logger, IHttpService httpService, IOptions<Jobs> jobs)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _httpService = httpService;
            _job = jobs.Value.Cron.First(x => x.Key.Equals("UDWImport"));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The start method of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UDWCronJob starts.");
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// The business implementation of the job
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task Process(CancellationToken cancellationToken)
        {
            string startTime = $"{DateTime.UtcNow:hh:mm:ss}";
            try
            {
                var reqMessage = _httpService.CreateHttpRequestMessage(_job);
                var responseMessage = await _httpService.SendAsync(reqMessage, 15);
                var response = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"UDWCronJob run at {startTime} completed. Response details:\n{response}");
                }
                else
                {
                    _logger.LogError($"UDWCronJob run at {startTime} failed. Response details:\n{response}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"An exception occured in the UDWCronJob run at {startTime}. Expection details:\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// The stop method of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UDWCronJob is stopping.");
            await base.StopAsync(cancellationToken);
        }
        #endregion
    }
}
