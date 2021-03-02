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
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        Task<AgentScheduleManager> GetAgentScheduleManagerChart(DateDetails dateDetails);

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        Task<AgentScheduleManager> GetAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails);

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
        /// <param name="agentScheduleManager">The agent schedule manager.</param>
        void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManager agentScheduleManager);

        /// <summary>
        /// Copies the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManager">The agent schedule manager.</param>
        void CopyAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManager agentScheduleManager);
    }
}

