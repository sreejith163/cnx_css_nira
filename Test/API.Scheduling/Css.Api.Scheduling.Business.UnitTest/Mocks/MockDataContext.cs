using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;

namespace Css.Api.Scheduling.Business.UnitTest.Mocks
{
    public class MockDataContext
    {
        #region Data

        readonly IQueryable<Client> clientsDB = new List<Client>() 
        {
            new Client { ClientId = 1, Name = "Client Name 1", IsDeleted = false },
            new Client { ClientId = 2, Name = "Client Name 2", IsDeleted = false }
        }.AsQueryable();

        readonly IQueryable<ClientLobGroup> clientLobGroupDB = new List<ClientLobGroup>() 
        {
            new ClientLobGroup { ClientId = 1, ClientLobGroupId = 1, Name = "Client Lob 1", IsDeleted = false },
            new ClientLobGroup { ClientId = 2, ClientLobGroupId = 2, Name = "Client Lob 2", IsDeleted = false }
        }.AsQueryable();

        readonly IQueryable<SkillGroup> skillGroupDB = new List<SkillGroup>()
        {
            new SkillGroup { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, Name = "Skill Group 1", IsDeleted = false },
            new SkillGroup { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, Name = "Skill Group 2", IsDeleted = false }
        }.AsQueryable();

        readonly IQueryable<SkillTag> skillTagDB = new List<SkillTag>() 
        {
            new SkillTag { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, Name = "Skill Tag 1", IsDeleted = false },
            new SkillTag { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, SkillTagId = 2, Name = "Skill Tag 2", IsDeleted = false }
        }.AsQueryable();

        readonly IQueryable<AgentSchedulingGroup> agentSchedulingGroupDB = new List<AgentSchedulingGroup>() 
        {
            new AgentSchedulingGroup { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, AgentSchedulingGroupId = 1,
                                       Name = "Agent Scheduliung Group 1", IsDeleted = false },
            new AgentSchedulingGroup { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, SkillTagId = 2, AgentSchedulingGroupId = 2,
                                       Name = "Agent Scheduliung Group 2", IsDeleted = false }
        }.AsQueryable();

