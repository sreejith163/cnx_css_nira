using Css.Api.Core.DataAccess.Repository.NoSQL;
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
        public AgentScheduleManagerRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            var agentScheduleManagers = FilterBy(x => true);

            var filteredAgentScheduleManagers = FilterAgentScheduleManagers(agentScheduleManagers, agentScheduleManagerChartQueryparameter);

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

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        public async Task<AgentScheduleManager> GetAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails)
        {
            dateDetails.Date = new DateTime(dateDetails.Date.Year, dateDetails.Date.Month, dateDetails.Date.Day, 0, 0, 0);

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
            dateDetails.Date = new DateTime(dateDetails.Date.Year, dateDetails.Date.Month, dateDetails.Date.Day, 0, 0, 0);

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
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.AgentSchedulingGroupId, myScheduleQueryParameter.AgentSchedulingGroupId) &
                Builders<AgentScheduleManager>.Filter.Gte(i => i.Date, myScheduleQueryParameter.StartDate) &
                Builders<AgentScheduleManager>.Filter.Lte(i => i.Date, myScheduleQueryParameter.EndDate);

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
        public async Task<List<int>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.AgentSchedulingGroupId, agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

            var agentSchedules = FilterBy(query);

            return await Task.FromResult(agentSchedules.Select(x => x.EmployeeId).ToList());
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManager">The agent schedule manager.</param>
        public void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManager agentScheduleManager)
        {
            agentScheduleManager.Date = new DateTime(agentScheduleManager.Date.Year, agentScheduleManager.Date.Month,
                                                     agentScheduleManager.Date.Day, 0, 0, 0);

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
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.Date == dateTimeWithZeroTimeSpan);
            }

            if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime) &&
                agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
            {
                var date = agentScheduleManagerChartQueryparameter.Date.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.Date != dateTimeWithZeroTimeSpan);
            }

            return agentScheduleManagers;
        }
    }
}

