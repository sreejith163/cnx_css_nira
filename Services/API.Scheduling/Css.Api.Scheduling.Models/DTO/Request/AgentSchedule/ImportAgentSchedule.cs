using Css.Api.Core.Models.Enums;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentSchedule
    {
        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ImportAgentScheduleChart> ImportAgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the modified user.
        /// </summary>
        public int ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}