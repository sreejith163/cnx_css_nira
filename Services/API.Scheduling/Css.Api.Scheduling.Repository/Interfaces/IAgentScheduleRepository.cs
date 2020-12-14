using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
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
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentScheduleByEmployeeId(AgentAdminEmployeeIdDetails agentAdminEmployeeIdDetails);

        /// <summary>
        /// Gets the agent schedules by employee ids.
        /// </summary>
        /// <param name="employeeIds">The employee ids.</param>
        /// <returns></returns>
        Task<List<AgentSchedule>> GetAgentSchedulesByEmployeeIds(List<string> employeeIds);

        /// <summary>
        /// Creates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        void CreateAgentSchedule(AgentSchedule agentScheduleRequest);

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        void UpdateAgentSchedule(AgentSchedule agentScheduleRequest);

        /// <summary>
        /// Initializes a new instance of the <see cref="IAgentScheduleRepository" /> interface.
        /// </summary>
        /// <param name="agentSchedule">The agent schedule.</param>
        /// <param name="copyAgentScheduleRequest">The copy agent schedule request.</param>
        void BulkUpdateAgentScheduleCharts(AgentSchedule agentSchedule, CopyAgentSchedule copyAgentScheduleRequest);

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        void DeleteAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails);
    }
}

