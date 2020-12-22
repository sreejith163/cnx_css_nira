using Css.Api.Scheduling.Models.Domain;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentScheduleChart : AgentScheduleChartAttributes
    {
        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager.
        /// </summary>
        public List<AgentScheduleManagerChart> AgentScheduleManagerCharts { get; set; }
    }
}