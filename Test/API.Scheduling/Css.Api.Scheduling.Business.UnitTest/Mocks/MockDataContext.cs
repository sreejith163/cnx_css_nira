using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.Scheduling.Business.UnitTest.Mocks
{
    public class MockDataContext
    {
        #region Data

        private readonly IQueryable<Client> clientsDB = new List<Client>() 
        {
            new Client { ClientId = 1, Name = "Client Name 1", RefId = 1, IsDeleted = false },
            new Client { ClientId = 2, Name = "Client Name 2", RefId = 2, IsDeleted = false }
        }.AsQueryable();

        private readonly IQueryable<ClientLobGroup> clientLobGroupDB = new List<ClientLobGroup>() 
        {
            new ClientLobGroup { ClientId = 1, ClientLobGroupId = 1, Name = "Client Lob 1", RefId = 1, IsDeleted = false },
            new ClientLobGroup { ClientId = 2, ClientLobGroupId = 2, Name = "Client Lob 2", RefId = 1, IsDeleted = false }
        }.AsQueryable();

        private readonly IQueryable<SkillGroup> skillGroupDB = new List<SkillGroup>()
        {
            new SkillGroup { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, Name = "Skill Group 1", RefId = 1, IsDeleted = false },
            new SkillGroup { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, Name = "Skill Group 2", RefId = 1, IsDeleted = false }
        }.AsQueryable();

        private readonly IQueryable<SkillTag> skillTagDB = new List<SkillTag>()
        {
            new SkillTag { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, Name = "Skill Tag 1", RefId = 1, IsDeleted = false },
            new SkillTag { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, SkillTagId = 2, Name = "Skill Tag 2", RefId = 1, IsDeleted = false }
        }.AsQueryable();

        private readonly IQueryable<AgentSchedulingGroup> agentSchedulingGroupDB = new List<AgentSchedulingGroup>() 
        {
            new AgentSchedulingGroup { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, AgentSchedulingGroupId = 1,
                                       Name = "Agent Scheduliung Group 1", RefId = 1, IsDeleted = false },
            new AgentSchedulingGroup { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, SkillTagId = 2, AgentSchedulingGroupId = 2,
                                       Name = "Agent Scheduliung Group 2", RefId = 1, IsDeleted = false }
        }.AsQueryable();

        private readonly IQueryable<Agent> agentAdminsDB = new List<Agent>()
        {
            new Agent { Id = new ObjectId("5fe0b5ad6a05416894c0718d"), FirstName = "abc", LastName = "def", Ssn = 1,
                        Sso = "user1@concentrix.com", ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, AgentSchedulingGroupId = 1,
                        CreatedBy = "Admin", CreatedDate = DateTime.UtcNow },
            new Agent { Id = new ObjectId("5fe0b5c46a05416894c0718f"), FirstName = "lmn", LastName = "pqr", Ssn = 2,
                        Sso = "user2@concentrix.com", ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, AgentSchedulingGroupId = 1,
                        CreatedBy = "Admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        private readonly IQueryable<ActivityLog> activityLogsDB = new List<ActivityLog>()
        {
            new ActivityLog { Id = new ObjectId("5fe0b5ad6a05416894c0718d"), ActivityOrigin=ActivityOrigin.CSS, ActivityStatus=ActivityStatus.Created,
                ActivityType=ActivityType.AgentAdmin, ExecutedBy="admin", EmployeeId = 1 },
             new ActivityLog { Id = new ObjectId("5fe0b5c46a05416894c0718f"), ActivityOrigin=ActivityOrigin.CSS, ActivityStatus=ActivityStatus.Updated,
                ActivityType=ActivityType.AgentAdmin, ExecutedBy="admin", EmployeeId = 1},
        }.AsQueryable();

        private readonly IQueryable<AgentSchedule> agentSchedulesDB = new List<AgentSchedule>() {
            new AgentSchedule { Id = new ObjectId("5fe0b5ad6a05416894c0718e"), EmployeeId = 1, FirstName = "Test", LastName = "last",
                                ActiveAgentSchedulingGroupId = 1, CreatedBy = "Admin", CreatedDate = DateTime.UtcNow, IsDeleted = false,
                                Ranges = new List<AgentScheduleRange>
                                {
                                    new AgentScheduleRange {
                                        DateFrom = DateTime.UtcNow, 
                                        DateTo = DateTime.UtcNow,
                                        AgentSchedulingGroupId = 1,
                                        Status = SchedulingStatus.Released,
                                        ScheduleCharts = new List<AgentScheduleChart>
                                        {
                                            new AgentScheduleChart
                                            {
                                                Day = 0,
                                                Charts = new List<ScheduleChart>
                                                {
                                                    new ScheduleChart
                                                    {
                                                        StartTime = "00:00 am",
                                                        EndTime = "00:05pm",
                                                        SchedulingCodeId = 1
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
            },
            new AgentSchedule { Id = new ObjectId("5fe0b5ad6a05416894c0718f"), EmployeeId = 2, FirstName = "Acr", LastName = "test",
                                ActiveAgentSchedulingGroupId = 1, CreatedBy = "Admin", CreatedDate = DateTime.UtcNow, IsDeleted = false,
                                Ranges = new List<AgentScheduleRange>
                                {
                                    new AgentScheduleRange {
                                        DateFrom = DateTime.UtcNow,
                                        DateTo = DateTime.UtcNow,
                                        AgentSchedulingGroupId = 1,
                                        Status = SchedulingStatus.Released,
                                        ScheduleCharts = new List<AgentScheduleChart>
                                        {
                                            new AgentScheduleChart
                                            {
                                                Day = 0,
                                                Charts = new List<ScheduleChart>
                                                {
                                                    new ScheduleChart
                                                    {
                                                        StartTime = "00:00 am",
                                                        EndTime = "00:05pm",
                                                        SchedulingCodeId = 1
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
            }
        }.AsQueryable();

        private readonly IQueryable<AgentScheduleManager> agentScheduleManagersDB = new List<AgentScheduleManager>() {
            new AgentScheduleManager { Id = new ObjectId("5fe0b5ad6a05416894c0718e"), EmployeeId = 1, AgentSchedulingGroupId = 1, CreatedBy = "Admin", CreatedDate = DateTime.UtcNow,
                                       Date = DateTime.Now, Charts = new List<AgentScheduleManagerChart>
                                       {
                                           new AgentScheduleManagerChart {
                                               StartDateTime = DateTime.Now.AddHours(1),
                                               EndDateTime = DateTime.UtcNow.AddHours(2),
                                               SchedulingCodeId = 1
                                            }
                                       },
            },
            new AgentScheduleManager { Id = new ObjectId("5fe0b5ad6a05416894c0718f"), EmployeeId = 2, AgentSchedulingGroupId = 1, CreatedBy = "Admin", CreatedDate = DateTime.UtcNow,
                                       Date = DateTime.Now, Charts = new List<AgentScheduleManagerChart>
                                       {
                                           new AgentScheduleManagerChart {
                                               StartDateTime = DateTime.Now.AddHours(1),
                                               EndDateTime = DateTime.UtcNow.AddHours(2),
                                               SchedulingCodeId = 1
                                            }
                                       },
            },
        }.AsQueryable();

        private readonly IQueryable<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
            new SchedulingCode { Id = new ObjectId("5fe0b5ad6a05416894c0718d"), SchedulingCodeId = 1, Name = "lunch", IsDeleted = false},
            new SchedulingCode { Id = new ObjectId("5fe0b5c46a05416894c0718f"), SchedulingCodeId = 2, Name = "lunch", IsDeleted = false},
        }.AsQueryable();

        #endregion

        #region Client 

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public Client GetClient(ClientIdDetails clientIdDetails)
        {
            return clientsDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(clientIdDetails.ClientId.ToString())).FirstOrDefault();
        }

        #endregion

        #region Client LOB

        /// <summary>
        /// Gets the client lob.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The clientLOB group identifier details.</param>
        /// <returns></returns>
        public ClientLobGroup GetClientLobGroup(ClientLobGroupIdDetails clientLOBGroupIdDetails)
        {
            return clientLobGroupDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(clientLOBGroupIdDetails.ClientLobGroupId.ToString())).FirstOrDefault();
        }

        #endregion

        #region Skill Group

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        public SkillGroup GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails)
        {
            return skillGroupDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(skillGroupIdDetails.SkillGroupId.ToString())).FirstOrDefault();
        }

        #endregion

        #region Skill Tag

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public SkillTag GetSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            return skillTagDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(skillTagIdDetails.SkillTagId.ToString())).FirstOrDefault();
        }

        #endregion      

        #region Agent Admin

        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        public PagedList<Entity> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = agentAdminsDB.Where(x => x.IsDeleted == false);

            var filteredAgentAdmins = FilterAgentAdmin(agentAdmins, agentAdminQueryParameter.SearchKeyword, agentAdminQueryParameter.AgentSchedulingGroupId);

            var sortedAgentAdmins = SortHelper.ApplySort(filteredAgentAdmins, agentAdminQueryParameter.OrderBy);

            var pagedAgentAdmins = sortedAgentAdmins
                .Skip((agentAdminQueryParameter.PageNumber - 1) * agentAdminQueryParameter.PageSize)
                .Take(agentAdminQueryParameter.PageSize);

            // var mappedAgentAdmins = JsonConvert.DeserializeObject<List<AgentAdminDTO>>(JsonConvert.SerializeObject(pagedAgentAdmins)) as IQueryable<AgentAdminDTO>;

            var shapedAgentAdmins = DataShaper.ShapeData(pagedAgentAdmins, agentAdminQueryParameter.Fields);

            return PagedList<Entity>
                .ToPagedList(shapedAgentAdmins, filteredAgentAdmins.Count(), agentAdminQueryParameter.PageNumber, agentAdminQueryParameter.PageSize).Result;
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public Agent GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            return agentAdminsDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentAdminIdDetails.AgentAdminId)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the agent admins by employee ids.
        /// </summary>
        /// <param name="agentAdminEmployeeIdsDetails">The agent admin employee ids details.</param>
        /// <returns></returns>
        public List<Agent> GetAgentAdminsByEmployeeIds(List<int> agentAdminEmployeeIdsDetails)
        {
            return agentAdminsDB.Where(x => x.IsDeleted == false && agentAdminEmployeeIdsDetails.Contains(x.Ssn)).ToList();
        }

        /// <summary>
        /// Gets the agent admin ids by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public Agent GetAgentAdminIdsByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails)
        {
            return agentAdminsDB.Where(x => x.IsDeleted == false && x.Ssn == agentAdminEmployeeIdDetails.Id).FirstOrDefault();
        }

        //To be changed
        /// <summary>
        /// Gets the agent admins by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <param name="agentAdminSsoDetails">The agent admin sso details.</param>
        public Agent GetAgentAdminIdsByEmployeeIdAndSso(EmployeeIdDetails agentAdminEmployeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails)
        {
            return agentAdminsDB.Where(x => x.IsDeleted == false && (x.Ssn == agentAdminEmployeeIdDetails.Id || x.Sso == agentAdminSsoDetails.Sso)).FirstOrDefault();
        }
        /// <summary>Gets the agent admin ids by sso.</summary>
        /// <param name="agentAdminSsoDetails">The agent admin sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Agent GetAgentAdminIdsBySso(AgentAdminSsoDetails agentAdminSsoDetails)
        {
            return agentAdminsDB.Where(x => x.IsDeleted == false && x.Sso == agentAdminSsoDetails.Sso).FirstOrDefault();
        }

        /// <summary>Gets the agent scheduling group basedon skill tag.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public AgentSchedulingGroup GetAgentSchedulingGroupBasedonSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            return agentSchedulingGroupDB.Where(x => x.IsDeleted == false && x.SkillTagId == skillTagIdDetails.SkillTagId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the agent admins count.
        /// </summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public int GetAgentAdminsCount()
        {
            return agentAdminsDB.Where(x => x.IsDeleted == false).Count();
        }
        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void CreateAgentAdmin(Agent agentAdminRequest)
        {
            agentAdminsDB.ToList().Add(agentAdminRequest);
        }
        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void UpdateAgentAdmin(Agent agentAdminRequest)
        {
            var agentAdmin = agentAdminsDB.Where(x => x.Id == agentAdminRequest.Id).FirstOrDefault();
            if (agentAdmin != null)
            {
                agentAdmin = agentAdminRequest;
            }
        }
        // <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void DeleteAgentAdmin(Agent agentAdminRequest)
        {
            var agentAdmin = agentAdminsDB.Where(x => x.IsDeleted == false && x.Id == agentAdminRequest.Id).FirstOrDefault();
            if (agentAdmin != null)
            {
                agentAdmin.IsDeleted = true;
                agentAdmin.ModifiedDate = DateTime.UtcNow;
            }
        }

        /// <summary>Filters the agent admin.</summary>
        /// <param name="agentAdmins">The agent admins.</param>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<Agent> FilterAgentAdmin(IQueryable<Agent> agentAdmins, string searchKeyword, int? agentSchedulingGroupId)
        {
            if (!agentAdmins.Any())
            {
                return agentAdmins;
            }

            if (agentSchedulingGroupId != null && agentSchedulingGroupId != default(int))
            {
                agentAdmins = agentAdmins.Where(x => x.AgentSchedulingGroupId == agentSchedulingGroupId);
            }

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                agentAdmins = agentAdmins.ToList().Where(o => o.Sso.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.Sso.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)).AsQueryable();
            }

            return agentAdmins;
        }

        #endregion

        #region Activity Logs

        /// <summary>
        /// Creates the activity log.
        /// </summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        public void CreateActivityLog(ActivityLog activityLogRequest)
        {
            activityLogsDB.ToList().Add(activityLogRequest);
        }

        /// <summary>
        /// Creates the activity logs.
        /// </summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        public void CreateActivityLogs(List<ActivityLog> activityLogRequest)
        {
            activityLogsDB.ToList().AddRange(activityLogRequest);
        }

        /// <summary>Gets the agent activity logs.</summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public PagedList<Entity> GetActivityLogs(ActivityLogQueryParameter activityLogQueryParameter)
        {
            var filteredActivityLogs = FilterActivityLogs(activityLogsDB, activityLogQueryParameter);

            var sortedActivityLogs = SortHelper.ApplySort(filteredActivityLogs, activityLogQueryParameter.OrderBy);

            var pagedActivityLogs = sortedActivityLogs
                .Skip((activityLogQueryParameter.PageNumber - 1) * activityLogQueryParameter.PageSize)
                .Take(activityLogQueryParameter.PageSize);

            var shapedActivityLogs = DataShaper.ShapeData(pagedActivityLogs, activityLogQueryParameter.Fields);

            return PagedList<Entity>
                .ToPagedList(shapedActivityLogs, filteredActivityLogs.Count(), activityLogQueryParameter.PageNumber, activityLogQueryParameter.PageSize).Result;
        }

        /// <summary>Filters the activity logs.</summary>
        /// <param name="activityLogs">The activity logs.</param>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<ActivityLog> FilterActivityLogs(IQueryable<ActivityLog> activityLogs, ActivityLogQueryParameter activityLogQueryParameter)
        {
            if (!activityLogs.Any())
            {
                return activityLogs;
            }

            string searchKeyword = activityLogQueryParameter.SearchKeyword;

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                activityLogs = activityLogs.Where(o => o.ExecutedBy.ToLower().Contains(searchKeyword.Trim().ToLower()) ||
                                                           o.FieldDetails.Any(
                                                               field => field.Name.ToLower().Contains(searchKeyword.Trim().ToLower()) ||
                                                               field.NewValue.ToLower().Contains(searchKeyword.Trim().ToLower()) ||
                                                               field.OldValue.ToLower().Contains(searchKeyword.Trim().ToLower())
                                                               ));
            }

            if (activityLogQueryParameter.ActivityType != null)
            {
                activityLogs = activityLogs.Where(o => o.ActivityType == activityLogQueryParameter.ActivityType);
            }

            return activityLogs;
        }

        #endregion

        #region Agent Schedules

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public PagedList<Entity> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = agentSchedulesDB.Where(x => x.IsDeleted == false);

            var filteredAgentSchedules = FilterAgentSchedules(agentSchedules, agentScheduleQueryparameter);

            var sortedAgentSchedules = SortHelper.ApplySort(filteredAgentSchedules, agentScheduleQueryparameter.OrderBy);

            var pagedAgentSchedules = sortedAgentSchedules
                .Skip((agentScheduleQueryparameter.PageNumber - 1) * agentScheduleQueryparameter.PageSize)
                .Take(agentScheduleQueryparameter.PageSize);

            var shapedAgentSchedules = DataShaper.ShapeData(pagedAgentSchedules, agentScheduleQueryparameter.Fields);

            return PagedList<Entity>
                .ToPagedList(shapedAgentSchedules, filteredAgentSchedules.Count(), agentScheduleQueryparameter.PageNumber, agentScheduleQueryparameter.PageSize).Result;
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public AgentSchedule GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public AgentSchedule GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange, SchedulingStatus status)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId) &&
                                               x.Ranges.Any(x => x.Status == status && x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public AgentSchedule GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId) &&
                                               x.Ranges.Any(x => x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public AgentScheduleRange GetAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId) &&
                                               x.Ranges.Any(x => x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo)).FirstOrDefault();

            return agentSchedule?.Ranges.FirstOrDefault(x => x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public bool IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var count = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId) &&
                                               x.Ranges.Any(x => ((dateRange.DateFrom < x.DateTo && dateRange.DateTo > x.DateFrom) ||
                                               (dateRange.DateFrom == x.DateFrom && dateRange.DateTo == x.DateTo)))).Count();
            return count > 0;
        }

        /// <summary>
        /// Gets the agent schedule count.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public long GetAgentScheduleCount(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).Count();
        }

        /// <summary>
        /// Gets the employee identifier by agent schedule identifier.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public int GetEmployeeIdByAgentScheduleId(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault().EmployeeId;
        }

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public List<int> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.ActiveAgentSchedulingGroupId == agentSchedulingGroupIdDetails.AgentSchedulingGroupId)
                .Select(x => x.EmployeeId).ToList();
        }

        /// <summary>
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public AgentSchedule GetAgentScheduleByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == agentAdminEmployeeIdDetails.Id).FirstOrDefault();
        }

        /// <summary>
        /// Gets the agent schedule count by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        public long GetAgentScheduleCountByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == employeeIdDetails.Id).Count();
        }

        /// <summary>
        /// Creates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        public void CreateAgentSchedule(AgentSchedule agentScheduleRequest)
        {
            agentSchedulesDB.ToList().Add(agentScheduleRequest);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        public void UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month,
                                                         agentScheduleDetails.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month,
                                                       agentScheduleDetails.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            if (agentSchedule != null)
            {
                var agentScheduleRange = agentSchedule?.Ranges.FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom && x.DateTo == agentScheduleDetails.DateTo);
                if (agentScheduleRange != null)
                {
                    agentScheduleRange.DateFrom = agentScheduleDetails.DateFrom;
                    agentScheduleRange.DateTo = agentScheduleDetails.DateTo;
                    agentScheduleRange.Status = agentScheduleDetails.Status;
                    agentScheduleRange.ModifiedBy = agentScheduleDetails.ModifiedBy;
                    agentScheduleRange.ModifiedDate = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleEmployeeDetails">The update agent schedule employee details.</param>
        public void UpdateAgentSchedule(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails)
        {
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == employeeIdDetails.Id).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.EmployeeId = updateAgentScheduleEmployeeDetails.EmployeeId;
                agentSchedule.FirstName = updateAgentScheduleEmployeeDetails.FirstName;
                agentSchedule.LastName = updateAgentScheduleEmployeeDetails.LastName;
                agentSchedule.ActiveAgentSchedulingGroupId = updateAgentScheduleEmployeeDetails.AgentSchedulingGroupId;
                agentSchedule.ModifiedBy = updateAgentScheduleEmployeeDetails.ModifiedBy;
                agentSchedule.ModifiedDate = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Updates the agent schedule with ranges.
        /// </summary>
        /// <param name="employeeIdDetails">
        /// The employee identifier details.
        /// </param>
        /// <param name="updateAgentScheduleEmployeeDetails">
        /// The update agent schedule employee details.
        /// </param>
        public void UpdateAgentScheduleWithRanges(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails)
        {
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == employeeIdDetails.Id).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.EmployeeId = updateAgentScheduleEmployeeDetails.EmployeeId;
                agentSchedule.FirstName = updateAgentScheduleEmployeeDetails.FirstName;
                agentSchedule.LastName = updateAgentScheduleEmployeeDetails.LastName;
                agentSchedule.Ranges = updateAgentScheduleEmployeeDetails.Ranges;
                agentSchedule.ActiveAgentSchedulingGroupId = updateAgentScheduleEmployeeDetails.AgentSchedulingGroupId;
                agentSchedule.ModifiedBy = updateAgentScheduleEmployeeDetails.ModifiedBy;
                agentSchedule.ModifiedDate = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleCharts">The agent schedule charts.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        public void UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleRange agentScheduleRange, ModifiedUserDetails modifiedUserDetails)
        {
            agentScheduleRange.DateFrom = new DateTime(agentScheduleRange.DateFrom.Year, agentScheduleRange.DateFrom.Month,
                                                       agentScheduleRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleRange.DateTo = new DateTime(agentScheduleRange.DateTo.Year, agentScheduleRange.DateTo.Month,
                                                     agentScheduleRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            var scheduleRange = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId) &&
                                                            x.Ranges.Any(x => x.Status == SchedulingStatus.Pending_Schedule && x.DateFrom == agentScheduleRange.DateFrom &&
                                                                              x.DateTo == agentScheduleRange.DateTo)).FirstOrDefault();

            if (agentSchedule != null)
            {
                var range = agentSchedule.Ranges.Where(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                                    x.DateFrom == agentScheduleRange.DateFrom &&
                                                                    x.DateTo == agentScheduleRange.DateTo).FirstOrDefault();
                if (range != null)
                {
                    if (agentScheduleRange.ScheduleCharts.Any())
                    {
                        range = agentScheduleRange;
                    }
                    else
                    {
                        agentSchedule.Ranges.Remove(range);
                    }
                }
                else
                {
                    agentSchedule.Ranges.Add(agentScheduleRange);
                }
            }
        }

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        public void CopyAgentSchedules(EmployeeIdDetails employeeIdDetails, AgentScheduleRange agentScheduleRange)
        {
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == employeeIdDetails.Id).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.Ranges.Add(agentScheduleRange);
            }
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        public void UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange)
        {
            var newDateFrom = new DateTime(dateRange.NewDateFrom.Year, dateRange.NewDateFrom.Month, dateRange.NewDateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            var newDateTo = new DateTime(dateRange.NewDateTo.Year, dateRange.NewDateTo.Month, dateRange.NewDateTo.Day, 0, 0, 0, DateTimeKind.Utc);
            var oldDateFrom = new DateTime(dateRange.OldDateFrom.Year, dateRange.OldDateFrom.Month, dateRange.OldDateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            var oldDateTo = new DateTime(dateRange.OldDateTo.Year, dateRange.OldDateTo.Month, dateRange.OldDateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            if (agentSchedule != null)
            {
                var agentScheduleRange = agentSchedule?.Ranges.FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule && x.DateFrom == oldDateFrom && x.DateTo == oldDateTo);
                if (agentScheduleRange != null)
                {
                    agentScheduleRange.DateFrom = newDateFrom;
                    agentScheduleRange.DateTo = newDateTo;
                    agentScheduleRange.ModifiedBy = dateRange.ModifiedBy;
                    agentScheduleRange.ModifiedDate = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Deletes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        public void DeleteAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            if (agentSchedule != null)
            {
                var agentScheduleRange = agentSchedule?.Ranges.FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule && 
                                                                                   x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo);
                if (agentScheduleRange != null)
                {
                    agentSchedule.Ranges.Remove(agentScheduleRange);
                }
            }
        }

        /// <summary>
        /// Deletes the agent schedule range import.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <param name="dateRange">The date range.</param>
        public void DeleteAgentScheduleRangeImport(int employeeId, int agentSchedulingGroupId, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == employeeId && x.ActiveAgentSchedulingGroupId == agentSchedulingGroupId).FirstOrDefault();
            if (agentSchedule != null)
            {
                var agentScheduleRange = agentSchedule?.Ranges.FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule && 
                                                                                   x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo);
                if (agentScheduleRange != null)
                {
                    agentSchedule.Ranges.Remove(agentScheduleRange);
                }
            }
        }

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        public void DeleteAgentSchedule(EmployeeIdDetails employeeIdDetails)
        {
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.EmployeeId == employeeIdDetails.Id).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.IsDeleted = true;
                agentSchedule.ModifiedDate = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Filters the agent schedules.
        /// </summary>
        /// <param name="agentSchedules">The agent admins.</param>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        private IQueryable<AgentSchedule> FilterAgentSchedules(IQueryable<AgentSchedule> agentSchedules, AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            if (!agentSchedules.Any())
            {
                return agentSchedules;
            }

            if (!string.IsNullOrWhiteSpace(agentScheduleQueryparameter.SearchKeyword))
            {
                SchedulingStatus scheduleStatus = SchedulingStatus.Released;
                int.TryParse(agentScheduleQueryparameter.SearchKeyword.Trim(), out int employeeId);
                var status = Enum.GetNames(typeof(SchedulingStatus)).FirstOrDefault(e => e.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.ToLower()));

                if (!string.IsNullOrWhiteSpace(status))
                {
                    scheduleStatus = (SchedulingStatus)Enum.Parse(typeof(SchedulingStatus), status, true);
                }

                agentSchedules = agentSchedules.Where(o => (o.EmployeeId == employeeId && employeeId != 0) ||
                                                           (o.Ranges.Any(x => x.Status == scheduleStatus && !string.IsNullOrWhiteSpace(status))) ||
                                                            o.FirstName.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                            o.LastName.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                            o.CreatedBy.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                            o.ModifiedBy.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            if (agentScheduleQueryparameter.EmployeeIds.Any())
            {
                agentSchedules = agentSchedules.Where(x => agentScheduleQueryparameter.EmployeeIds.Distinct().ToList().Contains(x.EmployeeId));
            }

            if (agentScheduleQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.ActiveAgentSchedulingGroupId == agentScheduleQueryparameter.AgentSchedulingGroupId);
            }

            if (agentScheduleQueryparameter.Status.HasValue)
            {
                agentSchedules = agentSchedules.Where(x => x.Ranges.Exists(y => y.Status == agentScheduleQueryparameter.Status));
            }

            if (agentScheduleQueryparameter.DateFrom.HasValue && agentScheduleQueryparameter.DateFrom != default(DateTime) &&
                agentScheduleQueryparameter.DateTo.HasValue && agentScheduleQueryparameter.DateTo != default(DateTime) &&
                !agentScheduleQueryparameter.ExcludeConflictSchedule)
            {
                agentScheduleQueryparameter.DateFrom = new DateTime(agentScheduleQueryparameter.DateFrom.Value.Year, agentScheduleQueryparameter.DateFrom.Value.Month,
                                                                    agentScheduleQueryparameter.DateFrom.Value.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleQueryparameter.DateTo = new DateTime(agentScheduleQueryparameter.DateTo.Value.Year, agentScheduleQueryparameter.DateTo.Value.Month,
                                                                    agentScheduleQueryparameter.DateTo.Value.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedules = agentSchedules.Where(x => x.Ranges.Any(y => agentScheduleQueryparameter.DateFrom == y.DateFrom &&
                                                                             agentScheduleQueryparameter.DateTo == y.DateTo));
            }

            if (agentScheduleQueryparameter.DateFrom.HasValue && agentScheduleQueryparameter.DateFrom != default(DateTime) &&
                agentScheduleQueryparameter.DateTo.HasValue && agentScheduleQueryparameter.DateTo != default(DateTime) &&
                agentScheduleQueryparameter.ExcludeConflictSchedule)
            {
                agentScheduleQueryparameter.DateFrom = new DateTime(agentScheduleQueryparameter.DateFrom.Value.Year, agentScheduleQueryparameter.DateFrom.Value.Month,
                                                                    agentScheduleQueryparameter.DateFrom.Value.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleQueryparameter.DateTo = new DateTime(agentScheduleQueryparameter.DateTo.Value.Year, agentScheduleQueryparameter.DateTo.Value.Month,
                                                                    agentScheduleQueryparameter.DateTo.Value.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedules = agentSchedules.Where(x => !x.Ranges.Any(y => ((agentScheduleQueryparameter.DateFrom < y.DateTo &&
                                                                               agentScheduleQueryparameter.DateTo > y.DateFrom) ||
                                                                              (agentScheduleQueryparameter.DateFrom == y.DateFrom &&
                                                                               agentScheduleQueryparameter.DateTo == y.DateTo))));
            }

            return agentSchedules;
        }

        #endregion

        #region Agent Schedule Manager

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        public PagedList<Entity> GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            var agentScheduleManagers = agentScheduleManagersDB;

            var filteredAgentScheduleManagers = FilterAgentScheduleManagers(agentScheduleManagers, agentScheduleManagerChartQueryparameter);

            var sortedAgentScheduleManagers = SortHelper.ApplySort(filteredAgentScheduleManagers, agentScheduleManagerChartQueryparameter.OrderBy);

            var pagedAgentScheduleManagers = sortedAgentScheduleManagers;

            if (!agentScheduleManagerChartQueryparameter.SkipPageSize)
            {
                pagedAgentScheduleManagers = pagedAgentScheduleManagers
                    .Skip((agentScheduleManagerChartQueryparameter.PageNumber - 1) * agentScheduleManagerChartQueryparameter.PageSize)
                    .Take(agentScheduleManagerChartQueryparameter.PageSize);
            }

            var shapedAgentScheduleManagers = DataShaper.ShapeData(pagedAgentScheduleManagers, agentScheduleManagerChartQueryparameter.Fields);

            return PagedList<Entity>
                .ToPagedList(shapedAgentScheduleManagers, filteredAgentScheduleManagers.Count(), agentScheduleManagerChartQueryparameter.PageNumber,
                             agentScheduleManagerChartQueryparameter.PageSize).Result;
        }

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        public AgentScheduleManager GetAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails)
        {
            dateDetails.Date = new DateTime(dateDetails.Date.Year, dateDetails.Date.Month, dateDetails.Date.Day, 0, 0, 0, DateTimeKind.Utc);

            return agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id && x.Date == dateDetails.Date).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether [is agent schedule manager chart exists] [the specified employee identifier details].
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns>
        ///   <c>true</c> if [is agent schedule manager chart exists] [the specified employee identifier details]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAgentScheduleManagerChartExists(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails)
        {
            dateDetails.Date = new DateTime(dateDetails.Date.Year, dateDetails.Date.Month, dateDetails.Date.Day, 0, 0, 0, DateTimeKind.Utc);

            var count = agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id && x.Date == dateDetails.Date).Count();
            return count > 0;
        }

        /// <summary>Gets the agent schedule manager chart by employee identifier.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public List<AgentScheduleManager> GetAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            myScheduleQueryParameter.StartDate = new DateTime(myScheduleQueryParameter.StartDate.Year, myScheduleQueryParameter.StartDate.Month, myScheduleQueryParameter.StartDate.Day, 0, 0, 0, DateTimeKind.Utc);
            myScheduleQueryParameter.EndDate = new DateTime(myScheduleQueryParameter.EndDate.Year, myScheduleQueryParameter.EndDate.Month, myScheduleQueryParameter.EndDate.Day, 0, 0, 0, DateTimeKind.Utc);

            return agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id && x.AgentSchedulingGroupId == myScheduleQueryParameter.AgentSchedulingGroupId &&
                                                      x.Date >= myScheduleQueryParameter.StartDate && x.Date <= myScheduleQueryParameter.EndDate).ToList();
        }

        /// <summary>Gets the schedule manager chart by employee identifier from date.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public List<AgentScheduleManager> GetScheduleManagerChartByEmployeeIdFromDate(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            myScheduleQueryParameter.StartDate = new DateTime(myScheduleQueryParameter.StartDate.Year, myScheduleQueryParameter.StartDate.Month, myScheduleQueryParameter.StartDate.Day, 0, 0, 0, DateTimeKind.Utc);
            myScheduleQueryParameter.EndDate = new DateTime(myScheduleQueryParameter.EndDate.Year, myScheduleQueryParameter.EndDate.Month, myScheduleQueryParameter.EndDate.Day, 0, 0, 0, DateTimeKind.Utc);

            return agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id && x.AgentSchedulingGroupId == myScheduleQueryParameter.AgentSchedulingGroupId &&
                                                      x.Date >= myScheduleQueryParameter.StartDate).ToList();
        }

        /// <summary>
        /// Determines whether [has agent schedule manager chart by employee identifier] [the specified employee identifier details].
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns>
        ///   <c>true</c> if [has agent schedule manager chart by employee identifier] [the specified employee identifier details]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            return agentScheduleManagersDB.ToList().Exists(x => x.EmployeeId == employeeIdDetails.Id);
        }

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public List<int> GetManagerEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            return agentScheduleManagersDB.Where(x => x.AgentSchedulingGroupId == agentSchedulingGroupIdDetails.AgentSchedulingGroupId).Select(x => x.EmployeeId).ToList();
        }

        /// <summary>
        /// Filters the agent scheduled open.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent schedule manager</param
        /// <returns></returns>
        public List<AgentScheduleManager> GetAgentScheduleByAgentSchedulingGroupId(List<int> agentSchedulingGroupIdDetailsList, DateTimeOffset date)
        {
            var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            return agentScheduleManagersDB.Where(x => agentSchedulingGroupIdDetailsList.Contains(x.AgentSchedulingGroupId) && x.Date == dateTimeWithZeroTimeSpan).ToList();
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManager">The agent schedule manager.</param>
        public void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManager agentScheduleManager)
        {
            agentScheduleManager.Date = new DateTime(agentScheduleManager.Date.Year, agentScheduleManager.Date.Month,
                                                     agentScheduleManager.Date.Day, 0, 0, 0, DateTimeKind.Utc);

            var manager = agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id && x.Date == agentScheduleManager.Date).FirstOrDefault();
            if (manager != null)
            {
                if (agentScheduleManager.Charts.Any())
                {
                    manager.AgentSchedulingGroupId = agentScheduleManager.AgentSchedulingGroupId;
                    manager.Charts = agentScheduleManager.Charts;
                    manager.ModifiedBy = agentScheduleManager.CreatedBy;
                    manager.ModifiedDate = DateTimeOffset.UtcNow;
                }
                else
                {
                    agentScheduleManagersDB.ToList().Remove(manager);
                }
            }
            else
            {
                agentScheduleManagersDB.ToList().Add(agentScheduleManager);
            }
        }

        /// <summary>
        /// Updates the agent schedule manager.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleManagerEmployeeDetails">The update agent schedule manager employee details.</param>
        public void UpdateAgentScheduleManager(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails)
        {
            var agentScheduleManagers = agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id).ToList();
            agentScheduleManagers.ForEach(manager =>
            {
                manager.EmployeeId = updateAgentScheduleManagerEmployeeDetails.EmployeeId;
                manager.AgentSchedulingGroupId = updateAgentScheduleManagerEmployeeDetails.AgentSchedulingGroupId;
                manager.ModifiedBy = updateAgentScheduleManagerEmployeeDetails.ModifiedBy;
                manager.ModifiedDate = DateTimeOffset.UtcNow;
            });
        }

        /// <summary>
        /// Updates the agent schedule manager from moving date.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleManagerEmployeeDetails">The update agent schedule manager employee details.</param>
        public void UpdateAgentScheduleManagerFromMovingDate(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails)
        {
            updateAgentScheduleManagerEmployeeDetails.MovingDate = new DateTime(updateAgentScheduleManagerEmployeeDetails.MovingDate.Year, 
                                                                                updateAgentScheduleManagerEmployeeDetails.MovingDate.Month, 
                                                                                updateAgentScheduleManagerEmployeeDetails.MovingDate.Day, 0, 0, 0, 
                                                                                DateTimeKind.Utc);
            
            var agentScheduleManagers = agentScheduleManagersDB.Where(x => x.EmployeeId == employeeIdDetails.Id && x.Date >= updateAgentScheduleManagerEmployeeDetails.MovingDate).ToList();
            agentScheduleManagers.ForEach(manager =>
            {
                manager.EmployeeId = updateAgentScheduleManagerEmployeeDetails.EmployeeId;
                manager.AgentSchedulingGroupId = updateAgentScheduleManagerEmployeeDetails.AgentSchedulingGroupId;
                manager.ModifiedBy = updateAgentScheduleManagerEmployeeDetails.ModifiedBy;
                manager.ModifiedDate = DateTimeOffset.UtcNow;
            });
        }

        /// <summary>
        /// Filters the agent schedules.
        /// </summary>
        /// <param name="agentScheduleManagers">The agent schedule managerss.</param>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        private IQueryable<AgentScheduleManager> FilterAgentScheduleManagers(IQueryable<AgentScheduleManager> agentScheduleManagers,
                                                                             AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            if (!agentScheduleManagers.Any())
            {
                return agentScheduleManagers;
            }

            agentScheduleManagers = agentScheduleManagers.Where(x => x.EmployeeId != 0);

            if (agentScheduleManagerChartQueryparameter.EmployeeId.HasValue && agentScheduleManagerChartQueryparameter.EmployeeId != default(int))
            {
                agentScheduleManagers = agentScheduleManagers.Where(x => x.EmployeeId == agentScheduleManagerChartQueryparameter.EmployeeId);
            }

            if (agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentScheduleManagers = agentScheduleManagers.Where(x => x.AgentSchedulingGroupId == agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId);
            }

            if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime) &&
                !agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
            {
                var date = agentScheduleManagerChartQueryparameter.Date.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.Date == dateTimeWithZeroTimeSpan);
            }

            if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime) &&
                agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
            {
                var date = agentScheduleManagerChartQueryparameter.Date.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.Date != dateTimeWithZeroTimeSpan);
            }

            return agentScheduleManagers;
        }

        #endregion

        #region Scheduling Codes

        /// <summary>
        /// Gets the schedulingCodes.
        /// </summary>
        /// <param name="schedulingCodeQueryparameter">The schedulingCode queryparameter.</param>
        /// <returns></returns>
        public PagedList<Entity> GetSchedulingCodes(SchedulingCodeQueryparameter schedulingCodeQueryparameter)
        {
            var schedulingCodes = schedulingCodesDB.Where(x => x.IsDeleted == false);

            var filteredSchedulingCodes = FilterSchedulingCodes(schedulingCodes, schedulingCodeQueryparameter);

            var sortedSchedulingCodes = SortHelper.ApplySort(filteredSchedulingCodes, schedulingCodeQueryparameter.OrderBy);

            var pagedSchedulingCodes = sortedSchedulingCodes
                .Skip((schedulingCodeQueryparameter.PageNumber - 1) * schedulingCodeQueryparameter.PageSize)
                .Take(schedulingCodeQueryparameter.PageSize);

            var shapedSchedulingCodes = DataShaper.ShapeData(pagedSchedulingCodes, schedulingCodeQueryparameter.Fields);

            return PagedList<Entity>
                .ToPagedList(shapedSchedulingCodes, filteredSchedulingCodes.Count(), schedulingCodeQueryparameter.PageNumber, schedulingCodeQueryparameter.PageSize).Result;
        }

        /// <summary>
        /// Gets the schedulingCode.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The schedulingCode identifier details.</param>
        /// <returns></returns>
        public SchedulingCode GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            return schedulingCodesDB.Where(x => x.IsDeleted == false && x.SchedulingCodeId == schedulingCodeIdDetails.SchedulingCodeId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the scheduling codes by ids.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns></returns>
        public long GetSchedulingCodesCountByIds(List<int> codes)
        {
            return schedulingCodesDB.Where(x => x.IsDeleted == false && codes.Contains(x.SchedulingCodeId)).Count();
        }

        /// <summary>
        /// Gets the schedulingCodes count.
        /// </summary>
        /// <returns></returns>
        public int GetSchedulingCodesCount()
        {
            return schedulingCodesDB.Where(x => x.IsDeleted == false).Count();
        }

        /// <summary>
        /// Creates the schedulingCode.
        /// </summary>
        /// <param name="schedulingCodeRequest">The schedulingCode request.</param>
        public void CreateSchedulingCode(SchedulingCode schedulingCodeRequest)
        {
            schedulingCodesDB.ToList().Add(schedulingCodeRequest);
        }

        /// <summary>
        /// Updates the schedulingCode.
        /// </summary>
        /// <param name="schedulingCodeRequest">The schedulingCode request.</param>
        public void UpdateSchedulingCode(SchedulingCode schedulingCodeRequest)
        {
            var schedulingCode = schedulingCodesDB.Where(x => x.SchedulingCodeId == schedulingCodeRequest.SchedulingCodeId).FirstOrDefault();
            if (schedulingCode != null)
            {
                schedulingCode = schedulingCodeRequest;
            }
        }

        /// <summary>
        /// Filters the schedulingCodes.
        /// </summary>
        /// <param name="schedulingCodes">The schedulingCodes.</param>
        /// <param name="schedulingCodeQueryparameter">The schedulingCode queryparameter.</param>
        /// <returns></returns>
        private IQueryable<SchedulingCode> FilterSchedulingCodes(IQueryable<SchedulingCode> schedulingCodes, SchedulingCodeQueryparameter schedulingCodeQueryparameter)
        {
            if (!schedulingCodes.Any())
            {
                return schedulingCodes;
            }

            if (!string.IsNullOrWhiteSpace(schedulingCodeQueryparameter.SearchKeyword))
            {
                schedulingCodes = schedulingCodes.Where(o => o.Name.ToLower().Contains(schedulingCodeQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            return schedulingCodes;
        }

        #endregion
    }
}
