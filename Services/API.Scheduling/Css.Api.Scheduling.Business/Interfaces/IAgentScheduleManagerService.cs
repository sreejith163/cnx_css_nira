using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using System;
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

        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetAgentMySchedule(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter);

        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="skillGroupId">The employee identifier details.</param>
        /// <param name="DateTimeOffset">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetAgentScheduledOpen(int skillGroupId, DateTimeOffset date);

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule manager details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentScheduleMangerChart(UpdateAgentScheduleManager agentScheduleDetails);

        /// <summary>
        /// Copies the agent schedule manager chart.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        Task<CSSResponse> CopyAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, CopyAgentScheduleManager agentScheduleDetails);
    }
}
