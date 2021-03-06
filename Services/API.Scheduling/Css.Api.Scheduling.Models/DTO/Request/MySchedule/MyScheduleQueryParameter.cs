﻿using System;

namespace Css.Api.Scheduling.Models.DTO.Request.MySchedule
{
    public class MyScheduleQueryParameter
    {
        /// <summary>Gets or sets the start date.</summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the end date.</summary>
        /// <value>The end date.</value>
        public DateTime EndDate { get; set; }

        /// <summary>Gets or sets the agent scheduling group identifier.</summary>
        /// <value>The agent scheduling group identifier.</value>
        public int AgentSchedulingGroupId { get; set; }
    }
}