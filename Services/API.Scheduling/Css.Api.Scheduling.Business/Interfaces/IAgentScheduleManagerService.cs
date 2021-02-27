using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IAgentScheduleManagerService
    {
        /// <summary>
        /// Gets the agent schedule manager charts.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter);

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="agentScheduleManagerIdDetails">The agent schedule manager identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentScheduleManagerChart(AgentScheduleManagerIdDetails agentScheduleManagerIdDetails);

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleManagerChartDetails">The agent schedule manager chart details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentScheduleMangerChart(UpdateAgentScheduleManagerChart agentScheduleManagerChartDetails);

        /// <summary>
        /// Copies the agent schedule manager chart.
        /// </summary>
        /// <param name="agentScheduleManagerIdDetails">The agent schedule manager identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        Task<CSSResponse> CopyAgentScheduleManagerChart(AgentScheduleManagerIdDetails agentScheduleManagerIdDetails, CopyAgentScheduleManager agentScheduleDetails);
    }
}
