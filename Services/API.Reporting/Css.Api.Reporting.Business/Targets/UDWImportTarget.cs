using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository;
using Css.Api.Reporting.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

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
        /// The name of the target which is used to map in the mapper json
        /// </summary>
        public string Name => "UDWImpTar";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="agentRepository"></param>
        /// <param name="agentSchedulingGroupRepository"></param>
        /// <param name="agentSchedulingGroupHistoryRepository"></param>
        /// <param name="agentScheduleRepository"></param>
        /// <param name="agentScheduleManagerRepository"></param>
        /// <param name="timezoneRepository"></param>
        /// <param name="activityLogRepository"></param>
        /// <param name="uow"></param>
        /// <param name="ftp"></param>
        public UDWImportTarget(IMapper mapper, IConfiguration configuration, IAgentRepository agentRepository, IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IAgentSchedulingGroupHistoryRepository agentSchedulingGroupHistoryRepository, IAgentScheduleRepository agentScheduleRepository, 
            IAgentScheduleManagerRepository agentScheduleManagerRepository, ITimezoneRepository timezoneRepository,
            IActivityLogRepository activityLogRepository, IUnitOfWork uow, IFTPService ftp) : base(ftp)
        {
            _mapper = mapper;
            _configuration = configuration;
            _agentRepository = agentRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _agentSchedulingGroupHistoryRepository = agentSchedulingGroupHistoryRepository;
            _timezoneRepository = timezoneRepository;
            _activityLogRepository = activityLogRepository;
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
                agents = metadata.Agents;
                var activityLogs = metadata.ActivityLogs;

                if(agents.Any())
                {
                    _agentRepository.Upsert(agents);
                    
                    var agentSchedulingGroupHistories = _mapper.Map<List<AgentSchedulingGroupHistory>>(agents).ToList();
                    var agentSchedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsByIds(agentSchedulingGroupHistories.Select(x => x.AgentSchedulingGroupId).ToList());
                    var timezones = await _timezoneRepository.GetTimezones(agentSchedulingGroups.Select(x => x.TimezoneId).ToList());

                    var agentSchedules = _mapper.Map<List<AgentSchedule>>(agents).ToList();
                    await CheckExistingSchedules(agentSchedules, agentSchedulingGroups, timezones, activityLogs);
                    _agentScheduleRepository.InsertAgentSchedules(agentSchedules);

                    ReconcileStartDates(agentSchedulingGroupHistories, agentSchedulingGroups, timezones);
                    _agentSchedulingGroupHistoryRepository.UpdateAgentSchedulingGroupHistory(agentSchedulingGroupHistories);

                    _activityLogRepository.CreateActivityLogs(activityLogs);
                    await _uow.Commit();
                }

                if (!string.IsNullOrWhiteSpace(metadata.Metadata))
                {
                    return new ActivityDataResponse()
                    {
                        Status = (int)ProcessStatus.Partial,
                        Metadata = metadata.Metadata
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
        /// <returns>Returns an instance of AgentMetadata. The property 'Metadata' will be an empty string if it matches, else the serialized array of all the partially imported sources data.</returns>
        private async Task<AgentMetadata> CheckImport(UDWAgentList source, List<Agent> dest)
        {
            var existingAgents = await _agentRepository.GetAgents(dest.Select(x => x.Ssn).ToList());
            var defaultMU = _configuration["DefaultMU"];
            dest = dest.Select(x => 
                        { 
                            if(!existingAgents.Select(y => y.Ssn).Contains(x.Ssn) && string.IsNullOrWhiteSpace(x.Mu))
                            {
                                source.NewAgents.Where(z => z.SSN == x.Ssn)
                                                .ToList()
                                                .ForEach(w =>
                                                {
                                                    w.MUString = defaultMU;
                                                });

                                source.ChangedAgents.Where(z => z.SSN == x.Ssn)
                                                    .ToList()
                                                    .ForEach(w =>
                                                    {
                                                        w.MUString = defaultMU;
                                                    });

                                x.Mu = defaultMU;
                            }
                            return x; 
                        })
                       .ToList();
            
            AssignAgentCategories(existingAgents, dest);
            
            var metadata = string.Empty;
            
            var schedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroups();
            var refIds = schedulingGroups.Select(x => x.RefId).ToList();

            var newAgents = source.NewAgents.Where(x => string.IsNullOrEmpty(x.SSN))
                            .Union(
                                (from ag in source.NewAgents.Where(x => !string.IsNullOrEmpty(x.SSN))
                                    join up in dest on ag.SSN equals up.Ssn
                                    where (ag.SenDate != null && up.SenDate == null)
                                    || (ag.MU == null) 
                                    || (ag.SenExt != null && up.SenExt == null)
                                    select ag)
                            )
                            .ToList();
            
            var invalidSchedulingGroupNewAgents = source.NewAgents.Where(x => !refIds.Contains(x.MU)).ToList();
            var invalidNewInsertsFromNewAgents = source.NewAgents
                                                    .Where(x => !string.IsNullOrEmpty(x.SSN)
                                                        && !existingAgents.Select(y => y.Ssn).ToList().Contains(x.SSN)
                                                        && (string.IsNullOrWhiteSpace(x.MUString)
                                                            || string.IsNullOrWhiteSpace(x.SSO)
                                                            || (string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) && string.IsNullOrWhiteSpace(x.Name))
                                                           ))
                                                    .ToList();

            newAgents = newAgents.Union(invalidSchedulingGroupNewAgents).Union(invalidNewInsertsFromNewAgents).Distinct().ToList();

            var changeAgents = source.ChangedAgents.Where(x => string.IsNullOrEmpty(x.SSN))
                                .Union(
                                    (from ag in source.ChangedAgents.Where(x => !string.IsNullOrEmpty(x.SSN))
                                    join up in dest on ag.SSN equals up.Ssn
                                    where (ag.SenDate != null && up.SenDate == null)
                                    || (ag.SenExt != null && up.SenExt == null)
                                    select ag)
                                )
                                .ToList();
            
            var invalidSchedulingGroupChangeAgents = source.ChangedAgents.Where(x => x.MU.HasValue && !refIds.Contains(x.MU)).ToList();
            var invalidNewInsertsFromChangeAgents = source.ChangedAgents
                                                    .Where(x => !string.IsNullOrEmpty(x.SSN)
                                                        && !existingAgents.Select(y => y.Ssn).ToList().Contains(x.SSN)
                                                        && (string.IsNullOrWhiteSpace(x.MUString)
                                                            || string.IsNullOrWhiteSpace(x.SSO)
                                                            || (string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) && string.IsNullOrWhiteSpace(x.Name))
                                                           ))
                                                    .ToList();


            changeAgents = changeAgents.Union(invalidSchedulingGroupChangeAgents).Union(invalidNewInsertsFromChangeAgents).Distinct().ToList();

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

            AgentMetadata agentMetadata = new AgentMetadata()
            {
                Agents = dest,
                ActivityLogs = GenerateActivityLogs(existingAgents, dest),
                Metadata = metadata
            };

            return agentMetadata;
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
                                       a.Ssn,
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
        /// A helper method to generate activity logs for agent collection changes
        /// </summary>
        /// <param name="currentData"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        private List<ActivityLog> GenerateActivityLogs(List<Agent> currentData, List<Agent> newData)
        {
            var currentTime = DateTime.UtcNow;
            List<ActivityLog> activityLogs = new List<ActivityLog>();
            newData.ForEach(agent =>
            {
                var existingData = currentData.FirstOrDefault(x => x.Ssn == agent.Ssn);
                if(existingData != null)
                {
                    var log = GenerateActivityLogForChangeAgents(existingData, agent);
                    if(log.FieldDetails.Any())
                    {
                        activityLogs.Add(log);
                    }
                }
                else
                {
                    activityLogs.Add(GenerateActivityLogForNewAgents(agent));
                }
            });

            return activityLogs;
        }

        /// <summary>
        /// A helper method to generate the activity log for a new agent
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private ActivityLog GenerateActivityLogForNewAgents(Agent agent)
        {
            ActivityLog activityLog = new ActivityLog()
            {
                ActivityOrigin = ActivityOrigin.UDW,
                ActivityStatus = ActivityStatus.Created,
                ActivityType = ActivityType.AgentAdmin,
                EmployeeId = agent.Ssn,
                ExecutedBy = ActivityOrigin.UDW.ToString(),
                FieldDetails = new List<FieldDetail>()
                        {
                            new FieldDetail()
                            {
                                Name = "EmployeeId",
                                OldValue = "",
                                NewValue = agent.Ssn.ToString()
                            },
                            new FieldDetail()
                            {
                                Name = "ClientId",
                                OldValue = "",
                                NewValue = agent.ClientId.ToString()
                            },
                            new FieldDetail()
                            {
                                Name = "ClientLobGroupId",
                                OldValue = "",
                                NewValue = agent.ClientLobGroupId.ToString()
                            },
                            new FieldDetail()
                            {
                                Name = "SkillGroupId",
                                OldValue = "",
                                NewValue = agent.SkillGroupId.ToString()
                            },
                            new FieldDetail()
                            {
                                Name = "SkillTagId",
                                OldValue = "",
                                NewValue = agent.SkillTagId.ToString()
                            }
                        }
            };

            var hireDateGroup = agent.AgentData.Select(x => x.Group).FirstOrDefault(x => x.Description.Equals("Hire Date"));

            if (hireDateGroup != null)
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Hire Date",
                    OldValue = "",
                    NewValue = hireDateGroup.Value.ToString()
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.Sso))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Sso",
                    OldValue = "",
                    NewValue = agent.Sso
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.FirstName))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "FirstName",
                    OldValue = "",
                    NewValue = agent.FirstName
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.LastName))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "LastName",
                    OldValue = "",
                    NewValue = agent.LastName
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.Mu))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Mu",
                    OldValue = "",
                    NewValue = agent.Mu
                });
            }

            return activityLog;
        }

        /// <summary>
        /// A helper method to generate activity log for a update agent 
        /// </summary>
        /// <param name="existingAgent"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        private ActivityLog GenerateActivityLogForChangeAgents(Agent existingAgent, Agent agent)
        {
            ActivityLog activityLog = new ActivityLog()
            {
                ActivityOrigin = ActivityOrigin.UDW,
                ActivityStatus = ActivityStatus.Updated,
                ActivityType = ActivityType.AgentAdmin,
                EmployeeId = agent.Ssn,
                ExecutedBy = ActivityOrigin.UDW.ToString(),
                FieldDetails = new List<FieldDetail>()          
            };

            if (!string.IsNullOrWhiteSpace(agent.Sso) && !(existingAgent.Sso ?? "").Equals(agent.Sso, StringComparison.InvariantCultureIgnoreCase))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Sso",
                    OldValue = existingAgent.Sso,
                    NewValue = agent.Sso
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.FirstName) && !(existingAgent.FirstName ?? "").Equals(agent.FirstName, StringComparison.InvariantCultureIgnoreCase))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "FirstName",
                    OldValue = existingAgent.FirstName,
                    NewValue = agent.FirstName
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.LastName) && !(existingAgent.LastName ?? "").Equals(agent.LastName, StringComparison.InvariantCultureIgnoreCase))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "LastName",
                    OldValue = existingAgent.LastName,
                    NewValue = agent.LastName
                });
            }

            if (!string.IsNullOrWhiteSpace(agent.Mu) && !(existingAgent.Mu ?? "").Equals(agent.Mu, StringComparison.InvariantCultureIgnoreCase))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Mu",
                    OldValue = existingAgent.Mu,
                    NewValue = agent.Mu
                });
            }

            if (agent.ClientId > 0 && existingAgent.ClientId != agent.ClientId)
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "ClientId",
                    OldValue = "",
                    NewValue = agent.ClientId.ToString()
                }); 
            }

            if (agent.ClientLobGroupId > 0 && existingAgent.ClientLobGroupId != agent.ClientLobGroupId)
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "ClientLobGroupId",
                    OldValue = "",
                    NewValue = agent.ClientLobGroupId.ToString()
                });
            }

            if (agent.SkillGroupId > 0 && existingAgent.SkillGroupId != agent.SkillGroupId)
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SkillGroupId",
                    OldValue = "",
                    NewValue = agent.SkillGroupId.ToString()
                });
            }

            if (agent.SkillTagId > 0 && existingAgent.SkillTagId != agent.SkillTagId)
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SkillTagId",
                    OldValue = "",
                    NewValue = agent.SkillTagId.ToString()
                });
            }
            
            var hireDateGroup = agent.AgentData.Select(x => x.Group).FirstOrDefault(x => x.Description.Equals("Hire Date"));

            if (hireDateGroup != null)
            {
                var existingHireGroup = existingAgent.AgentData.Select(x => x.Group).FirstOrDefault(x => x.Description.Equals("Hire Date"));
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Hire Date",
                    OldValue = existingHireGroup?.Value.ToString(),
                    NewValue = hireDateGroup.Value.ToString()
                });
            }

            return activityLog;
        }

        /// <summary>
        /// A method to check if there are existing schedules for agents
        /// </summary>
        /// <param name="agentSchedules"></param>
        /// <param name="agentSchedulingGroups"></param>
        /// <param name="timezones"></param>
        /// <param name="activityLogs"></param>
        /// <returns></returns>
        private async Task CheckExistingSchedules(List<AgentSchedule> agentSchedules, List<AgentSchedulingGroup> agentSchedulingGroups, List<Timezone> timezones, List<ActivityLog> activityLogs)
        {
            List<AgentSchedule> existingSchedules = await _agentScheduleRepository.GetSchedules(agentSchedules.Select(x => x.EmployeeId).ToList());
            agentSchedules.ForEach(async x =>
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
                        await ProcessMoveAgent(x, exSch, date, agentSchedulingGroups, timezones, activityLogs);
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
        /// A helper method to process the move agent scenarios
        /// </summary>
        /// <param name="agentSchedule"></param>
        /// <param name="existingSchedule"></param>
        /// <param name="date"></param>
        /// <param name="agentSchedulingGroups"></param>
        /// <param name="timezones"></param>
        /// <param name="activityLogs"></param>
        /// <returns></returns>
        private async Task ProcessMoveAgent(AgentSchedule agentSchedule, AgentSchedule existingSchedule, DateTime date, List<AgentSchedulingGroup> agentSchedulingGroups, List<Timezone> timezones, List<ActivityLog> activityLogs)
        {
            var timezoneId = agentSchedulingGroups.First(y => y.AgentSchedulingGroupId == agentSchedule.ActiveAgentSchedulingGroupId).TimezoneId;
            var offset = TimezoneHelper.GetOffset(timezones.First(y => y.TimezoneId == timezoneId).Abbreviation).Value;
            var changeDate = date.Add(offset).Date;
            var moveDate = new DateTime(changeDate.Year, changeDate.Month, changeDate.Day, 0, 0, 0, DateTimeKind.Utc);
            agentSchedule.Ranges = ScheduleHelper.GenerateAgentScheduleRanges(moveDate, agentSchedule.ActiveAgentSchedulingGroupId, existingSchedule.Ranges);
            agentSchedule.ModifiedBy = "UDW Import";
            agentSchedule.ModifiedDate = date;

            _agentScheduleManagerRepository.UpdateAgentSchedulingGroupForManagerCharts(new UpdatedAgentSchedulingGroupDetails
            {
                CurrentAgentSchedulingGroupId = agentSchedule.ActiveAgentSchedulingGroupId,
                EmployeeId = agentSchedule.EmployeeId,
                StartDate = moveDate
            });

            //Activity logs for range changes
            var newRanges = agentSchedule.Ranges.Where(y => y.DateFrom >= moveDate || (y.DateFrom <= moveDate && y.DateTo >= moveDate)).ToList();
            var currentTime = DateTime.UtcNow;
            newRanges.ForEach(range =>
            {
                activityLogs.Add(new ActivityLog
                {
                    ActivityOrigin = ActivityOrigin.UDW,
                    ActivityStatus = ActivityStatus.Updated,
                    ActivityType = ActivityType.SchedulingGrid,
                    EmployeeId = agentSchedule.EmployeeId,
                    ExecutedBy = ActivityOrigin.UDW.ToString(),
                    TimeStamp = currentTime,
                    SchedulingFieldDetails = new SchedulingFieldDetails
                    {
                        ActivityLogRange = new ActivityLogScheduleRange()
                        {
                            AgentSchedulingGroupId = range.AgentSchedulingGroupId,
                            DateFrom = range.DateFrom,
                            DateTo = range.DateTo,
                            ScheduleCharts = range.ScheduleCharts,
                            Status = range.Status
                        }
                    }
                });
            });

            //Activity logs for modified schedules
            var existingSchedules = await _agentScheduleManagerRepository.GetManagerSchedules(agentSchedule.EmployeeId, moveDate);
            existingSchedules.ForEach(sch =>
            {
                activityLogs.Add(new ActivityLog
                {
                    ActivityOrigin = ActivityOrigin.UDW,
                    ActivityStatus = ActivityStatus.Updated,
                    ActivityType = ActivityType.SchedulingGrid,
                    EmployeeId = agentSchedule.EmployeeId,
                    ExecutedBy = ActivityOrigin.UDW.ToString(),
                    TimeStamp = currentTime,
                    SchedulingFieldDetails = new SchedulingFieldDetails
                    {
                        ActivityLogManager = new ActivityLogScheduleManager()
                        {
                            AgentSchedulingGroupId = agentSchedule.ActiveAgentSchedulingGroupId,
                            Date = sch.Date,
                            Charts = sch.Charts
                        }
                    }
                });
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
                    var offset = TimezoneHelper.GetOffset(timezone.Abbreviation).Value;
                    var changeDate = agentSchedulingGroupHistory.StartDate.Add(offset).Date;
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
