using AutoMapper;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.CNX1;
using Css.Api.Reporting.Models.DTO.Request.Common;
using Css.Api.Reporting.Models.DTO.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Sources
{
    /// <summary>
    /// The CNX1 Export source
    /// </summary>
    public class CNX1ExportSource : ApiExportService, ISource
    {
        #region Private Properties

        /// <summary>
        /// The mapper service
        /// </summary>
        private readonly IMapperService _mapperService;

        /// <summary>
        /// The schedule service
        /// </summary>
        private readonly IScheduleService _scheduleService;
        #endregion

        #region Public Properties
        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name => "CNX1Exp";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="mapperService"></param>
        /// <param name="scheduleService"></param>
        public CNX1ExportSource(IMapperService mapperService, IScheduleService scheduleService)
        {
            _mapperService = mapperService;
            _scheduleService = scheduleService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to pull the data from the source
        /// </summary>
        /// <returns>A list of instances of DataFeed</returns>
        public async Task<List<DataFeed>> Pull()
        {
            return await Process();
        }

        /// <summary>
        /// The business logic to process the API export
        /// </summary>
        /// <returns>An instance of ActivityApiResponse</returns>
        public override async Task<ActivityApiResponse> Export()
        {
            var response = new ActivityApiResponse()
            {
                StatusCode = HttpStatusCode.OK
            };

            var cnxFilter = await Task.FromResult(_mapperService.GetQueryParams<CNX1Filter>());

            if (!(cnxFilter.AgentIds.Any() && cnxFilter.StartDate <= cnxFilter.EndDate && cnxFilter.StartDate != DateTime.MinValue & cnxFilter.EndDate != DateTime.MinValue))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = Messages.InvalidQueryParamsCNX1;
                return response;
            }

            ScheduleFilter filter = new ScheduleFilter()
            {
                AgentIds = cnxFilter.AgentIds,
                StartDate = cnxFilter.StartDate,
                EndDate = cnxFilter.EndDate
            };
            
            List<CalendarChart> calenderCharts = await _scheduleService.GetCalendarChartsUsingIds(filter);
            List<AgentActivitySchedule> agentActivitySchedulesList =  _scheduleService.GenerateActivityAgentSchedules(calenderCharts);

            response.Data = agentActivitySchedulesList;

            return response;
        }
        #endregion
    }
}
