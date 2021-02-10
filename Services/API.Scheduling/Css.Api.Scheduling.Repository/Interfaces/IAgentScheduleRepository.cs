using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Agent schedule
    /// </summary>
    public interface IAgentScheduleRepository
    {
        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter);

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the agent schedule count.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<long> GetAgentScheduleCount(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the employee identifier by agent schedule identifier.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<int> GetEmployeeIdByAgentScheduleId(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<List<int>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentScheduleByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails);

        /// <summary>
        /// Gets the agent schedule count by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        Task<long> GetAgentScheduleCountByEmployeeId(EmployeeIdDetails employeeIdDetails);

        /// <summary>
        /// Creates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        void CreateAgentSchedule(AgentSchedule agentScheduleRequest);

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        void UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails);

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleEmployeeDetails">The update agent schedule employee details.</param>
        void UpdateAgentSchedule(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails);

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleCharts">The agent schedule charts.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        void UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, List<AgentScheduleChart> agentScheduleCharts,
                                      ModifiedUserDetails modifiedUserDetails);

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManagerChart agentScheduleManagerChart, ModifiedUserDetails modifiedUserDetails);

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        void ImportAgentScheduleChart(ImportAgentScheduleChart agentScheduleDetails, ModifiedUserDetails modifiedUserDetails);

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="agentSchedule">The agent schedule.</param>
        /// <param name="copyAgentScheduleRequest">The copy agent schedule request.</param>
        void CopyAgentSchedules(AgentSchedule agentSchedule, CopyAgentSchedule copyAgentScheduleRequest);

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        void DeleteAgentSchedule(EmployeeIdDetails employeeIdDetails);
    }
}

