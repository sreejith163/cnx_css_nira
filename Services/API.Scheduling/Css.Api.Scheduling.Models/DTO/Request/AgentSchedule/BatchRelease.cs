using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class BatchRelease
    {
        public List<BatchReleaseDetails> BatchReleaseDetails { get; set; }
     
        
        /// <summary>
        /// Gets or sets the modified user.
        /// </summary>
        public string ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
