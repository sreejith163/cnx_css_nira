using Css.Api.Core.Models.Enums;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class UpdateAgentScheduleManager
    {
        /// <summary>
        /// Gets or sets the agent schedule managers.
        /// </summary>
        public List<UpdateScheduleManagerChart> ScheduleManagers { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is import.
        /// </summary>
        public bool IsImport { get; set; }

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
