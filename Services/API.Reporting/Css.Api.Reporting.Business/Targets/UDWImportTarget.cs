using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Data;
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
using System.Text.RegularExpressions;
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
        /// The batch service
        /// </summary>
        private readonly IBatchService _batchService;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfigurationService _configuration;

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
        /// The agent category repository
        /// </summary>
        private readonly IAgentCategoryRepository _agentCategoryRepository;

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
        /// <param name="batchService"></param>
        /// <param name="configuration"></param>
        /// <param name="agentRepository"></param>
        /// <param name="agentSchedulingGroupRepository"></param>
        /// <param name="agentSchedulingGroupHistoryRepository"></param>
        /// <param name="agentScheduleRepository"></param>
        /// <param name="agentScheduleManagerRepository"></param>
        /// <param name="agentCategoryRepository"></param>
        /// <param name="timezoneRepository"></param>
        /// <param name="activityLogRepository"></param>
        /// <param name="uow"></param>
        /// <param name="ftp"></param>
        public UDWImportTarget(IMapper mapper, IBatchService batchService, IConfigurationService configuration, IAgentRepository agentRepository, IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IAgentSchedulingGroupHistoryRepository agentSchedulingGroupHistoryRepository, IAgentScheduleRepository agentScheduleRepository, 
            IAgentScheduleManagerRepository agentScheduleManagerRepository, IAgentCategoryRepository agentCategoryRepository,
            ITimezoneRepository timezoneRepository, IActivityLogRepository activityLogRepository, IUnitOfWork uow, IFTPService ftp) : base(ftp)
        {
            _mapper = mapper;
            _batchService = batchService;
            _configuration = configuration;
            _agentRepository = agentRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _agentSchedulingGroupHistoryRepository = agentSchedulingGroupHistoryRepository;
            _agentCategoryRepository = agentCategoryRepository;
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

                var batches = _batchService.GenerateBatches(agents);
                UDWAgentList unprocessedRecords = new UDWAgentList()
                {
                    NewAgents = new List<UDWAgent>(),
                    ChangedAgents = new List<UDWAgentUpdate>()
                };

                foreach (var batch in batches)
                {
                    var batchSsns = batch.Items.Select(x => x.Ssn).ToList();
                    UDWAgentList batchData = new UDWAgentList()
                    {
                        NewAgents = root.NewAgents.Where(x => batchSsns.Contains(x.SSN)).ToList(),
                        ChangedAgents = root.ChangedAgents.Where(x => batchSsns.Contains(x.SSN)).ToList()
                    };

                    try
                    {
                        var metadata = await ProcessBatch(batchData, batch.Items);
                        if (metadata.Unprocessed.NewAgents != null && metadata.Unprocessed.NewAgents.Any())
                        {
                            unprocessedRecords.NewAgents.AddRange(metadata.Unprocessed.NewAgents);
                        }
                        if (metadata.Unprocessed.ChangedAgents != null && metadata.Unprocessed.ChangedAgents.Any())
                        {
                            unprocessedRecords.ChangedAgents.AddRange(metadata.Unprocessed.ChangedAgents);
                        }
                    }
                    catch(Exception ex)
                    {
                        string message = string.Format(Messages.BatchProcessingError, batch.BatchId, ex.Message);
                        batchData.NewAgents.ForEach(x =>
                        {
                            x.Messages.Add(message);
                        });
                        batchData.ChangedAgents.ForEach(x =>
                        {
                            x.Messages.Add(message);
                        });
                        unprocessedRecords.NewAgents.AddRange(batchData.NewAgents);
                        unprocessedRecords.ChangedAgents.AddRange(batchData.ChangedAgents);
                    }
                }

                if (unprocessedRecords.NewAgents.Any() || unprocessedRecords.ChangedAgents.Any())
                {
                    XMLParser<UDWAgentList> xmlParser = new XMLParser<UDWAgentList>();
                    return new ActivityDataResponse()
                    {
                        Status = (int)ProcessStatus.Partial,
                        Metadata = xmlParser.Serialize(unprocessedRecords)
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
        /// The method to process the batch 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="agents"></param>
        /// <returns>An instance of AgentMetadata</returns>
        private async Task<AgentMetadata> ProcessBatch(UDWAgentList root, List<Agent> agents)
        {
            var metadata = await CheckImport(root, agents);
            agents = metadata.Agents;

            if (agents.Any())
            {
                var activityLogs = metadata.ActivityLogs;
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
            return metadata;
        }

        /// <summary>
        /// The method to check any mismatches in the source and destination data
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns>Returns an instance of AgentMetadata. The property 'Metadata' will be an empty string if it matches, else the serialized array of all the partially imported sources data.</returns>
        private async Task<AgentMetadata> CheckImport(UDWAgentList source, List<Agent> dest)
        {
            var metadata = string.Empty;
            var existingAgents = await _agentRepository.GetAgents(dest.Select(x => x.Ssn).ToList());
            
            var agentCategoryMismatch = await CheckAgentCategories(source, existingAgents, dest);

            var newAgents = agentCategoryMismatch.NewAgents;
            var changeAgents = agentCategoryMismatch.ChangedAgents;
            source.NewAgents.RemoveAll(x => newAgents.Select(y => y.SSN).Contains(x.SSN));
            source.ChangedAgents.RemoveAll(x => changeAgents.Select(y => y.SSN).Contains(x.SSN));
            
            var ignoreAgents = newAgents.Select(x => x.SSN).Union(changeAgents.Select(x => x.SSN)).ToList();
            dest.RemoveAll(x => ignoreAgents.Contains(x.Ssn));

            if(!dest.Any())
            {
                XMLParser<UDWAgentList> parser = new XMLParser<UDWAgentList>();
                metadata = parser.Serialize(agentCategoryMismatch);

                return new AgentMetadata()
                {
                    Agents = dest,
                    Unprocessed = agentCategoryMismatch
                };
            }

            AssignAgentCategories(existingAgents, dest);
            AssignMUs(source, existingAgents, dest);

            var schedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroups();

            newAgents = newAgents.Union(CheckNewAgents(source, existingAgents, dest, schedulingGroups)).Distinct().ToList();
            changeAgents = changeAgents.Union(CheckChangeAgent(source, existingAgents, dest, schedulingGroups)).Distinct().ToList();
            
            ignoreAgents = newAgents.Select(x => x.SSN).Union(changeAgents.Select(x => x.SSN)).ToList();
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

            AgentMetadata agentMetadata = new AgentMetadata()
            {
                Agents = dest,
                ActivityLogs = await GenerateActivityLogs(existingAgents, dest),
                Unprocessed = mismatch
            };

            return agentMetadata;
        }

        /// <summary>
        /// A helper to check the agent categories received from UDW dump
        /// </summary>
        /// <param name="source"></param>
        /// <param name="existingAgents"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public async Task<UDWAgentList> CheckAgentCategories(UDWAgentList source, List<Agent> existingAgents, List<Agent> dest)
        {
            UDWAgentList mismatch = new UDWAgentList()
            {
                NewAgents = new List<UDWAgent>(),
                ChangedAgents = new List<UDWAgentUpdate>()
            };

            var agentCategories = await _agentCategoryRepository.GetAgentCategories();
            dest.ForEach(agent =>
            {
                AssignSSO(agent);
                AssignSupervisor(agent, existingAgents);
                AssignHireDate(agent, source);
                
                if (agent.AgentData.Any())
                {
                    List<string> messages = new List<string>();
                    var invalidCategories = agent.AgentData.Where(x => !agentCategories.Select(y => y.Name.Trim().ToUpper()).Contains(x.Group.Description.Trim().ToUpper())).ToList();
                    
                    if (invalidCategories.Any())
                    {
                        var categories = string.Join(", ", invalidCategories.Select(x => x.Group.Description.Trim()).ToList());
                        messages.Add(string.Format(Messages.InvalidAgentCategories, categories));
                    }
                    else
                    {
                        List<string> errors = new List<string>();
                        List<AgentCategoryValue> categoryValues = AssignAgentCategoryValues(agent, agentCategories, out errors);
                        messages.AddRange(errors);

                        if (!messages.Any())
                        {
                            agent.AgentCategoryValues = categoryValues;
                        }
                    }

                    if (messages.Any())
                    {
                        var newAgentMismatch = source.NewAgents.Where(x => x.SSN.Equals(agent.Ssn)).ToList();
                        var changeAgentMismatch = source.ChangedAgents.Where(x => x.SSN.Equals(agent.Ssn)).ToList();

                        newAgentMismatch = newAgentMismatch.Select(x =>
                        {
                            x.Messages = messages;
                            return x;
                        }).ToList();

                        changeAgentMismatch = changeAgentMismatch.Select(x =>
                        {
                            x.Messages = messages;
                            return x;
                        }).ToList();

                        mismatch.NewAgents.AddRange(newAgentMismatch);
                        mismatch.ChangedAgents.AddRange(changeAgentMismatch);
                    }  
                }
            });

            return mismatch;
        }

        /// <summary>
        /// The method to set and merge any existing agent categories with new ones
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        private void AssignAgentCategories(List<Agent> existingAgents, List<Agent> dest)
        {
            var currDate = DateTime.UtcNow;

            var agentCategories = (from a in existingAgents
                                   join d in dest on a.Ssn equals d.Ssn
                                   let cData = (from ad in a.AgentCategoryValues
                                                join dd in d.AgentCategoryValues on ad.CategoryId equals dd.CategoryId
                                                select new AgentCategoryValue { CategoryId = dd.CategoryId, CategoryValue = dd.CategoryValue, StartDate = ad.StartDate }).ToList()
                                   let oData = (from ad in a.AgentCategoryValues
                                                where !cData.Any(x => x.CategoryId == ad.CategoryId)
                                                select ad).ToList()
                                   let nData = (from dd in d.AgentCategoryValues
                                                where !cData.Any(x => x.CategoryId == dd.CategoryId)
                                                select new AgentCategoryValue { CategoryId = dd.CategoryId, CategoryValue = dd.CategoryValue, StartDate = currDate }).ToList()
                                   where d.AgentCategoryValues.Any()
                                   select new
                                   {
                                       a.Ssn,
                                       AgentCategoryValues = nData.Union(cData).Union(oData).OrderBy(x => x.CategoryId).ToList()
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
                        agent.AgentCategoryValues = agentCategory.AgentCategoryValues;
                    }
                }
                else
                {
                    agent.AgentCategoryValues = agent.AgentCategoryValues.Select(x => { x.StartDate = currDate; return x; }).ToList();
                }
            });

        }

        /// <summary>
        /// A helper method to assign agent category values to agent 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="agentCategories"></param>
        /// <param name="errors"></param>
        /// <returns>List of instances of AgentCategoryValue</returns>
        private List<AgentCategoryValue> AssignAgentCategoryValues(Agent agent, List<AgentCategory> agentCategories, out List<string> errors)
        {
            List<AgentCategoryValue> categoryValues = new List<AgentCategoryValue>();
            List<string> messages = new List<string>();
            agent.AgentData.ForEach(data =>
            {
                if (!string.IsNullOrWhiteSpace(data.Group.Value))
                {
                    var category = agentCategories.First(x => x.Name.Trim().Equals(data.Group.Description.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    List<string> errorMessages = new List<string>();
                    var agentCategoryValue = CheckAgentCategoryValue(data.Group.Value, category, out errorMessages);

                    if (errorMessages.Any())
                    {
                        messages.AddRange(errorMessages);
                    }
                    else
                    {
                        categoryValues.Add(agentCategoryValue);
                    }
                }
            });

            errors = messages;
            return categoryValues;
        }

        /// <summary>
        /// A helper method to assign SSO
        /// </summary>
        /// <param name="agent"></param>
        private void AssignSSO(Agent agent)
        {
            var ssoGroup = agent.AgentData.FirstOrDefault(x => x.Group.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Sso, StringComparison.InvariantCultureIgnoreCase));
            agent.Sso = ssoGroup?.Group.Value;
        }

        /// <summary>
        /// A helper method to assign supervisor details
        /// </summary>
        /// <param name="agent"></param>
        private void AssignSupervisor(Agent agent, List<Agent> existingAgents)
        {
            var supervisorGroup = agent.AgentData.FirstOrDefault(x => x.Group.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Supervisor, StringComparison.InvariantCultureIgnoreCase));
            if(supervisorGroup != null)
            {
                agent.SupervisorSso = string.Empty;
                agent.SupervisorId = string.Empty;
                agent.SupervisorName = GetSupervisorName(supervisorGroup.Group.Value.Trim());

                var existingRec = existingAgents.FirstOrDefault(x => x.Ssn.Equals(agent.Ssn));
                if(existingRec != null && agent.SupervisorName.Equals(existingRec.SupervisorName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    agent.SupervisorSso = existingRec.SupervisorSso;
                    agent.SupervisorId = existingRec.SupervisorId;
                }
            }
        }

        /// <summary>
        /// A helper method to assign hire date
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="source"></param>
        private void AssignHireDate(Agent agent, UDWAgentList source)
        {
            var hireData = agent.AgentData.FirstOrDefault(x => x.Group.Description.Equals(_configuration.Settings.AgentCategoryFields.Hire));
            if (agent.HireDate.HasValue)
            {
                if (hireData == null)
                {
                    (agent.AgentData ?? (agent.AgentData = new List<AgentData>())).Add(new AgentData
                    {
                        Group = new AgentGroup
                        {
                            Description = _configuration.Settings.AgentCategoryFields.Hire,
                            Value = agent.HireDate.Value.ToString("yyyyMMdd")
                        }
                    });
                }
                else
                {
                    hireData.Group.Value = agent.HireDate.Value.ToString("yyyyMMdd");
                }
            }
            else if(hireData != null)
            {
                DateTime hireDate;
                var status = DateTime.TryParseExact(hireData.Group.Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out hireDate);
                if(status)
                {
                    agent.HireDate = new DateTime(hireDate.Year, hireDate.Month, hireDate.Day, 0, 0, 0, DateTimeKind.Utc);
                    var newAgent = source.NewAgents.FirstOrDefault(x => x.SSN == agent.Ssn);
                    var changedAgent = source.ChangedAgents.FirstOrDefault(x => x.SSN == agent.Ssn);
                    if(newAgent != null)
                    {
                        newAgent.SenDate = new UDWAgentDate() { Day = agent.HireDate.Value.Day, Month = agent.HireDate.Value.Month, Year = agent.HireDate.Value.Year };
                    }
                    else if(changedAgent != null)
                    {
                        changedAgent.SenDate = new UDWAgentDate() { Day = agent.HireDate.Value.Day, Month = agent.HireDate.Value.Month, Year = agent.HireDate.Value.Year };
                    }
                }
            }
        }

        /// <summary>
        /// A helper method to assign MUs
        /// </summary>
        /// <param name="source"></param>
        /// <param name="existingAgents"></param>
        /// <param name="dest"></param>
        private void AssignMUs(UDWAgentList source, List<Agent> existingAgents, List<Agent> dest)
        {
            dest = dest.Select(agent =>
                        {
                            var defaultMu = string.Empty;

                            var empTerm = agent.AgentData.FirstOrDefault(y => y.Group.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Term, StringComparison.InvariantCultureIgnoreCase));
                            if (empTerm != null && !string.IsNullOrWhiteSpace(empTerm.Group.Value))
                            {
                                defaultMu = _configuration.Settings.MUs.Termination;
                            }
                            else if (!existingAgents.Select(y => y.Ssn).Contains(agent.Ssn) && string.IsNullOrWhiteSpace(agent.Mu))
                            {
                                defaultMu = _configuration.Settings.MUs.Default;
                            }

                            if (!string.IsNullOrWhiteSpace(defaultMu))
                            {
                                source.NewAgents.Where(z => z.SSN == agent.Ssn)
                                                .ToList()
                                                .ForEach(w =>
                                                {
                                                    w.MUString = defaultMu;
                                                });

                                source.ChangedAgents.Where(z => z.SSN == agent.Ssn)
                                                    .ToList()
                                                    .ForEach(w =>
                                                    {
                                                        w.MUString = defaultMu;
                                                    });

                                agent.Mu = defaultMu;
                            }

                            return agent;
                        })
                       .ToList();
        }

        /// <summary>
        /// A helper method to validate the SSO
        /// </summary>
        /// <param name="agent">The instance of UDWAgent</param>
        /// <returns>True if valid</returns>
        private bool ValidateSSO(UDWAgent agent)
        {
            var ssoField = agent.AgentData.FirstOrDefault(x => x.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Sso, StringComparison.InvariantCultureIgnoreCase));
            if (ssoField == null)
                return false;
            return ValidateSSO(ssoField.Value);
        }

        /// <summary>
        /// A helper method to validate the SSO
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private bool ValidateSSO(UDWAgentUpdate agent)
        {
            var ssoField = agent.AgentData.FirstOrDefault(x => x.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Sso, StringComparison.InvariantCultureIgnoreCase));
            if (ssoField == null)
                return false;
            return ValidateSSO(ssoField.Value);
        }

        /// <summary>
        /// A helper method to validate the SSO
        /// </summary>
        /// <param name="value">The input string</param>
        /// <returns>True if valid</returns>
        private bool ValidateSSO(string value)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// A helper method to get the supervisor name
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        private string GetSupervisorName(string fullname)
        {
            var splitName = fullname.Split(',');
            if (splitName.Length > 1)
            {
                return string.Join(" ", splitName[1].Trim(), splitName[0].Trim());
            }
            return fullname;
        }

        /// <summary>
        /// A helper method to check new agents from UDW dump
        /// </summary>
        /// <param name="source"></param>
        /// <param name="existingAgents"></param>
        /// <param name="dest"></param>
        /// <param name="schedulingGroups"></param>
        /// <returns></returns>
        private List<UDWAgent> CheckNewAgents(UDWAgentList source, List<Agent> existingAgents, List<Agent> dest, List<AgentSchedulingGroup> schedulingGroups)
        {
            var refIds = schedulingGroups.Select(x => x.RefId).ToList();

            var newAgents = source.NewAgents.Where(x => string.IsNullOrWhiteSpace(x.SSN) || x.SSN.Trim().Equals("0"))
                            .Union(
                                (from ag in source.NewAgents.Where(x => !string.IsNullOrWhiteSpace(x.SSN) && !x.SSN.Trim().Equals("0"))
                                    join up in dest on ag.SSN equals up.Ssn
                                    where (ag.SenDate != null && up.HireDate == null)
                                    || (ag.MU == null) 
                                    || (ag.SenExt != null && up.SenExt == null)
                                    select ag)
                            )
                            .ToList();
            
            var invalidSchedulingGroupNewAgents = source.NewAgents.Where(x => !refIds.Contains(x.MU)).ToList();
            var invalidNewInsertsFromNewAgents = source.NewAgents
                                                    .Where(x => !string.IsNullOrWhiteSpace(x.SSN)
                                                        && !existingAgents.Select(y => y.Ssn).ToList().Contains(x.SSN)
                                                        && (string.IsNullOrWhiteSpace(x.MUString)
                                                            || (x.SenDate == null || !x.AgentData.Any(x => x.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Hire, StringComparison.InvariantCultureIgnoreCase)))
                                                            || !ValidateSSO(x)
                                                            || (string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) && string.IsNullOrWhiteSpace(x.Name))
                                                           ))
                                                    .ToList();

            newAgents = newAgents.Union(invalidSchedulingGroupNewAgents).Union(invalidNewInsertsFromNewAgents).Distinct().ToList();

            return newAgents.Select(x =>
            {
                x.Messages = new List<string>();
                if (string.IsNullOrWhiteSpace(x.SSN) || x.SSN.Trim().Equals("0"))
                {
                    x.Messages.Add(Messages.SsnMandatory);
                }
                if (x.SenDate != null)
                {
                    DateTime dt;
                    if (x.SenDate.Day == 0 && x.SenDate.Month == 0 && x.SenDate.Year == 0)
                    {
                        x.Messages.Add(Messages.InvalidSenDate);
                    }
                    else if(!DateTime.TryParse(String.Join('-', x.SenDate.Year, x.SenDate.Month, x.SenDate.Day), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dt))
                    {
                        x.Messages.Add(Messages.InvalidSenDate);
                    }
                }
                if (x.SenExt.HasValue && (x.SenExt.Value <= 0 || x.SenExt.Value >= 10000))
                {
                    x.Messages.Add(Messages.InvalidSenExt);
                }
                if(!refIds.Contains(x.MU))
                {
                    x.Messages.Add(string.Format(Messages.InvalidMU, x.MU));
                }
                if (!existingAgents.Select(y => y.Ssn).ToList().Contains(x.SSN))
                {
                    if(string.IsNullOrWhiteSpace(x.MUString))
                    {
                        x.Messages.Add(Messages.MuMandatory);
                    }
                    if(!ValidateSSO(x.AgentData.FirstOrDefault(y => y.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Sso, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty))
                    {
                        x.Messages.Add(Messages.SsoMandatory);
                    }
                    if ((string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) && string.IsNullOrWhiteSpace(x.Name)))
                    {
                        x.Messages.Add(Messages.NameMandatory);
                    }
                    
                    var hireData = x.AgentData.FirstOrDefault(x => x.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Hire, StringComparison.InvariantCultureIgnoreCase));
                    if (x.SenDate == null || hireData == null)
                    {
                        x.Messages.Add(Messages.HireDateMandatory);
                    }
                }
                return x;
            }).ToList();
        }

        /// <summary>
        /// A helper method to check changed agents from UDW dump
        /// </summary>
        /// <param name="source"></param>
        /// <param name="existingAgents"></param>
        /// <param name="dest"></param>
        /// <param name="schedulingGroups"></param>
        /// <returns></returns>
        private List<UDWAgentUpdate> CheckChangeAgent(UDWAgentList source, List<Agent> existingAgents, List<Agent> dest, List<AgentSchedulingGroup> schedulingGroups)
        {
            var refIds = schedulingGroups.Select(x => x.RefId).ToList();

            var changeAgents = source.ChangedAgents.Where(x => string.IsNullOrWhiteSpace(x.SSN) || x.SSN.Trim().Equals("0"))
                                .Union(
                                    (from ag in source.ChangedAgents.Where(x => !string.IsNullOrWhiteSpace(x.SSN) && !x.SSN.Trim().Equals("0"))
                                    join up in dest on ag.SSN equals up.Ssn
                                    where (ag.SenDate != null && up.HireDate == null)
                                    || (ag.SenExt != null && up.SenExt == null)
                                    select ag)
                                )
                                .ToList();
            
            var invalidSchedulingGroupChangeAgents = source.ChangedAgents.Where(x => x.MU.HasValue && !refIds.Contains(x.MU)).ToList();
            var invalidNewInsertsFromChangeAgents = source.ChangedAgents
                                                    .Where(x => !string.IsNullOrWhiteSpace(x.SSN)
                                                        && !existingAgents.Select(y => y.Ssn).ToList().Contains(x.SSN)
                                                        && (string.IsNullOrWhiteSpace(x.MUString)
                                                            || (x.SenDate == null || !x.AgentData.Any(x => x.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Hire, StringComparison.InvariantCultureIgnoreCase)))
                                                            || !ValidateSSO(x)
                                                            || (string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) && string.IsNullOrWhiteSpace(x.Name))
                                                           ))
                                                    .ToList();

            changeAgents = changeAgents.Union(invalidSchedulingGroupChangeAgents).Union(invalidNewInsertsFromChangeAgents).Distinct().ToList();

            return changeAgents.Select(x =>
            {
                x.Messages = new List<string>();
                if (string.IsNullOrWhiteSpace(x.SSN) || x.SSN.Trim().Equals("0"))
                {
                    x.Messages.Add(Messages.AgentIdMandatory);
                }
                if (x.SenDate != null)
                {
                    DateTime dt;
                    if (x.SenDate.Day == 0 && x.SenDate.Month == 0 && x.SenDate.Year == 0)
                    {
                        x.Messages.Add(Messages.InvalidSenDate);
                    }
                    else if (!DateTime.TryParse(String.Join('-', x.SenDate.Year, x.SenDate.Month, x.SenDate.Day), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dt))
                    {
                        x.Messages.Add(Messages.InvalidSenDate);
                    }
                }
                if (x.SenExt.HasValue && (x.SenExt.Value <= 0 || x.SenExt.Value >= 10000))
                {
                    x.Messages.Add(Messages.InvalidSenExt);
                }
                if (x.MU.HasValue && !refIds.Contains(x.MU))
                {
                    x.Messages.Add(string.Format(Messages.InvalidMU, x.MU));
                }
                if (!existingAgents.Select(y => y.Ssn).ToList().Contains(x.SSN))
                {
                    if (string.IsNullOrWhiteSpace(x.MUString))
                    {
                        x.Messages.Add(Messages.MuMandatory);
                    }
                    if (!ValidateSSO(x.AgentData.FirstOrDefault(y => y.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Sso, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty))
                    {
                        x.Messages.Add(Messages.SsoMandatory);
                    }
                    if ((string.IsNullOrWhiteSpace(x.FirstName) && string.IsNullOrWhiteSpace(x.LastName) && string.IsNullOrWhiteSpace(x.Name)))
                    {
                        x.Messages.Add(Messages.NameMandatory);
                    }
                    
                    var hireData = x.AgentData.FirstOrDefault(x => x.Description.Trim().Equals(_configuration.Settings.AgentCategoryFields.Hire, StringComparison.InvariantCultureIgnoreCase));
                    if(x.SenDate == null || hireData == null)
                    {
                        x.Messages.Add(Messages.HireDateMandatory);
                    }
                }
                return x;
            }).ToList();
        }

        /// <summary>
        /// A helper method to check the agent category value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="agentCategory"></param>
        /// <param name="errorMessages"></param>
        /// <returns>An instance of AgentCategoryValue</returns>
        private AgentCategoryValue CheckAgentCategoryValue(string value, AgentCategory agentCategory, out List<string> errorMessages)
        {
            AgentCategoryValue agentCategoryValue = null;
            List<string> messages = new List<string>();

            if (agentCategory.AgentCategoryType == AgentCategoryType.Date)
            {
                DateTime dt;
                var parseDate = DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                if (parseDate)
                {
                    agentCategoryValue = new AgentCategoryValue()
                    {
                        CategoryId = agentCategory.AgentCategoryId,
                        CategoryValue = dt.ToString("yyyy-MM-dd")
                    };
                }
                else
                {
                    messages.Add(string.Format(Messages.InvalidAgentCategoryDate, agentCategory.Name));
                }
            }
            else if(agentCategory.AgentCategoryType == AgentCategoryType.Numeric)
            {
                int numericValue, minValue, maxValue;
                var parseNumeric = int.TryParse(value, out numericValue);
                if(parseNumeric)
                {
                    var parseMaxValue = int.TryParse(agentCategory.DataTypeMaxValue, out maxValue);
                    var parseMinValue = int.TryParse(agentCategory.DataTypeMinValue, out minValue);

                    if(parseMaxValue && parseMinValue && numericValue >= minValue && numericValue <= maxValue)
                    {
                        agentCategoryValue = new AgentCategoryValue()
                        {
                            CategoryId = agentCategory.AgentCategoryId,
                            CategoryValue = numericValue.ToString()
                        };
                    }
                    else
                    {
                        messages.Add(string.Format(Messages.InvalidAgentCategoryNumeric, agentCategory.Name, agentCategory.DataTypeMinValue, agentCategory.DataTypeMaxValue));
                    }
                }
            }
            else
            {
                agentCategoryValue = new AgentCategoryValue()
                {
                    CategoryId = agentCategory.AgentCategoryId,
                    CategoryValue = value
                };
            }
            
            errorMessages = messages;
            return agentCategoryValue;
        }

        /// <summary>
        /// A helper method to generate activity logs for agent collection changes
        /// </summary>
        /// <param name="currentData"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        private async Task<List<ActivityLog>> GenerateActivityLogs(List<Agent> currentData, List<Agent> newData)
        {
            var currentTime = DateTime.UtcNow;
            List<AgentCategory> agentCategories = await _agentCategoryRepository.GetAgentCategories();
            List<ActivityLog> activityLogs = new List<ActivityLog>();
            newData.ForEach(agent =>
            {
                var existingData = currentData.FirstOrDefault(x => x.Ssn == agent.Ssn);
                if(existingData != null)
                {
                    var log = GenerateActivityLogForChangeAgents(existingData, agent, agentCategories);
                    if(log.FieldDetails.Any())
                    {
                        activityLogs.Add(log);
                    }
                }
                else
                {
                    activityLogs.Add(GenerateActivityLogForNewAgents(agent, agentCategories));
                }
            });

            return activityLogs.Select(x => { x.TimeStamp = DateTime.UtcNow; return x; }).ToList();
        }

        /// <summary>
        /// A helper method to generate the activity log for a new agent
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private ActivityLog GenerateActivityLogForNewAgents(Agent agent, List<AgentCategory> agentCategories)
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

            var hireDateCategory = agentCategories.FirstOrDefault(x => x.Name.Equals(_configuration.Settings.AgentCategoryFields.Hire));
            var hireDateGroup = agent.AgentCategoryValues.FirstOrDefault(x => x.CategoryId == hireDateCategory?.AgentCategoryId);

            if (hireDateGroup != null)
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "Hire Date",
                    OldValue = "",
                    NewValue = hireDateGroup.CategoryValue.ToString()
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

            if (!string.IsNullOrWhiteSpace(agent.SupervisorName))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SupervisorId",
                    OldValue = "",
                    NewValue = agent.SupervisorId
                });
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SupervisorName",
                    OldValue = "",
                    NewValue = agent.SupervisorName
                });
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SupervisorSso",
                    OldValue = "",
                    NewValue = agent.SupervisorSso
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
        private ActivityLog GenerateActivityLogForChangeAgents(Agent existingAgent, Agent agent, List<AgentCategory> agentCategories)
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

            if (!string.IsNullOrWhiteSpace(agent.SupervisorName) && !existingAgent.SupervisorName.Equals(agent.SupervisorName, StringComparison.InvariantCultureIgnoreCase))
            {
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SupervisorId",
                    OldValue = "",
                    NewValue = agent.SupervisorId
                });

                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SupervisorName",
                    OldValue = "",
                    NewValue = agent.SupervisorName
                });
                activityLog.FieldDetails.Add(new FieldDetail()
                {
                    Name = "SupervisorSso",
                    OldValue = "",
                    NewValue = agent.SupervisorSso
                });
            }

            var hireDateCategory = agentCategories.FirstOrDefault(x => x.Name.Equals(_configuration.Settings.AgentCategoryFields.Hire));
            var hireDateGroup = agent.AgentCategoryValues.FirstOrDefault(x => x.CategoryId == hireDateCategory?.AgentCategoryId);

            if (hireDateGroup != null)
            {
                var existingHireGroup = existingAgent.AgentCategoryValues.FirstOrDefault(x => x.CategoryId == hireDateCategory?.AgentCategoryId);

                if(hireDateGroup.CategoryValue != existingHireGroup?.CategoryValue)
                {
                    activityLog.FieldDetails.Add(new FieldDetail()
                    {
                        Name = "Hire Date",
                        OldValue = existingHireGroup?.CategoryValue.ToString(),
                        NewValue = hireDateGroup.CategoryValue.ToString()
                    });
                }
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
