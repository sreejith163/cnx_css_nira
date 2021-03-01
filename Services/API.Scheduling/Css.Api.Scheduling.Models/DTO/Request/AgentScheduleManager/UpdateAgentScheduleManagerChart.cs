﻿using Css.Api.Scheduling.Models.Enums;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class UpdateAgentScheduleManagerChart
    {
        /// <summary>
        /// Gets or sets the agent schedule manager.
        /// </summary>
        /// <value>
        /// The agent schedule manager.
        /// </value>
        public List<AgentScheduleManagerDTO> AgentScheduleManagers { get; set; }

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