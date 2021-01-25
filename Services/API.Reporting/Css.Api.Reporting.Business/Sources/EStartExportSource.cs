using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Sources
{
    /// <summary>
    /// The estart export source class
    /// </summary>
    public class EStartExportSource : ISource
    {
        #region Private Properties

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

        /// <summary>
        /// The scheduling code repository
        /// </summary>
        private readonly ISchedulingCodeRepository _schedulingCodeRepository;

        /// <summary>
        /// The schedule clock helper service
        /// </summary>
        private readonly IScheduleClockService _scheduleClockService;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the source
        /// </summary>
        public string Name => "EStartExp";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="agentScheduleRepository"></param>
        /// <param name="schedulingCodeRepository"></param>
        /// <param name="scheduleClockService"></param>
        public EStartExportSource(IAgentScheduleRepository agentScheduleRepository, ISchedulingCodeRepository schedulingCodeRepository, IScheduleClockService scheduleClockService)
        {
            _agentScheduleRepository = agentScheduleRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
            _scheduleClockService = scheduleClockService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to pull the data from the source
        /// </summary>
        /// <returns>A list of instances of DataFeed</returns>
        public async Task<List<DataFeed>> Pull()
        {
            var reportDate = DateTime.UtcNow;
            var agentSchedules = await _agentScheduleRepository.GetSchedules(reportDate);
            var codes = await _schedulingCodeRepository.GetSchedulingCodes();
            var clockData = _scheduleClockService.GenerateClocks(reportDate, agentSchedules, codes);

            return new List<DataFeed>() {
                new DataFeed()
                {
                    Feeder = DataOptions.Mongo.GetDescription(),
                    Content = Encoding.Default.GetBytes(JsonConvert.SerializeObject(clockData))
                }
            };
        }
        #endregion
    }
}
