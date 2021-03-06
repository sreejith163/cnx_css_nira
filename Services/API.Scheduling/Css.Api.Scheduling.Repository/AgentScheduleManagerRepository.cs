﻿using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using System.Collections.Generic;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Newtonsoft.Json;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;

namespace Css.Api.Scheduling.Repository
{
    public class AgentScheduleManagerRepository : GenericRepository<AgentScheduleManager>, IAgentScheduleManagerRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="AgentScheduleManagerRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentScheduleManagerRepository(
            IMongoContext mongoContext,
            IAgentAdminRepository agentAdminRepository,
            IMapper mapper
            ) : base(mongoContext)

        {
            _mapper = mapper;
            _agentAdminRepository = agentAdminRepository;
        }

        /// <summary>
        /// The agent admin repository
        /// </summary>
        private readonly IAgentAdminRepository _agentAdminRepository;

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            var agents = await GetAgents(agentScheduleManagerChartQueryparameter);

            var mappedAgents = JsonConvert.DeserializeObject<List<AgentAdminDTO>>(JsonConvert.SerializeObject(agents));

            var agentScheduleManagers = FilterBy(x => true);

            var filteredAgentScheduleManagers = await FilterAgentScheduleManagers(agentScheduleManagers, agentScheduleManagerChartQueryparameter, mappedAgents);

            var sortedAgentScheduleManagers = SortHelper.ApplySort(filteredAgentScheduleManagers, agentScheduleManagerChartQueryparameter.OrderBy);

            var pagedAgentScheduleManagers = sortedAgentScheduleManagers;

            if (!agentScheduleManagerChartQueryparameter.SkipPageSize)
            {
                pagedAgentScheduleManagers = pagedAgentScheduleManagers
                    .Skip((agentScheduleManagerChartQueryparameter.PageNumber - 1) * agentScheduleManagerChartQueryparameter.PageSize)
                    .Take(agentScheduleManagerChartQueryparameter.PageSize);
            }

            var mappedAgentScheduleManagers = pagedAgentScheduleManagers
                .ProjectTo<AgentScheduleManagerChartDetailsDTO>(_mapper.ConfigurationProvider);

            var shapedAgentScheduleManagers = DataShaper.ShapeData(mappedAgentScheduleManagers, agentScheduleManagerChartQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentScheduleManagers, filteredAgentScheduleManagers.Count(), agentScheduleManagerChartQueryparameter.PageNumber,
                             agentScheduleManagerChartQueryparameter.PageSize);
        }


        private async Task<PagedList<Entity>> GetAgents(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            
            var agentAdminQueryParameter = _mapper.Map<AgentAdminQueryParameter>(agentScheduleManagerChartQueryparameter);
            agentAdminQueryParameter.Fields = "EmployeeId, FirstName, LastName, Sso, AgentSchedulingGroupId";
            agentAdminQueryParameter.SkipPageSize = true;

            return await _agentAdminRepository.GetAgentAdmins(agentAdminQueryParameter);
        }

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        public async Task<AgentScheduleManager> GetAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails)
        {
            dateDetails.Date = new DateTime(dateDetails.Date.Year, dateDetails.Date.Month, dateDetails.Date.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.Date, dateDetails.Date);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Determines whether [is agent schedule manager chart exists] [the specified employee identifier details].
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns>
        ///   <c>true</c> if [is agent schedule manager chart exists] [the specified employee identifier details]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsAgentScheduleManagerChartExists(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails)
        {
            dateDetails.Date = new DateTime(dateDetails.Date.Year, dateDetails.Date.Month, dateDetails.Date.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.Date, dateDetails.Date);

            var count = await FindCountByIdAsync(query);
            return count > 0;
        }

        /// <summary>Gets the agent schedule manager chart by employee identifier.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<AgentScheduleManager>> GetAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            var startDate = new DateTime(myScheduleQueryParameter.StartDate.Year, myScheduleQueryParameter.StartDate.Month, myScheduleQueryParameter.StartDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(myScheduleQueryParameter.EndDate.Year, myScheduleQueryParameter.EndDate.Month, myScheduleQueryParameter.EndDate.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.AgentSchedulingGroupId, myScheduleQueryParameter.AgentSchedulingGroupId) &
                Builders<AgentScheduleManager>.Filter.Gte(i => i.Date, startDate) &
                Builders<AgentScheduleManager>.Filter.Lte(i => i.Date, endDate);

            var agentAdmins = FilterBy(query);

            return await Task.FromResult(agentAdmins.ToList());
        }

        /// <summary>Gets the schedule manager chart by employee identifier from date.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<AgentScheduleManager>> GetScheduleManagerChartByEmployeeIdFromDate(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.AgentSchedulingGroupId, myScheduleQueryParameter.AgentSchedulingGroupId) &
                Builders<AgentScheduleManager>.Filter.Gte(i => i.Date, myScheduleQueryParameter.StartDate) ;

            var agentAdmins = FilterBy(query);

            return await Task.FromResult(agentAdmins.ToList());
        }

        /// <summary>
        /// Determines whether [has agent schedule manager chart by employee identifier] [the specified employee identifier details].
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns>
        ///   <c>true</c> if [has agent schedule manager chart by employee identifier] [the specified employee identifier details]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> HasAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id);

            var count = await FindCountByIdAsync(query);
            return count > 0;
        }

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<List<string>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.AgentSchedulingGroupId, agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

            var agentSchedules = FilterBy(query);

            return await Task.FromResult(agentSchedules.Select(x => x.EmployeeId).ToList());
        }

        /// <summary>
        /// Filters the agent scheduled open.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent schedule manager</param
        /// <returns></returns>
        public async Task<List<AgentScheduleManager>> GetAgentScheduleByAgentSchedulingGroupId(List<int> agentSchedulingGroupIdDetailsList, DateTimeOffset date)
        {
            var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            var less_one = dateTimeWithZeroTimeSpan.AddDays(-1);
            var datelist = new List<object>();
            datelist.Add(less_one);
            datelist.Add(dateTimeWithZeroTimeSpan);

            var query = Builders<AgentScheduleManager>.Filter.In(i => i.Date,  datelist) &
                     Builders<AgentScheduleManager>.Filter.In(i => i.AgentSchedulingGroupId, agentSchedulingGroupIdDetailsList);

            var x = FilterBy(query);

            return await Task.FromResult(x.ToList());
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

            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.Date, agentScheduleManager.Date);

            var documentCount = FindCountByIdAsync(query).Result;
            if (documentCount > 0)
            {
                if (agentScheduleManager.Charts.Any())
                {
                    var update = Builders<AgentScheduleManager>.Update
                        .Set(x => x.AgentSchedulingGroupId, agentScheduleManager.AgentSchedulingGroupId)
                        .Set(x => x.Charts, agentScheduleManager.Charts)
                        .Set(x => x.ModifiedBy, agentScheduleManager.CreatedBy)
                        .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

                    UpdateOneAsync(query, update);
                }
                else
                {
                    DeleteOneAsync(query);
                }
            }
            else
            {
                InsertOneAsync(agentScheduleManager);
            }
        }

        /// <summary>Updates the agent schedule manager.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleManagerEmployeeDetails">The update agent schedule manager employee details.</param>
        public void UpdateAgentScheduleManager(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id);

            var update = Builders<AgentScheduleManager>.Update
                .Set(x => x.EmployeeId, updateAgentScheduleManagerEmployeeDetails.EmployeeId)
                .Set(x => x.AgentSchedulingGroupId, updateAgentScheduleManagerEmployeeDetails.AgentSchedulingGroupId)
                .Set(x => x.ModifiedBy, updateAgentScheduleManagerEmployeeDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);
            
            UpdateManyAsync(query, update);
        }

        /// <summary>Updates the agent schedule manager from moving date.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleManagerEmployeeDetails">The update agent schedule manager employee details.</param>
        public void UpdateAgentScheduleManagerFromMovingDate(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails)
        {
            //var query =
            //    Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id);

            var query =
               Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
               Builders<AgentScheduleManager>.Filter.Gte(i => i.Date, updateAgentScheduleManagerEmployeeDetails.MovingDate);

            var update = Builders<AgentScheduleManager>.Update
                .Set(x => x.EmployeeId, updateAgentScheduleManagerEmployeeDetails.EmployeeId)
                .Set(x => x.AgentSchedulingGroupId, updateAgentScheduleManagerEmployeeDetails.AgentSchedulingGroupId)
                .Set(x => x.ModifiedBy, updateAgentScheduleManagerEmployeeDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateManyAsync(query, update);
        }

        /// <summary>
        /// Filters the agent schedules.
        /// </summary>
        /// <param name="agentScheduleManagers">The agent schedule managerss.</param>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        private async Task<IQueryable<AgentScheduleManagerChartDetailsDTO>> FilterAgentScheduleManagers(IQueryable<AgentScheduleManager> agentScheduleManagers,
                                                                             AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter, List<AgentAdminDTO> agentAdmins)
        {
            var searchKeyword = agentScheduleManagerChartQueryparameter.SearchKeyword;
            if (!agentScheduleManagers.Any())
            {
                var agentScheduleManagerChartDetails = _mapper.Map<IQueryable<AgentScheduleManagerChartDetailsDTO>>(agentScheduleManagers);
                return agentScheduleManagerChartDetails;
            }

            agentScheduleManagers = agentScheduleManagers.Where(x => x.EmployeeId != null || x.EmployeeId != "");

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                agentAdmins = agentAdmins.Where(o => o.Sso.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.FirstName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.LastName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.EmployeeId.ToString().Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)
                                                    ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(agentScheduleManagerChartQueryparameter.EmployeeId))
            {
                agentScheduleManagers = agentScheduleManagers.Where(x => x.EmployeeId == agentScheduleManagerChartQueryparameter.EmployeeId);
                agentAdmins = agentAdmins.Where(x => x.EmployeeId == agentScheduleManagerChartQueryparameter.EmployeeId).ToList();
            }

            if (agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentScheduleManagers = agentScheduleManagers.Where(x => x.AgentSchedulingGroupId == agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId);
            }


            if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime))
            {
                var date = agentScheduleManagerChartQueryparameter.Date.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.Date == dateTimeWithZeroTimeSpan);
            }

            var mappedAgentScheduleManagers = JsonConvert.DeserializeObject<List<AgentScheduleManagerChartDetailsDTO>>(JsonConvert.SerializeObject(agentScheduleManagers));

            if (agentAdmins.Any())
            {

                // loop through the agent admins and check from the schedule manager list if the agent has schedule
                foreach (var agent in agentAdmins)
                {
                    var mappedAgentScheduleManager = mappedAgentScheduleManagers.Find(x => x.EmployeeId == agent.EmployeeId);
                    var scheduleManagerExists =  await HasAgentScheduleManagerChartByEmployeeId(new EmployeeIdDetails { Id = agent.EmployeeId });

                    if (!scheduleManagerExists || mappedAgentScheduleManager == null)
                    {
                        // make a schedule placeholder if it doesn't exist
                        if (!agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
                        {
                            var agentScheduleManager = new AgentScheduleManagerChartDetailsDTO
                            {
                                EmployeeId = agent.EmployeeId,
                                FirstName = agent.FirstName,
                                LastName = agent.LastName,
                                AgentSchedulingGroupId = agent.AgentSchedulingGroupId,
                                ChartsCount = 0,
                                ChartsStartDateTime = DateTime.MaxValue
                        };
                            mappedAgentScheduleManagers.Add(agentScheduleManager);
                        }
                    }
                    else
                    {
                        // just map the name if it has a schedule
                        mappedAgentScheduleManager.FirstName = agent?.FirstName;
                        mappedAgentScheduleManager.LastName = agent?.LastName;
                        mappedAgentScheduleManager.ChartsCount = mappedAgentScheduleManager.Charts.Count;
                        mappedAgentScheduleManager.ChartsStartDateTime = mappedAgentScheduleManager.Charts.Count != 0 ? mappedAgentScheduleManager.Charts.Min(x => x.StartDateTime) : DateTime.MaxValue;
                    }
                }

            }

            //if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime) &&
            //    !agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
            //{
            //    var date = agentScheduleManagerChartQueryparameter.Date.Value;
            //    var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            //    agentScheduleManagers = agentScheduleManagers.Where(x => x.Date == dateTimeWithZeroTimeSpan);
            //}

            //if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime) &&
            //    agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
            //{
            //    var date = agentScheduleManagerChartQueryparameter.Date.Value;
            //    var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            //    agentScheduleManagers = agentScheduleManagers.Where(x => x.Date != dateTimeWithZeroTimeSpan);
            //}

            var schedules = mappedAgentScheduleManagers.AsQueryable().Where(x => x.FirstName != null && x.LastName != null);

            return schedules.AsQueryable();
        }
    }
}