        readonly IQueryable<Agent> agentAdminsDB = new List<Agent>()
        {
            new Agent { Id = new ObjectId("5fe0b5ad6a05416894c0718d"), AgentAdminId = 1, FirstName = "abc", LastName = "def", Ssn = 1, 
                        Sso = "user1@concentrix.com", ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, AgentSchedulingGroupId = 1,
                        CreatedBy = "Admin", CreatedDate = DateTime.UtcNow },
            new Agent { Id = new ObjectId("5fe0b5c46a05416894c0718f"), AgentAdminId = 2, FirstName = "lmn", LastName = "pqr", Ssn = 2,
                        Sso = "user2@concentrix.com", ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, AgentSchedulingGroupId = 1,
                        CreatedBy = "Admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        readonly IQueryable<AgentSchedule> agentSchedulesDB = new List<AgentSchedule>() {
            new AgentSchedule { Id = new ObjectId("5fe0b5ad6a05416894c0718e"), EmployeeId = 1, DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow,
                                Status = SchedulingStatus.Approved, AgentSchedulingGroupId = 1, CreatedBy = "Admin", CreatedDate = DateTime.UtcNow, IsDeleted = false,
                                AgentScheduleCharts = new List<AgentScheduleChart>
                                {
                                    new AgentScheduleChart {
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
                                },
                                AgentScheduleManagerCharts = new List<AgentScheduleManagerChart>
                                {
                                    new AgentScheduleManagerChart
                                    {
                                        Date = DateTime.UtcNow,
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
            },
            new AgentSchedule { Id = new ObjectId("5fe0b5ad6a05416894c0718f"), EmployeeId = 2, DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow,
                                Status = SchedulingStatus.Approved, AgentSchedulingGroupId = 1, CreatedBy = "Admin", CreatedDate = DateTime.UtcNow, IsDeleted = false,
                                AgentScheduleCharts = new List<AgentScheduleChart>
                                {
                                    new AgentScheduleChart {
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
                                },
                                AgentScheduleManagerCharts = new List<AgentScheduleManagerChart>
                                {
                                    new AgentScheduleManagerChart
                                    {
                                        Date = DateTime.UtcNow,
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
        }.AsQueryable();

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

            var filteredAgentAdmins = FilterAgentAdmin(agentAdmins, agentAdminQueryParameter);

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
            return agentAdminsDB.Where(x => x.IsDeleted == false && x.AgentAdminId == agentAdminIdDetails.AgentAdminId).FirstOrDefault();
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

        /// <summary>
        /// Filters the agent admin.
        /// </summary>
        /// <param name="agentAdmins">The agent admins.</param>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<Agent> FilterAgentAdmin(IQueryable<Agent> agentAdmins, AgentAdminQueryParameter agentAdminQueryParameter)
        {
            if (!agentAdmins.Any())
            {
                return agentAdmins;
            }

            if (!string.IsNullOrWhiteSpace(agentAdminQueryParameter.SearchKeyword))
            {
                agentAdmins = agentAdmins.Where(o => o.FirstName.ToLower().Contains(agentAdminQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                     o.LastName.ToLower().Contains(agentAdminQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                     o.CreatedBy.ToLower().Contains(agentAdminQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                     o.ModifiedBy.ToLower().Contains(agentAdminQueryParameter.SearchKeyword.Trim().ToLower()));
            }

            return agentAdmins;
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
        /// Gets the agent schedule count.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public long GetAgentScheduleCount(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            return agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).Count();
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
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.DateFrom = agentScheduleDetails.DateFrom;
                agentSchedule.DateTo = agentScheduleDetails.DateTo;
                agentSchedule.Status = agentScheduleDetails.Status;
                agentSchedule.ModifiedBy = agentScheduleDetails.ModifiedBy;
                agentSchedule.ModifiedDate = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        public void UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails)
        {
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.ModifiedBy = agentScheduleDetails.ModifiedBy;
                agentSchedule.ModifiedDate = DateTime.UtcNow;
            }

            switch (agentScheduleDetails.AgentScheduleType)
            {
                case AgentScheduleType.SchedulingTab:
                    agentSchedule.AgentScheduleCharts = agentScheduleDetails.AgentScheduleCharts;
                    break;

                case AgentScheduleType.SchedulingMangerTab:
                    var agentScheduleManagerChart = agentSchedule.AgentScheduleManagerCharts.Where(x => x.Date == agentScheduleDetails.AgentScheduleManagerChart.Date).FirstOrDefault();
                    if (agentScheduleManagerChart != null)
                    {
                        agentScheduleManagerChart = agentScheduleDetails.AgentScheduleManagerChart;
                    }
                    else
                    {
                        agentSchedule.AgentScheduleManagerCharts.Add(agentScheduleDetails.AgentScheduleManagerChart);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        public void ImportAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart agentScheduleDetails)
        {
            var agentSchedule = agentSchedulesDB.Where(x => x.IsDeleted == false && x.Id == new ObjectId(agentScheduleIdDetails.AgentScheduleId)).FirstOrDefault();
            if (agentSchedule != null)
            {
                agentSchedule.ModifiedBy = agentScheduleDetails.ModifiedBy;
                agentSchedule.ModifiedDate = DateTime.UtcNow;
            }

            switch (agentScheduleDetails.AgentScheduleType)
            {
                case AgentScheduleType.SchedulingTab:
                    agentSchedule.AgentScheduleCharts = agentScheduleDetails.AgentScheduleCharts;
                    break;

                case AgentScheduleType.SchedulingMangerTab:
                    agentSchedule.AgentScheduleManagerCharts = agentScheduleDetails.AgentScheduleManagerCharts;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="agentSchedule">The agent schedule.</param>
        /// <param name="copyAgentScheduleRequest">The copy agent schedule request.</param>
        public void CopyAgentSchedules(AgentSchedule agentSchedule, CopyAgentSchedule copyAgentScheduleRequest)
        {
            var agentSchedules = agentSchedulesDB.Where(x => x.IsDeleted == false && copyAgentScheduleRequest.EmployeeIds.Contains(x.EmployeeId)).ToList();
            agentSchedules.ForEach(x => { x.ModifiedBy = agentSchedule.ModifiedBy; x.ModifiedDate = DateTime.UtcNow; });

            switch (copyAgentScheduleRequest.AgentScheduleType)
            {
                case AgentScheduleType.SchedulingTab:
                    agentSchedules.ForEach(x => x.AgentScheduleCharts = agentSchedule.AgentScheduleCharts);
                    break;

                case AgentScheduleType.SchedulingMangerTab:
                    agentSchedules.ForEach(x => x.AgentScheduleManagerCharts = agentSchedule.AgentScheduleManagerCharts);
                    break;

                default:
                    break;
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
                agentSchedules = agentSchedules.Where(o => o.CreatedBy.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                           o.ModifiedBy.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            if (agentScheduleQueryparameter.EmployeeIds.Any())
            {
                agentSchedules = agentSchedules.Where(x => agentScheduleQueryparameter.EmployeeIds.Contains(x.EmployeeId));
            }

            if (agentScheduleQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.AgentSchedulingGroupId == agentScheduleQueryparameter.AgentSchedulingGroupId);
            }

            if (agentScheduleQueryparameter.FromDate.HasValue && agentScheduleQueryparameter.FromDate != default(DateTime))
            {
                agentSchedules = agentSchedules.Where(x => x.DateFrom >= agentScheduleQueryparameter.FromDate);
            }

            return agentSchedules;
        }

        #endregion
    }
}
