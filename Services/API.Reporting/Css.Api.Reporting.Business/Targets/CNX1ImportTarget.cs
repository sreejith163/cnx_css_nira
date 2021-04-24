using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Targets
{
    /// <summary>
    /// The CNX1 Import target class
    /// </summary>
    public class CNX1ImportTarget : ApiImportService, ITarget
    {
        #region Private Methods

        /// <summary>
        /// The auto mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The activity log repository
        /// </summary>
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// The schedule service
        /// </summary>
        private readonly IScheduleService _scheduleService;

        /// <summary>
        /// The transation support of mongo
        /// </summary>
        private readonly IUnitOfWork _uow;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name => "CNX1ImpTar";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize the properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="agentScheduleManagerRepository"></param>
        /// <param name="activityLogRepository"></param>
        /// <param name="scheduleService"></param>
        /// <param name="uow"></param>
        public CNX1ImportTarget(IMapper mapper, IAgentScheduleManagerRepository agentScheduleManagerRepository, 
            IActivityLogRepository activityLogRepository, IScheduleService scheduleService, IUnitOfWork uow)
        {
            _mapper = mapper;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _activityLogRepository = activityLogRepository;
            _scheduleService = scheduleService;
            _uow = uow;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to push data to the target
        /// </summary>
        /// <param name="feeds"></param>
        /// <returns>An instance of StrategyResponse</returns>
        public async Task<StrategyResponse> Push(List<DataFeed> feeds)
        {
            return await Process(feeds);
        }

        /// <summary>
        /// The business logic to process the API import
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public override async Task<ActivityApiData> Import(DataFeed feed)
        {
            var content = Encoding.Default.GetString(feed.Content);
            var scheduleUpdate = JsonConvert.DeserializeObject<ActivityScheduleUpdate>(content);
            scheduleUpdate.AgentId = scheduleUpdate.AgentId.Trim();

            ActivityApiData activityApiData = new ActivityApiData()
            {
                AgentId = scheduleUpdate.AgentId,
                ScheduleDate = scheduleUpdate.ScheduleDate
            };
            
            try
            {
                ScheduleData scheduleData = await _scheduleService.ParseActivitySchedule(scheduleUpdate);
                if (scheduleData.Messages.Any())
                {
                    activityApiData.Message = string.Join('\n', scheduleData.Messages);
                    activityApiData.Status = ProcessStatus.Failed.ToString();
                }
                else if (scheduleData.Schedule.Charts.Any())
                {
                    _agentScheduleManagerRepository.UpsertAgentScheduleManagers(new List<AgentScheduleManager>() { scheduleData.Schedule });

                    scheduleData.Origin = ActivityOrigin.CNX1;
                    scheduleData.Status = scheduleData.Schedule.ModifiedDate != null ? ActivityStatus.Updated : ActivityStatus.Created;
                    scheduleData.Type = ActivityType.SchedulingmanagerGrid;
                    var activityLog = _mapper.Map<ActivityLog>(scheduleData);
                    _activityLogRepository.CreateActivityLogs(new List<ActivityLog>() { activityLog });

                    await _uow.Commit();
                    activityApiData.Message = string.Empty;
                    activityApiData.Status = ProcessStatus.Success.ToString();
                }
                else
                {
                    activityApiData.Message = Messages.NoSchedulesToUpdate;
                    activityApiData.Status = ProcessStatus.Failed.ToString();
                }
            }
            catch (Exception ex)
            {
                activityApiData.Message = ex.Message;
                activityApiData.Status = ProcessStatus.Failed.ToString();
            }

            return activityApiData;
        }
        #endregion
    }
}
