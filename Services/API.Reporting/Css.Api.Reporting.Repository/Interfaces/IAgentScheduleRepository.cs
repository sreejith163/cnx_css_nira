using Css.Api.Core.Models.Domain.NoSQL;
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
        /// <param name="reportDate">The requested date for which schedule is to be picked</param>
        /// <returns>The list of instances of AgentSchedule</returns>
        Task<List<AgentSchedule>> GetSchedules(DateTime reportDate);

        /// <summary>
        /// The method to fetch schedules for input employees
        /// </summary>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        Task<List<AgentSchedule>> GetSchedules(List<int> employeeIds);

        // <summary>
        /// The method to insert schedules for agents if it doesn't exist
        /// </summary>
        /// <param name="agentSchedules">The agents schedules to be updated</param>
        void InsertAgentSchedules(List<AgentSchedule> agentSchedules);
    }
}
