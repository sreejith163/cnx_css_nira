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
        /// The service provider
        /// </summary>
        private readonly IHttpClientFactory _httpClient;

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
        /// <param name="httpClient">The http client factory</param>
        /// <param name="jobs">The jobs configuration</param>
        public UDWImportJob(IScheduleConfig<UDWImportJob> config, ILogger<UDWImportJob> logger, IHttpClientFactory httpClient, IOptions<Jobs> jobs)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _httpClient = httpClient;
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
            var client = _httpClient.CreateClient();
            try
            { 
                var reqMessage = CreateHttpRequestMessage();
                var responseMessage = await client.SendAsync(reqMessage);
                var response = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"UDWCronJob run at {DateTime.Now:hh:mm:ss} completed. Response details:\n{response}");
                }
                else
                {
                    _logger.LogError($"UDWCronJob run at {DateTime.Now:hh:mm:ss} failed. Response details:\n{response}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"An exception occured in the UDWCronJob run at {DateTime.Now:hh:mm:ss}. Expection details:\n{ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                client.Dispose();
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

        #region Private Methods

        /// <summary>
        /// Method to create the HTTPRequestMessage using the job attributes
        /// </summary>
        /// <returns></returns>
        private HttpRequestMessage CreateHttpRequestMessage()
        {
            var reqMessage = new HttpRequestMessage();
            reqMessage.RequestUri = new Uri(_job.Url);
            
            if(_job.Headers != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in _job.Headers)
                {
                    reqMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            reqMessage.Method = new HttpMethod(_job.Method);
            
            if(!string.IsNullOrWhiteSpace(_job.Content))
            {
                reqMessage.Content = new StringContent(_job.Content);
            }

            return reqMessage;
        }
        #endregion
    }
}
