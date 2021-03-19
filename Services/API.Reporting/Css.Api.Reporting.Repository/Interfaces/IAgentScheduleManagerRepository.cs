using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Request.Common;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent schedule manager repository
    /// </summary>
    public interface IAgentScheduleManagerRepository
    {
        /// <summary>
        /// The method to fetch manager schedules for input employees
        /// </summary>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        Task<List<AgentScheduleManager>> GetManagerSchedules(List<int> agentIds);

        /// <summary>
        /// The method to fetch manager schedules for input employees for the dates
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="dates"></param>
        /// <returns></returns>
        Task<List<AgentScheduleManager>> GetManagerSchedules(List<int> agentIds, List<DateTime> dates);

        /// <summary>
        /// The method to fetch manager schedules for matching filter details
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<AgentScheduleManager>> GetManagerSchedules(ScheduleFilter filter);

        /// <summary>
        /// The method to update the agent scheduling group for the manager charts of an agent
        /// </summary>
        /// <param name="updatedAgentSchedulingGroupDetails"></param>
        void UpdateAgentSchedulingGroupForManagerCharts(UpdatedAgentSchedulingGroupDetails updatedAgentSchedulingGroupDetails);

        /// <summary>
        /// The method to update the agent scheduling group for the respective manager charts for all agents
        /// </summary>
        /// <param name="updatedAgentSchedulingGroupDetailsList"></param>
        void UpdateAgentSchedulingGroupForManagerCharts(List<UpdatedAgentSchedulingGroupDetails> updatedAgentSchedulingGroupDetailsList);

        /// <summary>
        /// The method to insert manager schedules for agents if it doesn't exist
        /// </summary>
        /// <param name="agentScheduleManagers">The agents manager schedules to be updated</param>
        void UpsertAgentScheduleManagers(List<AgentScheduleManager> agentScheduleManagers);
    }
}
