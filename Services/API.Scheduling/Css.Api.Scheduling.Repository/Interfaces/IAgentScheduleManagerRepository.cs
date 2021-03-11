using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using System;
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
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        Task<AgentScheduleManager> GetAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails);

        /// <summary>
        /// Determines whether [is agent schedule manager chart exists] [the specified employee identifier details].
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="dateDetails">The date details.</param>
        /// <returns></returns>
        Task<bool> IsAgentScheduleManagerChartExists(EmployeeIdDetails employeeIdDetails, DateDetails dateDetails);

        /// <summary>Gets the agent schedule manager chart by employee identifier.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<AgentScheduleManager>> GetAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter);

        /// <summary>
        /// Determines whether [has agent schedule manager chart by employee identifier] [the specified employee identifier details].
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        Task<bool> HasAgentScheduleManagerChartByEmployeeId(EmployeeIdDetails employeeIdDetails);

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<List<int>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Gets the scheduled open identifier by skillgroup id.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<List<AgentScheduleManager>> GetAgentScheduleByAgentSchedulingGroupId(List<int> agentSchedulingGroupIdDetailsList, DateTimeOffset date);

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleManager">The agent schedule manager.</param>
        void UpdateAgentScheduleMangerChart(EmployeeIdDetails employeeIdDetails, AgentScheduleManager agentScheduleManager);

        /// <summary>Updates the agent schedule manager.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleManagerEmployeeDetails">The update agent schedule manager employee details.</param>
        void UpdateAgentScheduleManager(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails);
    }
}

