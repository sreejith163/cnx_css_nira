using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgentScheduleManager = Css.Api.Core.Models.Domain.NoSQL.AgentScheduleManager;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Agent schedule manager
    /// </summary>
    public interface IAgentScheduleManagerRepository
    {
        /// <summary>
        /// Gets the agent schedule manager charts.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter);

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="agentScheduleManagerIdDetails">The agent schedule manager identifier details.</param>
        /// <returns></returns>
        Task<AgentScheduleManager> GetAgentScheduleManagerChart(AgentScheduleManagerIdDetails agentScheduleManagerIdDetails);

        /// <summary>
        /// Gets the agent schedule manager chart by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        Task<AgentScheduleManager> GetAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails);

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<List<int>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Creates the agent schedule manager.
        /// </summary>
        /// <param name="agentScheduleManagerRequest">The agent schedule manager request.</param>
        void CreateAgentScheduleManager(AgentScheduleManager agentScheduleManagerRequest);

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManagerChart agentScheduleManagerChart);

        /// <summary>
        /// Copies the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        void CopyAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManagerChart agentScheduleManagerChart);

        /// <summary>
        /// Deletes the agent schedule manager.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        void DeleteAgentScheduleManager(EmployeeIdDetails employeeIdDetails);
    }
}

