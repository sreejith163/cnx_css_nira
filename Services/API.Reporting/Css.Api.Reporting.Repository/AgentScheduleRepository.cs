using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.DTO.Request;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Css.Api.Reporting.Models.DTO.Request.Common;
using Css.Api.Reporting.Models.DTO.Processing;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for agent schedule collection
    /// </summary>
    public class AgentScheduleRepository: GenericRepository<AgentSchedule>, IAgentScheduleRepository
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentScheduleRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// The method to fetch schedules for all agent for a date
        /// </summary>
        /// <param name="filter">The filter condition based on which schedules are to be retrieved</param>
        /// <returns>The list of instances of AgentSchedule</returns>
        public async Task<List<AgentSchedule>> GetSchedules(ScheduleFilter filter)
        {
            var query = Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<AgentSchedule>.Filter.In(i => i.ActiveAgentSchedulingGroupId, filter.AgentSchedulingGroupIds) &
                (
                    Builders<AgentSchedule>.Filter.Where(i => i.Ranges.Any(x => x.DateFrom <= filter.StartDate && filter.StartDate <= x.DateTo)) |
                    Builders<AgentSchedule>.Filter.Where(i => i.Ranges.Any(x => x.DateFrom <= filter.EndDate && filter.EndDate <= x.DateTo)) |
                    Builders<AgentSchedule>.Filter.Where(i => i.Ranges.Any(x => x.DateFrom > filter.StartDate && filter.EndDate > x.DateTo))
                );

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to fetch schedules for input employees
        /// </summary>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        public async Task<List<AgentSchedule>> GetSchedules(List<string> agentIds)
        {
            var query = Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<AgentSchedule>.Filter.Where(i => agentIds.Contains(i.EmployeeId));

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to insert schedules for agents if it doesn't exist
        /// </summary>
        /// <param name="agentSchedules">The agents schedules to be updated</param>
        public void InsertAgentSchedules(List<AgentSchedule> agentSchedules)
        {
            agentSchedules.ForEach(agentScheduleDetails =>
            {
                var query = Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, agentScheduleDetails.EmployeeId);

                var update = Builders<AgentSchedule>.Update.SetOnInsert(x => x.EmployeeId, agentScheduleDetails.EmployeeId)
                    .SetOnInsert(x => x.IsDeleted, agentScheduleDetails.IsDeleted)
                    .SetOnInsert(x => x.CreatedBy, agentScheduleDetails.CreatedBy)
                    .SetOnInsert(x => x.CreatedDate, agentScheduleDetails.CreatedDate)
                    .Set(x => x.Ranges, agentScheduleDetails.Ranges)
                    .Set(x => x.ModifiedBy, agentScheduleDetails.ModifiedBy)
                    .Set(x => x.ModifiedDate, agentScheduleDetails.ModifiedDate);

                if(!string.IsNullOrWhiteSpace(agentScheduleDetails.FirstName))
                {
                    update = update.Set(x => x.FirstName, agentScheduleDetails.FirstName);
                }

                if(!string.IsNullOrWhiteSpace(agentScheduleDetails.LastName))
                {
                    update = update.Set(x => x.LastName, agentScheduleDetails.LastName);
                }

                if(agentScheduleDetails.ActiveAgentSchedulingGroupId > 0)
                {
                    update = update.Set(x => x.ActiveAgentSchedulingGroupId, agentScheduleDetails.ActiveAgentSchedulingGroupId);
                }

                UpdateOneAsync(query, update, new UpdateOptions
                {
                    IsUpsert = true
                });
            });
        }

    }
}
