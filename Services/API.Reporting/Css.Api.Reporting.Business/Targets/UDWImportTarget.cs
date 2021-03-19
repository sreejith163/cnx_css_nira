using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Targets
{
    /// <summary>
    /// The UDW import target service
    /// </summary>
    public class UDWImportTarget : FTPImportService, ITarget
    {
        #region Private Properties

        /// <summary>
        /// The automapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The agent repository
        /// </summary>
        private readonly IAgentRepository _agentRepository;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

        /// <summary>
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The agent scheduling group history repository
        /// </summary>
        private readonly IAgentSchedulingGroupHistoryRepository _agentSchedulingGroupHistoryRepository;

        /// <summary>
        /// The timezone repository
        /// </summary>
        private readonly ITimezoneRepository _timezoneRepository;

        /// <summary>
        /// The transation support of mongo
        /// </summary>
        private readonly IUnitOfWork _uow;

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the target which is used to map in the mapper json
        /// </summary>
        public string Name => "UDWImpTar";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="agentRepository"></param>
        /// <param name="agentSchedulingGroupRepository"></param>
        /// <param name="agentSchedulingGroupHistoryRepository"></param>
        /// <param name="agentScheduleRepository"></param>
        /// <param name="agentScheduleManagerRepository"></param>
        /// <param name="uow"></param>
        /// <param name="ftp"></param>
        public UDWImportTarget(IMapper mapper, IAgentRepository agentRepository, IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IAgentSchedulingGroupHistoryRepository agentSchedulingGroupHistoryRepository, IAgentScheduleRepository agentScheduleRepository, 
            IAgentScheduleManagerRepository agentScheduleManagerRepository, ITimezoneRepository timezoneRepository, IUnitOfWork uow, IFTPService ftp) : base(ftp)
        {
            _mapper = mapper;
            _agentRepository = agentRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _agentSchedulingGroupHistoryRepository = agentSchedulingGroupHistoryRepository;
            _timezoneRepository = timezoneRepository;
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
                XMLParser<UDWAgentList> parser = new XMLParser<UDWAgentList>();
                UDWAgentList root = parser.Deserialize(data);
                var agents = _mapper.Map<List<Agent>>(root.NewAgents)
                    .Union(_mapper.Map<List<Agent>>(root.ChangedAgents))
                    .ToList();

                var metadata = await CheckImport(root, agents);
                
                if(agents.Any())
                {
                    _agentRepository.Upsert(agents);
                    
                    var agentSchedulingGroupHistories = _mapper.Map<List<AgentSchedulingGroupHistory>>(agents).ToList();
                    var agentSchedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsByIds(agentSchedulingGroupHistories.Select(x => x.AgentSchedulingGroupId).ToList());
                    var timezones = await _timezoneRepository.GetTimezones(agentSchedulingGroups.Select(x => x.TimezoneId).ToList());

                    var agentSchedules = _mapper.Map<List<AgentSchedule>>(agents).ToList();
                    await CheckExistingSchedules(agentSchedules, agentSchedulingGroups, timezones);
                    _agentScheduleRepository.InsertAgentSchedules(agentSchedules);

                    ReconcileStartDates(agentSchedulingGroupHistories, agentSchedulingGroups, timezones);
                    _agentSchedulingGroupHistoryRepository.UpdateAgentSchedulingGroupHistory(agentSchedulingGroupHistories);

                    await _uow.Commit();
                }

                if (!string.IsNullOrWhiteSpace(metadata))
                {
                    return new ActivityDataResponse()
                    {
                        Status = (int)ProcessStatus.Partial,
                        Metadata = metadata
                    };
                }

                return new ActivityDataResponse()
                {
                    Status = (int)ProcessStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new ActivityDataResponse()
                {
                    Status = (int)ProcessStatus.Failed,
                    Metadata = ex.Message + "at - \n\n" + ex.StackTrace
                };
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// The method to check any mismatches in the source and destination data
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns>Returns an empty string if it matches, else the serialized array of all the partially imported sources data.</returns>
        private async Task<string> CheckImport(UDWAgentList source, List<Agent> dest)
        {
            var existingAgents = await _agentRepository.GetAgents(dest.Select(x => x.Ssn).ToList());
            AssignAgentCategories(existingAgents, dest);
            
            var metadata = string.Empty;
            
            var schedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroups();
            var refIds = schedulingGroups.Select(x => x.RefId).ToList();

            var newAgents = source.NewAgents.Where(x => x.SSN == 0)
                            .Union(
                                (from ag in source.NewAgents.Where(x => x.SSN != 0)
                                    join up in dest on ag.SSN equals up.Ssn
                                    where (ag.SenDate != null && up.SenDate == null)
                                    || (ag.MU == null) 
                                    || (ag.SenExt != null && up.SenExt == null)
                                    select ag)
                            )// remove below code when MU change or move agent scenario is handled in UDW import
                            //.Union(
                            //    (from ag in source.NewAgents
                            //    join eag in existingAgents on ag.SSN equals eag.Ssn
                            //    where !(ag.MUString.Equals(eag.Mu))
                            //    select ag)
                            //)
                            .ToList();
            
            var invalidSchedulingGroupNewAgents = source.NewAgents.Where(x => !refIds.Contains(x.MU)).ToList();
            newAgents = newAgents.Union(invalidSchedulingGroupNewAgents).Distinct().ToList();

            var changeAgents = source.ChangedAgents.Where(x => x.SSN == 0)
                                .Union(
                                    (from ag in source.ChangedAgents.Where(x => x.SSN != 0)
                                    join up in dest on ag.SSN equals up.Ssn
                                    where (ag.SenDate != null && up.SenDate == null)
                                    || (ag.SenExt != null && up.SenExt == null)
                                    select ag)
                                )// remove below code when MU change or move agent scenario is handled in UDW import
                                //.Union(
                                //    (from ag in source.ChangedAgents
                                //    join eag in existingAgents on ag.SSN equals eag.Ssn
                                //    where ag.MU != null && !(ag.MUString.Equals(eag.Mu))
                                //    select ag)
                                //)
                                .ToList();
            
            var invalidSchedulingGroupChangeAgents = source.ChangedAgents.Where(x => x.MU.HasValue && !refIds.Contains(x.MU)).ToList();
            changeAgents = changeAgents.Union(invalidSchedulingGroupChangeAgents).Distinct().ToList();

            var ignoreAgents = newAgents.Select(x => x.SSN).Union(changeAgents.Select(x => x.SSN)).ToList();
            dest.RemoveAll(x => ignoreAgents.Contains(x.Ssn));
            
            dest.ForEach(x =>
            {
                var schedulingGroup = schedulingGroups.FirstOrDefault(y => y.RefId.ToString() == x.Mu);
                if (schedulingGroup != null)
                {
                    x.AgentSchedulingGroupId = schedulingGroup.AgentSchedulingGroupId;
                    x.ClientId = schedulingGroup.ClientId;
                    x.ClientLobGroupId = schedulingGroup.ClientLobGroupId;
                    x.SkillGroupId = schedulingGroup.SkillGroupId;
                    x.SkillTagId = schedulingGroup.SkillTagId;
                }
            });

            var mismatch = new UDWAgentList();
            if (newAgents.Any())
            {
                mismatch.NewAgents = newAgents;
            }

            if (changeAgents.Any())
            {
                mismatch.ChangedAgents = changeAgents;
            }

            if ((mismatch.NewAgents != null && mismatch.NewAgents.Any())
                || (mismatch.ChangedAgents != null && mismatch.ChangedAgents.Any())
            )
            {
                XMLParser<UDWAgentList> parser = new XMLParser<UDWAgentList>();
                metadata = parser.Serialize(mismatch);
            }

            return metadata;
        }

        /// <summary>
        /// The method to set and merge any existing agent categories with new ones
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        private void AssignAgentCategories(List<Agent> existingAgents, List<Agent> dest)
        {
            var agentCategories = (from a in existingAgents
                                   join d in dest on a.Ssn equals d.Ssn
                                   let cData = a.AgentData != null && d.AgentData != null ? (from ad in a.AgentData
                                                join dd in d.AgentData on ad.Group.Description equals dd.Group.Description
                                                select dd).ToList() : new List<AgentData>()
                                   let oData = a.AgentData != null ? (from ad in a.AgentData
                                                where !cData.Any(x => x.Group.Description.Equals(ad.Group.Description, StringComparison.InvariantCultureIgnoreCase))
                                                select ad).ToList() : new List<AgentData>()
                                   let nData = d.AgentData != null ? (from dd in d.AgentData
                                                where !cData.Any(x => x.Group.Description.Equals(dd.Group.Description, StringComparison.InvariantCultureIgnoreCase))
                                                select dd).ToList() : new List<AgentData>()
                                   where d.AgentData.Any()
                                   select new
                                   {
                                       Ssn = a.Ssn,
                                       AgentData = nData.Union(cData).Union(oData).OrderBy(x => x.Group.Description).ToList()
                                   }).ToList();
            
            var agents = existingAgents.Select(x => x.Ssn).ToList();
            
            dest.ForEach(agent =>
            {
                if(agents.Contains(agent.Ssn))
                {
                    agent.ModifiedBy = "UDW Import";
                    agent.ModifiedDate = DateTime.UtcNow;
                    var agentCategory = agentCategories.FirstOrDefault(x => x.Ssn == agent.Ssn);
                    if(agentCategory != null)
                    {
                        agent.AgentData = agentCategory.AgentData;
                    }
                }
            });

        }

        /// <summary>
        /// A method to check if there are existing schedules for agents
        /// </summary>
        /// <param name="agentSchedules"></param>
        /// <param name="agentSchedulingGroups"></param>
        /// <param name="timezones"></param>
        /// <returns></returns>
        private async Task CheckExistingSchedules(List<AgentSchedule> agentSchedules, List<AgentSchedulingGroup> agentSchedulingGroups, List<Timezone> timezones)
        {
            List<AgentSchedule> existingSchedules = await _agentScheduleRepository.GetSchedules(agentSchedules.Select(x => x.EmployeeId).ToList());
            agentSchedules.ForEach(x =>
            {
                var exSch = existingSchedules.FirstOrDefault(y => y.EmployeeId == x.EmployeeId);
                if (exSch != null) 
                {
                    var date = DateTime.UtcNow;
                    if (exSch.FirstName != x.FirstName || exSch.LastName != x.LastName)
                    {
                        x.ModifiedBy = "UDW Import";
                        x.ModifiedDate = date;
                    }
                    else
                    {
                        x.ModifiedBy = null;
                        x.ModifiedDate = null;
                    }

                    if (x.ActiveAgentSchedulingGroupId != 0 && exSch.ActiveAgentSchedulingGroupId != x.ActiveAgentSchedulingGroupId)
                    {
                        var timezoneId = agentSchedulingGroups.First(y => y.AgentSchedulingGroupId == x.ActiveAgentSchedulingGroupId).TimezoneId;
                        var offset = timezones.First(y => y.TimezoneId == timezoneId).UtcOffset;
                        var changeDate = date.Add(offset).Date;
                        var moveDate = new DateTime(changeDate.Year, changeDate.Month, changeDate.Day, 0, 0, 0, DateTimeKind.Utc);
                        x.Ranges = ScheduleHelper.GenerateAgentScheduleRanges(moveDate, x.ActiveAgentSchedulingGroupId, exSch.Ranges);
                        x.ModifiedBy = "UDW Import";
                        x.ModifiedDate = date;
                        
                        _agentScheduleManagerRepository.UpdateAgentSchedulingGroupForManagerCharts(new UpdatedAgentSchedulingGroupDetails
                                                                                                    {
                                                                                                        CurrentAgentSchedulingGroupId = x.ActiveAgentSchedulingGroupId,
                                                                                                        EmployeeId = x.EmployeeId,
                                                                                                        StartDate = moveDate
                                                                                                    });
                    }
                    else
                    {
                        x.Ranges = exSch.Ranges;
                    }
                } 
                else
                {
                    x.ModifiedBy = null;
                    x.ModifiedDate = null;
                }
            });
        }

        /// <summary>
        /// The helper method to reconcile the start date based on corresponding ASG timezones
        /// </summary>
        /// <param name="agentSchedulingGroupHistories"></param>
        /// <param name="agentSchedulingGroups"></param>
        /// <param name="timezones"></param>
        private void ReconcileStartDates(List<AgentSchedulingGroupHistory> agentSchedulingGroupHistories, List<AgentSchedulingGroup> agentSchedulingGroups, List<Timezone> timezones)
        {
            List<int> removeIds = new List<int>();
            
            agentSchedulingGroupHistories.ForEach(agentSchedulingGroupHistory =>
            {
                var agentSchedulingGroup = agentSchedulingGroups.FirstOrDefault(x => x.AgentSchedulingGroupId == agentSchedulingGroupHistory.AgentSchedulingGroupId);
                if(agentSchedulingGroup != null)
                {
                    var timezone = timezones.First(x => x.TimezoneId == agentSchedulingGroup.TimezoneId);
                    var changeDate = agentSchedulingGroupHistory.StartDate.Add(timezone.UtcOffset).Date;
                    agentSchedulingGroupHistory.StartDate = new DateTime(changeDate.Year, changeDate.Month, changeDate.Day, 0, 0, 0, DateTimeKind.Utc);
                }
                else
                {
                    removeIds.Add(agentSchedulingGroupHistory.AgentSchedulingGroupId);
                }
            });

            if (removeIds.Any())
            {
                agentSchedulingGroupHistories.RemoveAll(x => removeIds.Contains(x.AgentSchedulingGroupId));
            }
        }
        #endregion
    }
}
