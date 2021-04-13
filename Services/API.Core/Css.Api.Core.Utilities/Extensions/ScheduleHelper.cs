using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Css.Api.Core.Utilities.Extensions
{
    /// <summary>
    /// The schedule range helper class
    /// </summary>
    public static class ScheduleHelper
    {
        #region Public Methods

        /// <summary>
        /// A helper method to generate agent schedule ranges
        /// </summary>
        /// <param name="date"></param>
        /// <param name="newAgentSchedulingGroupId"></param>
        /// <param name="ranges"></param>
        /// <returns>The list of regenerated agent schedule ranges</returns>
        public static List<AgentScheduleRange> GenerateAgentScheduleRanges(DateTime date, int newAgentSchedulingGroupId, List<AgentScheduleRange> ranges)
        {
            List<AgentScheduleRange> agentScheduleRanges = new List<AgentScheduleRange>();

            agentScheduleRanges.AddRange(ranges.Where(x => x.DateTo < date).ToList());
            ranges.RemoveAll(x => x.DateTo < date);

            var range = ranges.FirstOrDefault(x => x.DateFrom <= date & x.DateTo >= date);

            if (range != null)
            {
                range.AgentSchedulingGroupId = newAgentSchedulingGroupId;
                ranges.Remove(range);
                agentScheduleRanges.Add(range);
            }

            agentScheduleRanges.AddRange(ranges.Select(x =>
            {
                x.AgentSchedulingGroupId = newAgentSchedulingGroupId;
                return x;
            }).ToList());
            ranges.RemoveAll(x => true);

            return agentScheduleRanges;
        }

        /// <summary>
        /// The helper method to generate manager charts from range
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="range"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<AgentScheduleManager> GenerateAgentScheduleManagers(int employeeId, AgentScheduleRange range, string user)
        {
            var currTimestamp = DateTime.UtcNow;
            var dates = Enumerable.Range(0, 1 + range.DateTo.Date.Subtract(range.DateFrom.Date).Days)
                            .Select(offset => range.DateFrom.Date.AddDays(offset))
                            .ToArray();

            List<ScheduleClock> clocks = new List<ScheduleClock>();

            range.ScheduleCharts.ForEach(chart =>
            {
                var matchingDates = dates.Where(x => chart.Day == (int) x.DayOfWeek).ToList();
                matchingDates.ForEach(date =>
                {
                    clocks.AddRange(chart.Charts.Select(x =>
                    {
                        return new ScheduleClock
                        {
                            EmployeeId = employeeId,
                            ActivityId = x.SchedulingCodeId,
                            CurrentAgentSchedulingGroupId = range.AgentSchedulingGroupId,
                            Date = new DateTime(date.Year, date.Month, date.Day,0,0,0,DateTimeKind.Utc),
                            ScheduledDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc),
                            FromTime = ConvertTo24HrClock(x.StartTime),
                            ToTime = ConvertTo24HrClock(x.EndTime)
                        };
                    }).ToList());
                });
            });

            clocks = clocks.OrderBy(x => x.Date).ThenBy(x => x.FromTime).ToList();
            ReconcileScheduledActivityDates(clocks);

            var charts = GenerateManagerCharts(clocks);
            return charts.Select(x =>{
                x.CreatedBy = user;
                x.CreatedDate = currTimestamp;
                return x; }).ToList();
        }

        /// <summary>
        /// A helper method to generate manager charts using reconciled clocks
        /// </summary>
        /// <param name="reconciledClocks"></param>
        /// <returns>A list of instances of AgentScheduleManager</returns>
        public static List<AgentScheduleManager> GenerateManagerCharts(List<ScheduleClock> reconciledClocks)
        {
            List<AgentScheduleManager> managerCharts = new List<AgentScheduleManager>();
            var scheduleDates = reconciledClocks.Select(x => x.ScheduledDate).Distinct().OrderBy(x => x).ToList();
            scheduleDates.ForEach(date =>
            {
                var empDateClock = reconciledClocks.Where(x => x.ScheduledDate == date).ToList();
                AgentScheduleManager agentScheduleManager = new AgentScheduleManager()
                {
                    AgentSchedulingGroupId = empDateClock.First().CurrentAgentSchedulingGroupId,
                    EmployeeId = empDateClock.First().EmployeeId,
                    Date = date,
                    Charts = new List<AgentScheduleManagerChart>()
                };

                empDateClock.ForEach(clock =>
                {
                    var startDateTime = string.Join(" ", clock.Date.ToString("yyyy-MM-dd"), clock.FromTime);
                    var endDateTime = string.Join(" ", clock.Date.ToString("yyyy-MM-dd"), clock.ToTime);
                    List<string> zeroTimes = new List<string>() { "00:00", "00:00:00" };

                    if (zeroTimes.Contains(clock.ToTime.Trim()) || TimeSpan.ParseExact(clock.ToTime, "g", CultureInfo.InvariantCulture) < TimeSpan.ParseExact(clock.FromTime, "g", CultureInfo.InvariantCulture))
                    {
                        startDateTime = string.Join(" ", clock.Date.AddDays(-1).ToString("yyyy-MM-dd"), clock.FromTime);
                    }

                    agentScheduleManager.Charts.Add(new AgentScheduleManagerChart()
                    {
                        SchedulingCodeId = clock.ActivityId,
                        StartDateTime = DateTime.SpecifyKind(DateTime.Parse(startDateTime), DateTimeKind.Utc),
                        EndDateTime = DateTime.SpecifyKind(DateTime.Parse(endDateTime), DateTimeKind.Utc)
                    });

                });
                managerCharts.Add(agentScheduleManager);
            });

            return managerCharts;
        }

        /// <summary>
        /// A helper method to check if the input chart overlaps any existing schedules chart
        /// </summary>
        /// <param name="managerChart"></param>
        /// <param name="agentScheduleManagers"></param>
        /// <returns></returns>
        public static bool CheckIfOverlapSchedulesExists(AgentScheduleManagerChart managerChart, List<AgentScheduleManager> agentScheduleManagers)
        {
            var dates = Enumerable.Range(0, 1 + (int) managerChart.EndDateTime.Subtract(managerChart.StartDateTime).TotalMinutes / 5 )
                            .Select(offset => managerChart.StartDateTime.Add(new TimeSpan(0,offset * 5,0)))
                            .ToArray();

            foreach(var agentManager in agentScheduleManagers)
            {
                foreach(var agentManagerChart in agentManager.Charts)
                {
                    var mgrDates = Enumerable.Range(0, 1 + (int) agentManagerChart.EndDateTime.Subtract(agentManagerChart.StartDateTime).TotalMinutes / 5)
                            .Select(offset => agentManagerChart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                            .ToArray();
                    if(mgrDates.Any(x => dates.Contains(x)))
                    {
                        return true;
                        
                    }
                }
            }

            return false;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// A helper method to convert timestring to 24 hour string
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns>A time string in 'hh:mm' format</returns>
        private static string ConvertTo24HrClock(string timeString)
        {
            string time24hr;
            var pattern = new Regex(@"(12:\d{2}){1}\s*((am|pm)|(a.m.|p.m.))");
            var matches = pattern.Match(timeString);
            if (matches.Success)
            {
                var splitTime = matches.Groups[1].Value.Split(":");
                if (timeString.Contains("pm"))
                {
                    time24hr = string.Join(":", splitTime[0], splitTime[1]);
                }
                else
                {
                    time24hr = string.Join(":", "00", splitTime[1]);
                }

            }
            else if (timeString.Contains("pm"))
            {
                var splitTime = timeString.Split(":");
                time24hr = string.Join(":", (int.Parse(splitTime[0]) + 12).ToString(), splitTime[1].Replace("pm", ""));
            }
            else
            {
                time24hr = timeString.Replace("am", "");
            }
            return time24hr.Trim();
        }

        /// <summary>
        /// A helper method to reconcile the scheduled dates
        /// </summary>
        /// <param name="clocks"></param>
        private static void ReconcileScheduledActivityDates(List<ScheduleClock> clocks)
        {
            var employees = clocks.Select(x => x.EmployeeId).Distinct().ToList();
            employees.ForEach(emp =>
            {
                var empClocks = clocks.Where(x => x.EmployeeId == emp).ToList();
                var timePairs = empClocks.Skip(1).Zip(empClocks, (second, first) => new[] { first, second }).ToList();
                timePairs.ForEach(cp =>
                {
                    var clock1FromTime = TimeSpan.Parse(cp[0].FromTime);
                    var clock1ToTime = TimeSpan.Parse(cp[0].ToTime);
                    var clock1 = cp[0].Date.Add(TimeSpan.Parse(cp[0].ToTime));
                    var clock2 = cp[1].Date.Add(TimeSpan.Parse(cp[1].FromTime));
                    if (clock1FromTime > clock1ToTime)
                    {
                        clock1 = clock1.AddDays(1);
                        cp[0].Date = cp[0].Date.AddDays(1);
                    }

                    var diff = (clock2 - clock1).TotalMinutes;
                    if (diff < 5)
                    {
                        cp[1].ScheduledDate = cp[0].ScheduledDate;
                    }
                });
            });

            List<ScheduleClock> reconciliedClocks = new List<ScheduleClock>();
            employees.ForEach(emp =>
            {
                var empClocks = clocks.Where(x => x.EmployeeId == emp).ToList();
                var scheduledEmpClockDates = empClocks.Select(x => x.ScheduledDate).Distinct().ToList();
                scheduledEmpClockDates.ForEach(date =>
                {
                    var empDateClocks = empClocks.Where(x => x.ScheduledDate == date).ToList();
                    if (empDateClocks.Count == 1)
                    {
                        reconciliedClocks.AddRange(empDateClocks);
                    }
                    else
                    {
                        var currClock = empDateClocks.First();
                        for (int i = 1; i < empDateClocks.Count; i++)
                        {
                            var clock = empDateClocks[i];
                            if (currClock.ActivityId != clock.ActivityId || (currClock.ActivityId == clock.ActivityId && currClock.ToTime != currClock.FromTime))
                            {
                                reconciliedClocks.Add(currClock);
                                currClock = clock;
                            }
                            else
                            {

                                currClock.ToTime = clock.ToTime;
                                currClock.ScheduledDate = clock.ScheduledDate;
                            }
                            if (i == empDateClocks.Count - 1)
                            {
                                reconciliedClocks.Add(currClock);
                            }

                        }
                    }
                });
            });

            clocks.RemoveAll(x => true);
            clocks.AddRange(reconciliedClocks);
        }

        #endregion
    }
}
