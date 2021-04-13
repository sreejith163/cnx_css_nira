using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Request.Common;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for agent schedule manager collection
    /// </summary>
    public class AgentScheduleManagerRepository : GenericRepository<AgentScheduleManager>, IAgentScheduleManagerRepository
    {
        
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleManagerRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentScheduleManagerRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// The method to fetch manager schedules for input employees
        /// </summary>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        public async Task<List<AgentScheduleManager>> GetManagerSchedules(List<int> agentIds)
        {
            var query = Builders<AgentScheduleManager>.Filter.Where(i => agentIds.Contains(i.EmployeeId));

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to fetch manager schedules for input employees
        /// </summary>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        public async Task<List<AgentScheduleManager>> GetManagerSchedules(List<int> agentIds, List<DateTime> dates)
        {
            List<DateTime> schDates = dates.Select(x => { return new DateTime(x.Year, x.Month, x.Day, 0, 0, 0, DateTimeKind.Utc); }).ToList();

            var query = Builders<AgentScheduleManager>.Filter.Where(i => agentIds.Contains(i.EmployeeId)) &
                        Builders<AgentScheduleManager>.Filter.Where(i => schDates.Contains(i.Date));

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to fetch manager schedules for input employee from a specific date
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public async Task<List<AgentScheduleManager>> GetManagerSchedules(int agentId, DateTime fromDate)
        {
            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var query = Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, agentId) &
                        Builders<AgentScheduleManager>.Filter.Where(i => i.Date >= fromDate);

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to fetch manager schedules for matching filter details
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<AgentScheduleManager>> GetManagerSchedules(ScheduleFilter filter)
        {
            filter.StartDate = new DateTime(filter.StartDate.Date.Year, filter.StartDate.Month, filter.StartDate.Day, 0, 0, 0, DateTimeKind.Utc);
            filter.EndDate = new DateTime(filter.EndDate.Date.Year, filter.EndDate.Month, filter.EndDate.Day, 0, 0, 0, DateTimeKind.Utc);

            FilterDefinition<AgentScheduleManager> query;             
            if(filter.AgentIds.Any())
            {
                query = Builders<AgentScheduleManager>.Filter.Where(i => i.Date >= filter.StartDate && i.Date <= filter.EndDate)
                    & Builders<AgentScheduleManager>.Filter.In(i => i.EmployeeId, filter.AgentIds);
            }
            else
            {
                query = Builders<AgentScheduleManager>.Filter.Where(i => i.Date >= filter.StartDate && i.Date <= filter.EndDate)
                    & Builders<AgentScheduleManager>.Filter.In(i => i.AgentSchedulingGroupId, filter.AgentSchedulingGroupIds);
            }

            if (filter.UpdatedInPastDays.HasValue && filter.UpdatedInPastDays.Value > 0)
            {
                var endDateTime = DateTime.UtcNow;
                var startDateTime = endDateTime.AddDays(-filter.UpdatedInPastDays.Value).Date;

                query = query & (Builders<AgentScheduleManager>.Filter.Where(i => i.CreatedDate >= startDateTime && i.CreatedDate <= endDateTime)
                            | Builders<AgentScheduleManager>.Filter.Where(i => i.ModifiedDate >= startDateTime && i.ModifiedDate <= endDateTime));
            }

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to update the agent scheduling group for the manager charts of an agent
        /// </summary>
        /// <param name="updatedAgentSchedulingGroupDetails"></param>
        public void UpdateAgentSchedulingGroupForManagerCharts(UpdatedAgentSchedulingGroupDetails updatedAgentSchedulingGroupDetails)
        {
            updatedAgentSchedulingGroupDetails.StartDate = new DateTime(updatedAgentSchedulingGroupDetails.StartDate.Year,
                                    updatedAgentSchedulingGroupDetails.StartDate.Month, updatedAgentSchedulingGroupDetails.StartDate.Day,
                                    0, 0, 0, DateTimeKind.Utc);

            var query = Builders<AgentScheduleManager>.Filter.Where(i => i.EmployeeId == updatedAgentSchedulingGroupDetails.EmployeeId
                                                                    && i.Date >= updatedAgentSchedulingGroupDetails.StartDate);
            var update = Builders<AgentScheduleManager>.Update
                            .Set(x => x.AgentSchedulingGroupId, updatedAgentSchedulingGroupDetails.CurrentAgentSchedulingGroupId);

            UpdateManyAsync(query, update);
        }

        /// <summary>
        /// The method to update the agent scheduling group for the respective manager charts for all input agents
        /// </summary>
        /// <param name="updatedAgentSchedulingGroupDetailsList"></param>
        public void UpdateAgentSchedulingGroupForManagerCharts(List<UpdatedAgentSchedulingGroupDetails> updatedAgentSchedulingGroupDetailsList)
        {
            updatedAgentSchedulingGroupDetailsList.ForEach(updatedAgentSchedulingGroupDetails =>
            {
                updatedAgentSchedulingGroupDetails.StartDate = new DateTime(updatedAgentSchedulingGroupDetails.StartDate.Year,
                                    updatedAgentSchedulingGroupDetails.StartDate.Month, updatedAgentSchedulingGroupDetails.StartDate.Day,
                                    0, 0, 0, DateTimeKind.Utc);

                var query = Builders<AgentScheduleManager>.Filter.Where(i => i.EmployeeId == updatedAgentSchedulingGroupDetails.EmployeeId 
                                                                    && i.Date >= updatedAgentSchedulingGroupDetails.StartDate);
                var update = Builders<AgentScheduleManager>.Update
                                .Set(x => x.AgentSchedulingGroupId, updatedAgentSchedulingGroupDetails.CurrentAgentSchedulingGroupId);

                UpdateManyAsync(query, update);
            });
        }

        /// <summary>
        /// The method to insert manager schedules for agents if it doesn't exist
        /// </summary>
        /// <param name="agentSchedules">The agents manager schedules to be updated</param>
        public void UpsertAgentScheduleManagers(List<AgentScheduleManager> agentScheduleManagers)
        {
            agentScheduleManagers.ForEach(agentScheduleManagerDetails =>
            {
                agentScheduleManagerDetails.Date = new DateTime(agentScheduleManagerDetails.Date.Year, agentScheduleManagerDetails.Date.Month,
                                agentScheduleManagerDetails.Date.Day, 0, 0, 0, DateTimeKind.Utc);

                var query = Builders<AgentScheduleManager>.Filter.Eq(i => i.EmployeeId, agentScheduleManagerDetails.EmployeeId) &
                           Builders<AgentScheduleManager>.Filter.Eq(i => i.Date, agentScheduleManagerDetails.Date);

                var update = Builders<AgentScheduleManager>.Update.SetOnInsert(x => x.EmployeeId, agentScheduleManagerDetails.EmployeeId)
                    .Set(x => x.Charts, agentScheduleManagerDetails.Charts)
                    .SetOnInsert(x => x.CreatedBy, agentScheduleManagerDetails.CreatedBy)
                    .SetOnInsert(x => x.CreatedDate, agentScheduleManagerDetails.CreatedDate)
                    .SetOnInsert(x => x.Date, agentScheduleManagerDetails.Date)
                    .Set(x => x.ModifiedBy, agentScheduleManagerDetails.ModifiedBy)
                    .Set(x => x.ModifiedDate, agentScheduleManagerDetails.ModifiedDate);
                ;

                if (agentScheduleManagerDetails.AgentSchedulingGroupId > 0)
                {
                    update = update.Set(x => x.AgentSchedulingGroupId, agentScheduleManagerDetails.AgentSchedulingGroupId);
                }

                UpdateOneAsync(query, update, new UpdateOptions
                {
                    IsUpsert = true
                });
            });
        }
    }
}
