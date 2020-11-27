using Css.Api.Setup.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup
{
    public class AgentSchedulingGroupDetailsDTO : AgentSchedulingGroupDTO
    {
        /// <summary>
        /// Gets or sets the operation hour.
        /// </summary>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}

