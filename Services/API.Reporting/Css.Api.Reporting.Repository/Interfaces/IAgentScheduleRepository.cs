using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent schedule repository
    /// </summary>
    public interface IAgentScheduleRepository
    {
        /// <summary>
        /// The method to fetch schedules for all agent for a date
        /// </summary>
        /// <param name="filter">The filter condition based on which schedules are to be retrieved</param>
        /// <returns>The list of instances of AgentSchedule</returns>
        Task<List<AgentSchedule>> GetSchedules(ScheduleFilter filter);

        /// <summary>
        /// The method to fetch schedules for input employees
        /// </summary>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        Task<List<AgentSchedule>> GetSchedules(List<int> agentIds);

        // <summary>
        /// The method to insert schedules for agents if it doesn't exist
        /// </summary>
        /// <param name="agentSchedules">The agents schedules to be updated</param>
        void InsertAgentSchedules(List<AgentSchedule> agentSchedules);

    }
}
