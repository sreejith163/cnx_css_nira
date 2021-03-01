using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using System.Collections.Generic;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

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
            var agentScheduleManagers = FilterBy(x => x.IsDeleted == false);

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
                .ProjectTo<AgentScheduleManagerDTO>(_mapper.ConfigurationProvider);

            var shapedAgentScheduleManagers = DataShaper.ShapeData(mappedAgentScheduleManagers, agentScheduleManagerChartQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentScheduleManagers, filteredAgentScheduleManagers.Count(), agentScheduleManagerChartQueryparameter.PageNumber,
                             agentScheduleManagerChartQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="agentScheduleManagerIdDetails">The agent schedule manager identifier details.</param>
        /// <returns></returns>
        public async Task<AgentScheduleManager> GetAgentScheduleManagerChart(AgentScheduleManagerIdDetails agentScheduleManagerIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleManagerIdDetails.AgentScheduleManagerId)) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule manager chart by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        public async Task<AgentScheduleManager> GetAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<AgentScheduleManager>.Filter.ElemMatch(i => i.AgentScheduleManagerCharts, range => range.AgentSchedulingGroupId == agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

            var agentSchedules = FilterBy(query);

            return await Task.FromResult(agentSchedules.Select(x => x.EmployeeId).ToList());
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        public void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManagerChart agentScheduleManagerChart)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.IsDeleted, false);

            agentScheduleManagerChart.Date = agentScheduleManagerChart.Date.Add(TimeSpan.Zero);

            var documentQuery = query & Builders<AgentScheduleManager>.Filter
                .ElemMatch(i => i.AgentScheduleManagerCharts, chart => chart.Date == agentScheduleManagerChart.Date);

            var documentCount = FindCountByIdAsync(documentQuery).Result;
            if (documentCount > 0)
            {
                query = documentQuery;
                if (agentScheduleManagerChart.Charts.Any())
                {
                    var update = Builders<AgentScheduleManager>.Update.Set(x => x.AgentScheduleManagerCharts[-1], agentScheduleManagerChart);
                    UpdateOneAsync(query, update);
                }
                else
                {
                    var update = Builders<AgentScheduleManager>.Update.PullFilter(x => x.AgentScheduleManagerCharts, builder => builder.Date == agentScheduleManagerChart.Date);
                    UpdateOneAsync(query, update);
                }
            }
            else
            {
                var update = Builders<AgentScheduleManager>.Update.AddToSet(x => x.AgentScheduleManagerCharts, agentScheduleManagerChart);
                UpdateOneAsync(query, update, new UpdateOptions { IsUpsert = true });
            }
        }

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        public void CopyAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManagerChart agentScheduleManagerChart)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentScheduleManager>.Update
                .AddToSet(x => x.AgentScheduleManagerCharts, agentScheduleManagerChart);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Deletes the agent schedule manager.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        public void DeleteAgentScheduleManager(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentScheduleManager>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentScheduleManager>.Update
                .Set(x => x.IsDeleted, true)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateOneAsync(query, update);
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

            if (agentScheduleManagerChartQueryparameter.EmployeeId.HasValue && agentScheduleManagerChartQueryparameter.EmployeeId != default(int))
            {
                agentScheduleManagers = agentScheduleManagers.Where(x => x.EmployeeId == agentScheduleManagerChartQueryparameter.EmployeeId);
            }

            if (agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentScheduleManagers = agentScheduleManagers.Where(x => x.CurrentAgentShedulingGroupId == agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId);
            }

            if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime) &&
                !agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule.HasValue)
            {
                var dateTimeWithZeroTimeSpan = agentScheduleManagerChartQueryparameter.Date.Value.Add(TimeSpan.Zero);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.AgentScheduleManagerCharts.Exists(y => y.Date == dateTimeWithZeroTimeSpan));
            }

            if (agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule.HasValue)
            {
                var dateTimeWithZeroTimeSpan = agentScheduleManagerChartQueryparameter.Date.Value.Add(TimeSpan.Zero);
                agentScheduleManagers = agentScheduleManagers.Where(x => x.AgentScheduleManagerCharts.Exists(y => y.Date != dateTimeWithZeroTimeSpan));
            }

            return agentScheduleManagers;
        }
    }
}

