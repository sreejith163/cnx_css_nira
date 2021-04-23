using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Business.Services;
using Css.Api.Job.Models.DTO.Common;
using Css.Api.Job.Models.DTO.Configurations;
using Css.Api.Job.Models.DTO.EStart;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
    /// The EStart Export job class
    /// </summary>
    public class EStartExportJob : CronJobService
    {
        #region Private Properties

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<EStartExportJob> _logger;

        /// <summary>
        /// The Http helper service
        /// </summary>
        private readonly IHttpService _httpService;

        /// <summary>
        /// The time service
        /// </summary>
        private readonly ITimeService _timeService;

        /// <summary>
        /// The EStart helper service
        /// </summary>
        private readonly IEStartService _eStartService;

        /// <summary>
        /// The cron job configurations
        /// </summary>
        private readonly CronJob _job;

        /// <summary>
        /// The filter for EStart job in the config
        /// </summary>
        private readonly List<SpanFilter> _filters;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize the properties
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="httpService"></param>
        /// <param name="timeService"></param>
        /// <param name="eStartService"></param>
        /// <param name="jobs"></param>
        public EStartExportJob(IScheduleConfig<EStartExportJob> config, ILogger<EStartExportJob> logger, IHttpService httpService, ITimeService timeService, IEStartService eStartService, IOptions<Jobs> jobs)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _httpService = httpService;
            _timeService = timeService;
            _eStartService = eStartService;
            _job = jobs.Value.Cron.First(x => x.Key.Equals("EStartExport"));
            _filters = JsonConvert.DeserializeObject<List<SpanFilter>>(_job.Filters);
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
            _logger.LogInformation("EStart export starts.");
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// The business implementation of the job
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task Process(CancellationToken cancellationToken)
        {
            var startTime = $"{DateTime.UtcNow:hh:mm:ss}";
            try
            {
                var timeDetails = await _timeService.GetTimeDetails(_filters);

                if (!timeDetails.Any())
                {
                    _logger.LogInformation($"EStart export run at {startTime} doesn't have any matching timezones with their current times close to either of {_eStartService.GetTimesOfDay(_filters)}.");
                    return;
                }
                var exportRequests = _eStartService.GenerateExportRequests(timeDetails);
                List<HttpRequestMessage> requestMessages = (from r in exportRequests
                                                            select _httpService
                                                                    .CreateHttpRequestMessage(_job, JsonConvert.SerializeObject(r))
                                                           ).ToList();

                var responseMessages = await _httpService.SendAsync(requestMessages);
                if (responseMessages.All(x => x.IsSuccessStatusCode))
                {
                    responseMessages.ForEach(async msg =>
                    {
                        var requestString = await msg.RequestMessage.Content.ReadAsStringAsync();
                        var responseString = await msg.Content.ReadAsStringAsync();
                        _logger.LogInformation($"Request - \n{requestString}\n Response - {responseString}\n");
                    });
                    _logger.LogInformation($"EStart export run at {startTime} completed.");
                }
                else if (responseMessages.All(x => !x.IsSuccessStatusCode))
                {
                    responseMessages.Where(x => x.IsSuccessStatusCode).ToList().ForEach(async msg =>
                    {
                        var requestString = await msg.RequestMessage.Content.ReadAsStringAsync();
                        var responseString = await msg.Content.ReadAsStringAsync();
                        _logger.LogInformation($"Request - \n{requestString}\n Response - {responseString}\n");
                    });
                    _logger.LogError($"EStart export run at {startTime} failed.");
                    await LogFailedRequests(startTime, responseMessages);
                }
                else
                {
                    responseMessages.Where(x => x.IsSuccessStatusCode).ToList().ForEach(async msg =>
                    {
                        var requestString = await msg.RequestMessage.Content.ReadAsStringAsync();
                        var responseString = await msg.Content.ReadAsStringAsync();
                        _logger.LogInformation($"Request - \n{requestString}\n Response - {responseString}\n");
                    });
                    _logger.LogInformation($"EStart export run at {startTime} processed partially.");
                    await LogFailedRequests(startTime, responseMessages);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"An exception occured in the EStartExport run at {startTime}. Expection details:\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// The stop method of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EStart export is stopping.");
            await base.StopAsync(cancellationToken);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// A helper method to log all the failed requests
        /// </summary>
        /// <param name="runTime">The job execution start time</param>
        /// <param name="responseMessages">The response messages</param>
        /// <returns></returns>
        private async Task LogFailedRequests(string runTime, List<HttpResponseMessage> responseMessages)
        {
            var failedRequests = responseMessages.Where(x => !x.IsSuccessStatusCode).ToList();
            foreach (var res in failedRequests)
            {
                var requestContent = await res.RequestMessage.Content.ReadAsStringAsync();
                var responseContent = await res.Content.ReadAsStringAsync();
                _logger.LogError($"EStart export run at {runTime} failed with {res.StatusCode}\n for request - {requestContent}\n and response - {responseContent}");
            }
        }
        #endregion
    }
}
