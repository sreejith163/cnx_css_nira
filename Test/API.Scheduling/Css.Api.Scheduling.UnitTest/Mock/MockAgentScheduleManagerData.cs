using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Core.Models.Domain;
using Newtonsoft.Json;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Response.MySchedule;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockAgentScheduleManagerData
    {
        readonly List<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
            new SchedulingCode { Id = new ObjectId("5fe0b5ad6a05416894c0718d"), SchedulingCodeId = 1, Name = "lunch", IsDeleted = false},
            new SchedulingCode { Id = new ObjectId("5fe0b5c46a05416894c0718f"), SchedulingCodeId = 2, Name = "lunch", IsDeleted = false},
        };

        /// <summary>
        /// Gets the agent schedule manager charts.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        public CSSResponse GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            var agents = GetAgents(agentScheduleManagerChartQueryparameter);
            var mappedAgents = JsonConvert.DeserializeObject<List<AgentAdminDTO>>(JsonConvert.SerializeObject(agents));

            var agentScheduleManagers = new MockDataContext().GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter);

            var mappedAgentScheduleManagers = JsonConvert.DeserializeObject<List<AgentScheduleManagerChartDetailsDTO>>(JsonConvert.SerializeObject(agentScheduleManagers));

            foreach (var agent in mappedAgents)
            {
                var mappedAgentScheduleManager = mappedAgentScheduleManagers.FirstOrDefault(x => x.EmployeeId == agent.EmployeeId);
                if (mappedAgentScheduleManager == null)
                {
                    var scheduleManagerExists = new MockDataContext().HasAgentScheduleManagerChartByEmployeeId(new EmployeeIdDetails { Id = agent.EmployeeId });
                    if (!scheduleManagerExists || !agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
                    {
                        var agentScheduleManager = new AgentScheduleManagerChartDetailsDTO
                        {
                            EmployeeId = agent.EmployeeId,
                            FirstName = agent.FirstName,
                            LastName = agent.LastName,
                            AgentSchedulingGroupId = agent.AgentSchedulingGroupId,
                        };
                        mappedAgentScheduleManagers.Add(agentScheduleManager);
                    }
                }
                else
                {
                    mappedAgentScheduleManager.FirstName = agent?.FirstName;
                    mappedAgentScheduleManager.LastName = agent?.LastName;
                }
            }

            mappedAgentScheduleManagers = mappedAgentScheduleManagers.Where(x => x.FirstName != null && x.LastName != null).ToList();

            return new CSSResponse(mappedAgentScheduleManagers.Distinct().ToList(), HttpStatusCode.OK);
        }



        /// <summary>
        /// Gets the agent my schedule.
        /// </summary>
        /// <param name="skillGroupId">The employee identifier details.</param>
        /// <param name="date"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentScheduledOpen(int skillGroupId, DateTimeOffset date)
        {
            var skillGroupIdDetails = new SkillGroupIdDetails { SkillGroupId = skillGroupId };
            var agentSchedulingGroup = new MockDataContext().GetAgentSchedulingGroupBySkillGroupId(skillGroupIdDetails);

            var agentSchedulingGroupId = new List<int>();

            foreach (var id in agentSchedulingGroup)
            {
                agentSchedulingGroupId.Add(id.AgentSchedulingGroupId);
            }

            if (agentSchedulingGroupId == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var dateTimeWithZeroTimeSpan = new DateTimeOffset(date.Date, TimeSpan.Zero);
            var agentSchedule = new MockDataContext().GetAgentScheduleByAgentSchedulingGroupId(agentSchedulingGroupId, dateTimeWithZeroTimeSpan);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            int[] numbers = { 3, 15, 16, 17, 20, 21, 22, 173, 174, 175, 176, 177 };
            foreach (var item1 in agentSchedule)
            {
                item1.Charts = item1.Charts.Where(p => numbers.Contains(p.SchedulingCodeId)).ToList();
            }

            int minutes = 30;
            var interval = new TimeSpan(0, minutes, 0);
            var schedOpen = agentSchedule.SelectMany(x => x.Charts).ToList();

            agentSchedule.RemoveAll(x => x.Charts.Count() == 0);

            var myTimeList = new List<ScheduleOpen>();
            var timeList = new List<TimeSpan>();

            foreach (var chart in schedOpen)
            {
                TimeSpan start = chart.StartDateTime.TimeOfDay;
                TimeSpan end = chart.EndDateTime.TimeOfDay;

                var diffSpan = end - start;
                var diffMinutes = diffSpan.TotalMinutes > 0 ? diffSpan.TotalMinutes : diffSpan.TotalMinutes + (60 * 24);
                for (int i = 0; i < diffMinutes + minutes; i += minutes)
                {
                    var scheduleOpen = new ScheduleOpen { Time = start };
                    myTimeList.Add(scheduleOpen);
                    timeList.Add(start);
                    start = start.Add(interval);
                }
            }

            var groupedTime = myTimeList
                .GroupBy(x => x.Time)
                .Select(
                    group => new
                    {
                        time = RoundToNearestMinutes(group.Key, 30),
                        scheduleOpen = group.Count()
                    }
                );

            if (groupedTime.Count() == 0)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(groupedTime, HttpStatusCode.OK);
        }

        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentMySchedule(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            var agentSchedules = new MockDataContext().GetAgentScheduleManagerChartByEmployeeId(employeeIdDetails, myScheduleQueryParameter);
            if (agentSchedules == null || agentSchedules.Count < 1)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentMyScheduleDetailsDTO = new AgentMyScheduleDetailsDTO
            {
                AgentMySchedules = new List<AgentMyScheduleDay>()
            };

            AgentMyScheduleDay schedule;

            foreach (DateTime date in EachDay(myScheduleQueryParameter.StartDate, myScheduleQueryParameter.EndDate))
            {
                AgentScheduleManager agentSchedule = agentSchedules.Where(s => s.Date == date).FirstOrDefault();
                if (agentSchedule != null)
                {

                    bool isChartAvailableForDay =
                        agentSchedule.Charts.Any();
                    if (isChartAvailableForDay)
                    {
                        var chartsOfDay = agentSchedule.Charts;
                        var firstStartTime = chartsOfDay.Min(chart => chart.StartDateTime);
                        var lastEndTime = chartsOfDay.Max(chart => chart.EndDateTime);

                        schedule = new AgentMyScheduleDay
                        {
                            Day = (int)agentSchedule.Date.DayOfWeek,
                            Date = agentSchedule.Date,
                            Charts = chartsOfDay,
                            FirstStartTime = firstStartTime.ToString(),
                            LastEndTime = lastEndTime.ToString()
                        };
                    }
                    else
                    {
                        schedule = CreateMyScheduleDayWithNoChart(date);
                    }
                }
                else
                {
                    schedule = CreateMyScheduleDayWithNoChart(date);

                }
                agentMyScheduleDetailsDTO.AgentMySchedules.Add(schedule);
            }

            return new CSSResponse(agentMyScheduleDetailsDTO, HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule manager details.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentScheduleMangerChart(UpdateAgentScheduleManager agentScheduleDetails)
        {
            var hasValidCodes = HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var scheduleManager in agentScheduleDetails.ScheduleManagers)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = scheduleManager.EmployeeId };

                foreach (var agentScheduleManagerChart in scheduleManager.AgentScheduleManagerCharts)
                {
                    var dateDetails = new DateDetails { Date = agentScheduleManagerChart.Date };
                    var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                    agentScheduleManagerChart.Date = new DateTime(agentScheduleManagerChart.Date.Year, agentScheduleManagerChart.Date.Month,
                                                                  agentScheduleManagerChart.Date.Day, 0, 0, 0, DateTimeKind.Utc);
                    if (agentScheduleDetails.IsImport)
                    {
                        var scheduleExists = new MockDataContext().IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails);
                        if (scheduleExists)
                        {
                            continue;
                        }
                    }

                    var agentAdmin = new MockDataContext().GetAgentAdminByEmployeeId(employeeIdDetails);
                    if (agentAdmin != null)
                    {
                        var agentScheduleManager = new AgentScheduleManager
                        {
                            EmployeeId = agentAdmin.Ssn,
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            Date = agentScheduleManagerChart.Date,
                            Charts = agentScheduleManagerChart.Charts,
                            CreatedBy = modifiedUserDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        new MockDataContext().UpdateAgentScheduleMangerChart(employeeIdDetails, agentScheduleManager);
                    }
                }
            }


            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse CopyAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, CopyAgentScheduleManager agentScheduleDetails)
        {
            var dateDetails = new DateDetails { Date = agentScheduleDetails.Date };
            var agentScheduleManager = new MockDataContext().GetAgentScheduleManagerChart(employeeIdDetails, dateDetails);
            if (agentScheduleManager == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = new MockDataContext().GetEmployeeIdsByAgentSchedulingGroup(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentScheduleManager.EmployeeId);
            }

            var activityLogs = new List<ActivityLog>();

            agentScheduleDetails.Date = new DateTime(agentScheduleDetails.Date.Year, agentScheduleDetails.Date.Month, agentScheduleDetails.Date.Day,
                                                     0, 0, 0, DateTimeKind.Utc);

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                var employeeDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var scheduleExists = new MockDataContext().IsAgentScheduleManagerChartExists(employeeDetails, dateDetails);
                if (!scheduleExists)
                {
                    var agentAdmin = new MockDataContext().GetAgentAdminByEmployeeId(employeeDetails);
                    if (agentAdmin != null)
                    {
                        var agentScheduleManagerChart = new AgentScheduleManager
                        {
                            EmployeeId = agentAdmin.Ssn,
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            Date = agentScheduleDetails.Date,
                            Charts = agentScheduleManager.Charts,
                            CreatedBy = modifiedUserDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        new MockDataContext().UpdateAgentScheduleMangerChart(employeeDetails, agentScheduleManagerChart);
                    }
                }
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Determines whether [has valid scheduling codes] [the specified manager].
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns>
        ///   <c>true</c> if [has valid scheduling codes] [the specified manager]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasValidSchedulingCodes(UpdateAgentScheduleManager manager)
        {
            bool isValid = true;
            List<int> codes = new List<int>();

            foreach (var agentScheduleManager in manager.ScheduleManagers)
            {
                foreach (var agentScheduleManagerChart in agentScheduleManager.AgentScheduleManagerCharts)
                {
                    var scheduleManagerCodes = agentScheduleManagerChart.Charts.Select(x => x.SchedulingCodeId).ToList().ToList();
                    codes.AddRange(scheduleManagerCodes);
                }
            }

            codes = codes.Distinct().ToList();

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

        /// <summary>
        /// Gets the agents.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        private PagedList<Entity> GetAgents(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            AgentAdminQueryParameter agentAdminQueryParameter = new AgentAdminQueryParameter
            {
                AgentSchedulingGroupId = agentScheduleManagerChartQueryparameter.AgentSchedulingGroupId,
                Fields = "EmployeeId, FirstName, LastName"
            };

            return new MockDataContext().GetAgentAdmins(agentAdminQueryParameter);
        }

        /// <summary>
        /// Eaches the day.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>Creates my schedule day with no chart.</summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private AgentMyScheduleDay CreateMyScheduleDayWithNoChart(DateTime date)
        {
            return new AgentMyScheduleDay
            {
                Day = (int)date.DayOfWeek,
                Date = date
            };
        }

        /// <summary>
        /// Rounds to nearest minutes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        private TimeSpan RoundToNearestMinutes(TimeSpan input, int minutes)
        {
            var totalMinutes = (int)(input + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;

            return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
        }
    }
}
