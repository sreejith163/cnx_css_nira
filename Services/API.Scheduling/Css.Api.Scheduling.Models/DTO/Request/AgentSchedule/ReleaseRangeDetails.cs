using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
  public  class ReleaseRangeDetails
    {

        public int AgentSchedulingGroupId { get; set; }
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTime DateTo { get; set; }
    }
}
