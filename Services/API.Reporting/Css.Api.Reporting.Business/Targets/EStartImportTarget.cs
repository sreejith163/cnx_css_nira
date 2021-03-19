using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Common;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Targets
{
    /// <summary>
    /// The EStart import target
    /// </summary>
    public class EStartImportTarget : FTPImportService, ITarget
    {
        #region Private Properties

        /// <summary>
        /// The auto mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The clock helper service
        /// </summary>
        private readonly IScheduleService _scheduleService;

        /// <summary>
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The activity log repository
        /// </summary>
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// The transation support of mongo
        /// </summary>
        private readonly IUnitOfWork _uow;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name => "EStartImpTar";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="scheduleClockService"></param>
        /// <param name="activityLogRepository"></param>
        /// <param name="agentScheduleManagerRepository"></param>
        /// <param name="uow"></param>
        /// <param name="ftp"></param>
        public EStartImportTarget(IMapper mapper, IScheduleService scheduleClockService, 
            IActivityLogRepository activityLogRepository, IAgentScheduleManagerRepository agentScheduleManagerRepository, 
            IUnitOfWork uow, IFTPService ftp) : base(ftp)
        {
            _mapper = mapper;
            _scheduleService = scheduleClockService;
            _activityLogRepository = activityLogRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _uow = uow;
        }
        #endregion        

        #region Public Methods

        /// <summary>
        /// The method to push data to the destination
        /// </summary>
        /// <param name="feeds">List of instances of DataFeed (sources)</param>
        /// <returns>An instance of StrategyResponse</returns>
        public async Task<StrategyResponse> Push(List<DataFeed> feeds)
        {
            foreach (DataFeed feed in feeds)
            {
                await Process(feed);
            }
            return ImportFeedback;
        }

        /// <summary>
        /// The business logic to process the import is written here
        /// </summary>
        /// <param name="data">The input byte[] to be processed</param>
        /// <returns>An instance of ActivityDataResponse</returns>
        public override async Task<ActivityDataResponse> Import(byte[] data)
        {
            try
            {
                var content = Encoding.Default.GetString(data);
                
                List<string> invalidData;
                List<CalendarChart> calendarCharts = _scheduleService.ParseCalenderCharts(content, out invalidData, true);
                
                if (calendarCharts.Any())
                {
                    ActivityInformation activityInformation = new ActivityInformation
                    {
                        Origin = ActivityOrigin.EStart,
                        Status = ActivityStatus.Updated,
                        Type = ActivityType.SchedulingmanagerGrid
                    };

                    List<ScheduleManagerDetails> scheduleManagerDetailsList = await _scheduleService.GenerateAgentScheduleManagerCharts(calendarCharts, activityInformation);

                    var agentScheduleManagers = _mapper.Map<List<AgentScheduleManager>>(scheduleManagerDetailsList).ToList();
                    _agentScheduleManagerRepository.UpsertAgentScheduleManagers(agentScheduleManagers);

                    var activityLogs = _mapper.Map<List<ActivityLog>>(scheduleManagerDetailsList).ToList();
                    _activityLogRepository.CreateActivityLogs(activityLogs);

                    await _uow.Commit();
                }

                if (invalidData.Any())
                {
                    return new ActivityDataResponse()
                    {
                        Status = (int)ProcessStatus.Partial,
                        Metadata = string.Join("\r\n", invalidData)
                    };
                }

                return new ActivityDataResponse()
                {
                    Status = (int)ProcessStatus.Success
                };
            }
            catch(Exception ex)
            {
                return new ActivityDataResponse()
                {
                    Status = (int)ProcessStatus.Failed,
                    Metadata = ex.Message + "at - \n\n" + ex.StackTrace
                };
            }
        }
        #endregion
    }
}
