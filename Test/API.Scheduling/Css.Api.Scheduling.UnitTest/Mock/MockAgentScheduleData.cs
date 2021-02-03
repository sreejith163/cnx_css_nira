using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Css.Api.Scheduling.Models.Domain;
using System;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockAgentScheduleData
    {
        List<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
            new SchedulingCode { Id = new ObjectId("5fe0b5ad6a05416894c0718d"), SchedulingCodeId = 1, Name = "lunch", IsDeleted = false},
            new SchedulingCode { Id = new ObjectId("5fe0b5c46a05416894c0718f"), SchedulingCodeId = 2, Name = "lunch", IsDeleted = false},
        };

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

            var hasValidCodes = HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            new MockDataContext().UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleDetails);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleManagerChartDetails">The agent schedule manager chart details.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentScheduleMangerChart(UpdateAgentScheduleManagerChart agentScheduleManagerChartDetails)
        {
            var hasValidCodes = HasValidSchedulingCodes(agentScheduleManagerChartDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            foreach (var agentScheduleManager in agentScheduleManagerChartDetails.AgentScheduleManagers)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = agentScheduleManager.EmployeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleManagerChartDetails.ModifiedBy };
                new MockDataContext().UpdateAgentScheduleMangerChart(employeeIdDetails, agentScheduleManager.AgentScheduleManagerChart, modifiedUserDetails);
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse ImportAgentScheduleChart(ImportAgentSchedule agentScheduleDetails)
        {
            var hasValidCodes = HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            new MockDataContext().ImportAgentScheduleChart(agentScheduleDetails);

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

        /// <summary>
        /// Gets the activity log for scheduling chart.
        /// </summary>
        /// <param name="scheduleCharts">The schedule charts.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingChart(object scheduleCharts, int employeeId, string executedBy, ActivityOrigin activityOrigin)
        {
            var activityLog = new ActivityLog()
            {
                EmployeeId = employeeId,
                ExecutedBy = executedBy,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = activityOrigin,
                ActivityStatus = ActivityStatus.Updated,
                SchedulingFieldDetails = new SchedulingFieldDetails()
            };

            if (scheduleCharts is List<AgentScheduleChart>)
            {
                var charts = scheduleCharts as List<AgentScheduleChart>;
                activityLog.ActivityType = ActivityType.SchedulingGrid;
                activityLog.SchedulingFieldDetails.AgentScheduleCharts = charts;
            }
            else if (scheduleCharts is List<AgentScheduleManagerChart>)
            {
                var charts = scheduleCharts as List<AgentScheduleManagerChart>;
                activityLog.ActivityType = ActivityType.SchedulingmanagerGrid;
                activityLog.SchedulingFieldDetails.AgentScheduleManagerCharts = charts;
            }
            else if (scheduleCharts is AgentScheduleManagerChart)
            {
                var chart = scheduleCharts as AgentScheduleManagerChart;
                activityLog.ActivityType = ActivityType.SchedulingmanagerGrid;
                activityLog.SchedulingFieldDetails.AgentScheduleManagerCharts = new List<AgentScheduleManagerChart>
                {
                    chart
                };
            }

            return activityLog;
        }

        /// <summary>
        /// Determines whether [has valid scheduling codes] [the specified agent schedule details].
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns>
        ///   <c>true</c> if [has valid scheduling codes] [the specified agent schedule details]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasValidSchedulingCodes(object agentScheduleDetails)
        {
            bool isValid = true;
            List<int> codes = new List<int>();

            if (agentScheduleDetails is UpdateAgentScheduleChart)
            {
                var details = agentScheduleDetails as UpdateAgentScheduleChart;
                foreach (var agentScheduleChart in details.AgentScheduleCharts)
                {
                    var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                    codes.AddRange(scheduleCodes);
                }
            }
            else if (agentScheduleDetails is UpdateAgentScheduleManagerChart)
            {
                var details = agentScheduleDetails as UpdateAgentScheduleManagerChart;
                foreach (var agentScheduleManager in details.AgentScheduleManagers)
                {
                    var scheduleManagerCodes = agentScheduleManager.AgentScheduleManagerChart.Charts.Select(x => x.SchedulingCodeId).ToList().ToList();
                    codes.AddRange(scheduleManagerCodes);
                }
            }
            else if (agentScheduleDetails is ImportAgentSchedule)
            {

                var details = agentScheduleDetails as ImportAgentSchedule;
                foreach (var agentScheduleChart in details.AgentScheduleCharts)
                {
                    var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                    codes.AddRange(scheduleCodes);
                }
            }

            if (codes.Any())
            {
                var schedulingCodesCount = schedulingCodesDB.FindAll(x => x.IsDeleted == false && codes.Contains(x.SchedulingCodeId))?.Count();
                if (schedulingCodesCount != codes.Count())
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
