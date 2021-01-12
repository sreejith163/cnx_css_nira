using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentScheduleRepository : GenericRepository<AgentSchedule>, IAgentScheduleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentScheduleRepository(
            IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = FilterBy(x => x.IsDeleted == false);

            var filteredAgentSchedules = FilterAgentSchedules(agentSchedules, agentScheduleQueryparameter);

            var sortedAgentSchedules = SortHelper.ApplySort(filteredAgentSchedules, agentScheduleQueryparameter.OrderBy);

            var pagedAgentSchedules = sortedAgentSchedules;

            if (!agentScheduleQueryparameter.SkipPageSize)
            {
                pagedAgentSchedules = pagedAgentSchedules
                    .Skip((agentScheduleQueryparameter.PageNumber - 1) * agentScheduleQueryparameter.PageSize)
                    .Take(agentScheduleQueryparameter.PageSize);
            }

            var shapedAgentSchedules = DataShaper.ShapeData(pagedAgentSchedules, agentScheduleQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentSchedules, filteredAgentSchedules.Count(), agentScheduleQueryparameter.PageNumber, agentScheduleQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule count.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<long> GetAgentScheduleCount(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindCountByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentScheduleByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, agentAdminEmployeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule count by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        public async Task<long> GetAgentScheduleCountByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindCountByIdAsync(query);
        }

        /// <summary>
        /// Creates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        public void CreateAgentSchedule(AgentSchedule agentScheduleRequest)
        {
            InsertOneAsync(agentScheduleRequest);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        public void UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.DateFrom, agentScheduleDetails.DateFrom)
                .Set(x => x.DateTo, agentScheduleDetails.DateTo)
                .Set(x => x.Status, agentScheduleDetails.Status)
                .Set(x => x.ModifiedBy, agentScheduleDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleEmployeeDetails">The update agent schedule employee details.</param>
        public void UpdateAgentSchedule(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.EmployeeId, updateAgentScheduleEmployeeDetails.EmployeeId)
                .Set(x => x.AgentSchedulingGroupId, updateAgentScheduleEmployeeDetails.AgentSchedulingGroupId)
                .Set(x => x.ModifiedBy, updateAgentScheduleEmployeeDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleChart">The agent schedule chart.</param>
        public void UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleChart)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.ModifiedBy, agentScheduleChart.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow)
                .Set(x => x.AgentScheduleCharts, agentScheduleChart.AgentScheduleCharts);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        public void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManagerChart agentScheduleManagerChart,
                                                   ModifiedUserDetails modifiedUserDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.ModifiedBy, modifiedUserDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            agentScheduleManagerChart.Date = new DateTimeOffset(agentScheduleManagerChart.Date.Date, TimeSpan.Zero);

            var documentQuery = query & Builders<AgentSchedule>.Filter
                .ElemMatch(i => i.AgentScheduleManagerCharts, chart => chart.Date == agentScheduleManagerChart.Date);

            var documentCount = FindCountByIdAsync(documentQuery).Result;
            if (documentCount > 0)
            {
                query = documentQuery;
                update = update.Set(x => x.AgentScheduleManagerCharts[-1], agentScheduleManagerChart);
            }
            else
            {
                update = update.AddToSet(x => x.AgentScheduleManagerCharts, agentScheduleManagerChart);
            }

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        public void ImportAgentScheduleChart(ImportAgentScheduleChart agentScheduleDetails, ModifiedUserDetails modifiedUserDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, agentScheduleDetails.EmployeeId) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.ModifiedBy, modifiedUserDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow)
                .Set(x => x.AgentScheduleCharts, agentScheduleDetails.AgentScheduleCharts);

            if (agentScheduleDetails.DateFrom.HasValue && agentScheduleDetails.DateFrom != default(DateTimeOffset))
            {
                update = update.Set(x => x.DateFrom, agentScheduleDetails.DateFrom);
            }

            if (agentScheduleDetails.DateTo.HasValue && agentScheduleDetails.DateTo != default(DateTimeOffset))
            {
                update = update.Set(x => x.DateTo, agentScheduleDetails.DateTo);
            }

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="agentSchedule">The agent schedule.</param>
        /// <param name="copyAgentScheduleRequest">The copy agent schedule request.</param>
        public void CopyAgentSchedules(AgentSchedule agentSchedule, CopyAgentSchedule copyAgentScheduleRequest)
        {

            var query = 
                Builders<AgentSchedule>.Filter.Eq(i => i.AgentSchedulingGroupId, copyAgentScheduleRequest.AgentSchedulingGroupId) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            if (copyAgentScheduleRequest.EmployeeIds.Any())
            {
                query &= Builders<AgentSchedule>.Filter.In(i => i.EmployeeId, copyAgentScheduleRequest.EmployeeIds.Distinct().ToList());
            }

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.ModifiedBy, agentSchedule.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            switch (copyAgentScheduleRequest.AgentScheduleType)
            {
                case AgentScheduleType.SchedulingTab:
                    update = update
                        .Set(x => x.DateFrom, agentSchedule.DateFrom)
                        .Set(x => x.DateTo, agentSchedule.DateTo)
                        .Set(x => x.AgentScheduleCharts, agentSchedule.AgentScheduleCharts);
                    break;

                case AgentScheduleType.SchedulingMangerTab:
                    update = update.Set(x => x.AgentScheduleManagerCharts, agentSchedule.AgentScheduleManagerCharts);
                    break;

                default:
                    break;
            }

            UpdateManyAsync(query, update);
        }

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        public void DeleteAgentSchedule(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.IsDeleted, true)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateOneAsync(query, update);
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
                agentSchedules = agentSchedules.Where(x => agentScheduleQueryparameter.EmployeeIds.Distinct().ToList().Contains(x.EmployeeId));
            }

            if (agentScheduleQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.AgentSchedulingGroupId == agentScheduleQueryparameter.AgentSchedulingGroupId);
            }

            if (agentScheduleQueryparameter.FromDate.HasValue && agentScheduleQueryparameter.FromDate != default(DateTime))
            {
                agentSchedules = agentSchedules.Where(x => x.DateFrom >= agentScheduleQueryparameter.FromDate.Value.ToUniversalTime());
            }

            return agentSchedules;
        }
    }
}

