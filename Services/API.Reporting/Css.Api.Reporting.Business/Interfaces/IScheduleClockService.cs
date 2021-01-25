using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// An interface for the schedule clock helper service
    /// </summary>
    public interface IScheduleClockService
    {
        /// <summary>
        /// The method to generate instances of clocks based on the schedules for a date
        /// </summary>
        /// <param name="reportDate">The date for which the clock has to generated</param>
        /// <param name="schedules">The agent schedules</param>
        /// <param name="codes">The scheduling codes</param>
        /// <returns>The list of instances of ScheduleClock</returns>
        List<ScheduleClock> GenerateClocks(DateTime reportDate, List<AgentSchedule> schedules, List<SchedulingCode> codes);

        /// <summary>
        /// The method to generate a text string for the clocks
        /// </summary>
        /// <param name="clocks">The list of instances of ScheduleClock</param>
        /// <returns>The text string</returns>
        string GenerateClocksText(List<ScheduleClock> clocks);
    }
}
