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
        /// <param name="reportDate">The requested date for which schedule is to be picked</param>
        /// <returns>The list of instances of AgentSchedule</returns>
        public async Task<List<AgentSchedule>> GetSchedules(DateTime reportDate)
        {
            var query = Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<AgentSchedule>.Filter.Lte(i => i.DateFrom, reportDate) &
                Builders<AgentSchedule>.Filter.Gte(i => i.DateTo, reportDate) &
                Builders<AgentSchedule>.Filter.Where(i => i.AgentScheduleCharts.Any(x => x.Day == (int) reportDate.DayOfWeek));

            var schedules = FilterBy(query);

            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// The method to fetch schedules for input employees
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        public async Task<List<AgentSchedule>> GetSchedules(List<int> employeeIds)
        {
            var query = Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<AgentSchedule>.Filter.Where(i => employeeIds.Contains(i.EmployeeId));

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
                    .SetOnInsert(x => x.DateFrom, agentScheduleDetails.DateFrom)
                    .SetOnInsert(x => x.DateTo, agentScheduleDetails.DateTo)
                    .SetOnInsert(x => x.Status, agentScheduleDetails.Status)
                    .SetOnInsert(x => x.CreatedBy, agentScheduleDetails.CreatedBy)
                    .SetOnInsert(x => x.CreatedDate, agentScheduleDetails.CreatedDate)
                    .SetOnInsert(x => x.AgentScheduleCharts, agentScheduleDetails.AgentScheduleCharts)
                    .SetOnInsert(x => x.AgentScheduleManagerCharts, agentScheduleDetails.AgentScheduleManagerCharts)
                    .Set(x => x.ModifiedBy, agentScheduleDetails.ModifiedBy)
                    .Set(x => x.ModifiedDate, agentScheduleDetails.ModifiedDate);

                if (agentScheduleDetails.AgentSchedulingGroupId > 0)
                {
                    update = update.Set(x => x.AgentSchedulingGroupId, agentScheduleDetails.AgentSchedulingGroupId);
                }

                UpdateOneAsync(query, update, new UpdateOptions
                {
                    IsUpsert = true
                });
            });
        }
    }
}
