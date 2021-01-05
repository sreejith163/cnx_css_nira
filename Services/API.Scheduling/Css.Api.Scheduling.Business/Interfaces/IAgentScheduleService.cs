using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IAgentScheduleService
    {
        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter);

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the agent schedule charts.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleChartQueryparameter">The agent schedule chart queryparameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentScheduleCharts(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter);

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agentAdmin identifier details.</param>
        /// <param name="agentAdminDetails">The agentAdmin details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails);

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails);

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        Task<CSSResponse> ImportAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart agentScheduleDetails);

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        Task<CSSResponse> CopyAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails);
    }
}
