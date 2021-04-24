using AutoMapper;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.CNX1;
using Css.Api.Reporting.Models.DTO.Request.Common;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Repository.Interfaces;
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

        /// <summary>
        /// The agent repository
        /// </summary>
        private readonly IAgentRepository _agentRepository;
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
        /// <param name="agentRepository"></param>
        public CNX1ExportSource(IMapperService mapperService, IScheduleService scheduleService, IAgentRepository agentRepository)
        {
            _mapperService = mapperService;
            _scheduleService = scheduleService;
            _agentRepository = agentRepository;
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
            var response = new ActivityApiResponse();

            var cnxFilter = await Task.FromResult(_mapperService.GetQueryParams<CNX1Filter>());

            if (!(cnxFilter.AgentIds.Any() && cnxFilter.StartDate <= cnxFilter.EndDate && cnxFilter.StartDate != DateTime.MinValue & cnxFilter.EndDate != DateTime.MinValue))
            {
                response.Message = Messages.InvalidQueryParamsCNX1;
                return response;
            }

            ScheduleFilter filter = new ScheduleFilter()
            {
                AgentIds = cnxFilter.AgentIds.Select(x => { return x.Trim(); }).ToList(),
                StartDate = cnxFilter.StartDate,
                EndDate = cnxFilter.EndDate
            };

            var agents = await _agentRepository.GetAgents(filter.AgentIds);

            List<AgentActivitySchedule> agentActivitySchedulesList = filter.AgentIds.Where(ag => !agents.Select(x =>x.Ssn).Contains(ag))
                                                                          .Select(x => new AgentActivitySchedule() 
                                                                                {
                                                                                    AgentId = x, 
                                                                                    Message = Messages.AgentNotFound 
                                                                                })
                                                                          .ToList();

            filter.AgentIds.RemoveAll(x => agentActivitySchedulesList.Select(y => y.AgentId).Contains(x));
            
            if(filter.AgentIds.Any())
            {
                List<CalendarChart> calenderCharts = await _scheduleService.GetCalendarChartsUsingIds(filter);
                var schedules = _scheduleService.GenerateActivityAgentSchedules(calenderCharts);
                var missingSchedules = filter.AgentIds
                                        .Where(x => !schedules.Select(y => y.AgentId).Contains(x))
                                        .Select(z => new AgentActivitySchedule 
                                            { 
                                                AgentId = z, 
                                                Message = string.Format(Messages.NoSchedulesPresent, filter.StartDate.ToString("yyyy-MM-dd"), filter.EndDate.ToString("yyyy-MM-dd"))
                                        })
                                        .ToList();
                agentActivitySchedulesList.AddRange(schedules);
                agentActivitySchedulesList.AddRange(missingSchedules);
            }
            
            response.Data = agentActivitySchedulesList.OrderBy(x => x.AgentId).ToList();

            return response;
        }
        #endregion
    }
}
