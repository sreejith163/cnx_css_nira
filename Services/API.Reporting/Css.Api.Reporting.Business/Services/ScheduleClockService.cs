using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The schedule clock helper service
    /// </summary>
    public class ScheduleClockService : IScheduleClockService
    {
        #region Public Methods

        /// <summary>
        /// The method to generate instances of clocks based on the schedules for a date
        /// </summary>
        /// <param name="reportDate">The date for which the clock has to generated</param>
        /// <param name="schedules">The agent schedules</param>
        /// <param name="codes">The scheduling codes</param>
        /// <returns>The list of instances of ScheduleClock</returns>
        public List<ScheduleClock> GenerateClocks(DateTime reportDate, List<AgentSchedule> schedules, List<SchedulingCode> codes)
        {
            List<ScheduleClock> clocks = new List<ScheduleClock>();
            //int dayOfWeek = (int)reportDate.DayOfWeek;
            //var agentsChartForDay = schedules
            //                    .Where(x => x.AgentScheduleCharts.Any(y => y.Day == dayOfWeek))
            //                    .Select(y => new
            //                    {
            //                        Id = y.EmployeeId,
            //                        Charts = y.AgentScheduleCharts.FirstOrDefault(z => z.Day == dayOfWeek).Charts
            //                    }).ToList();

            //agentsChartForDay.ForEach(x =>
            //{
            //    clocks.AddRange(
            //        (from c in x.Charts
            //            join sc in codes on c.SchedulingCodeId equals sc.SchedulingCodeId
            //            select new ScheduleClock
            //            {
            //                Date = reportDate.Date,
            //                ActivityId = c.SchedulingCodeId,
            //                EmployeeId = x.Id,
            //                ActivityName = sc.Name,
            //                FromTime = c.StartTime,
            //                ToTime = c.EndTime
            //            }).ToList()
            //    );
            //});

            return clocks;
        }

        /// <summary>
        /// The method to generate a text string for the clocks
        /// </summary>
        /// <param name="clocks">The list of instances of ScheduleClock</param>
        /// <returns>The text string</returns>
        public string GenerateClocksText(List<ScheduleClock> clocks)
        {
            string clockText = "";
            clocks.ForEach(x =>
            {
                clockText += string.Join('|', x.Date.ToString("yyyyMMdd"), x.Date.ToString("yyyyMMdd"), x.EmployeeId, x.ActivityName, x.FromTime, x.ToTime) + "\n";
            });
            return clockText;
        }
        #endregion
    }
}
