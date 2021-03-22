
using System.Collections.Generic;
using Css.Api.Core.Models.Enums;


namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleImport
    {
        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleImportData> AgentScheduleImportData { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

    }
}
