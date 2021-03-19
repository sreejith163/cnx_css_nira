using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Common;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// An interface for the schedule clock helper service
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// The method to generate instances of calendar charts using the filter params
        /// </summary>
        /// <param name="filter">The filters to be applied</param>
        /// <returns>The list of instances of CalendarChart</returns>
        Task<List<CalendarChart>> GetCalendarCharts(ScheduleFilter filter);

        /// <summary>
        /// The method to generate instances of calendar charts using the agent ids and other filter params
        /// </summary>
        /// <param name="filter">The filters to be applied</param>
        /// <returns>The list of instances of CalendarChart</returns>
        Task<List<CalendarChart>> GetCalendarChartsUsingIds(ScheduleFilter filter);

        /// <summary>
        /// The method to parse the clocks text to instances of calender charts
        /// </summary>
        /// <param name="clockText"></param>
        /// <param name="invalidData"></param>
        /// <param name="estartProvisioning"></param>
        /// <returns>A list of instances of ScheduleClock. All unparsed/invalid data is added to the 'invalidData' parameter</returns>
        List<CalendarChart> ParseCalenderCharts(string chartsText, out List<string> invalidData, bool? estartProvisioning = null);

        /// <summary>
        /// The method to generate a text string for the charts
        /// </summary>
        /// <param name="charts">The list of instances of CalendarChart</param>
        /// <returns>The text string</returns>
        string GenerateExportText(List<CalendarChart> charts);

        /// <summary>
        /// A helper method to parse the schedule manager details from the input schedule clocks
        /// </summary>
        /// <param name="charts"></param>
        /// <param name="activityInformation"></param>
        /// <returns>The list of instances of AgentScheduleManager</returns>
        Task<List<ScheduleManagerDetails>> GenerateAgentScheduleManagerCharts(List<CalendarChart> charts, ActivityInformation activityInformation);

        /// <summary>
        /// The method to generate activity agent schedules from the calendar charts
        /// </summary>
        /// <param name="calendarCharts"></param>
        /// <returns>The list of instances of AgentActivitySchedule</returns>
        List<AgentActivitySchedule> GenerateActivityAgentSchedules(List<CalendarChart> calendarCharts);

        /// <summary>
        /// A helper method to parse activity schedules
        /// </summary>
        /// <param name="activityScheduleUpdate"></param>
        /// <returns>An instance of ScheduleData</returns>
        Task<ScheduleData> ParseActivitySchedule(ActivityScheduleUpdate activityScheduleUpdate);
    }
}
