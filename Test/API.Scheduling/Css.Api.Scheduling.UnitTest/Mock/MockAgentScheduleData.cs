using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using System.Net;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockAgentScheduleData
    {
        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public CSSResponse GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = new MockDataContext().GetAgentSchedules(agentScheduleQueryparameter);
            return new CSSResponse(agentSchedules, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public CSSResponse GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentProfile = new MockDataContext().GetAgentAdminIdsByEmployeeId(new EmployeeIdDetails { Id = agentSchedule.EmployeeId });
            return new CSSResponse(agentProfile, HttpStatusCode.OK);
        }

        public CSSResponse GetAgentScheduleCharts(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentScheduleChartQueryparameter.AgentScheduleType == AgentScheduleType.SchedulingTab)
            {
                var mappedAgentScheduleChart = new AgentScheduleChartDetailsDTO {
                    Id = agentSchedule.Id.ToString(),
                    AgentScheduleCharts = agentSchedule.AgentScheduleCharts
                };
                return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
            }
            else
            {
                var mappedAgentScheduleChart = new AgentScheduleManagerChartDetailsDTO
                {
                    Id = agentSchedule.Id.ToString(),
                    AgentScheduleManagerCharts = agentSchedule.AgentScheduleManagerCharts
                };
                return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentScheduleIdDetails"></param>
        /// <param name="agentScheduleDetails"></param>
        /// <returns></returns>
        public CSSResponse UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            var agentScheduleCount = new MockDataContext().GetAgentScheduleCount(agentScheduleIdDetails);
            if (agentScheduleCount < 1)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            new MockDataContext().UpdateAgentSchedule(agentScheduleIdDetails, agentScheduleDetails);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails)
        {
            var agentScheduleCount = new MockDataContext().GetAgentScheduleCount(agentScheduleIdDetails);
            if (agentScheduleCount < 1)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            new MockDataContext().UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleDetails);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse ImportAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart agentScheduleDetails)
        {
            var agentScheduleCount = new MockDataContext().GetAgentScheduleCount(agentScheduleIdDetails);
            if (agentScheduleCount < 1)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            new MockDataContext().ImportAgentScheduleChart(agentScheduleIdDetails, agentScheduleDetails);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse CopyAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            new MockDataContext().CopyAgentSchedules(agentSchedule, agentScheduleDetails);

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
