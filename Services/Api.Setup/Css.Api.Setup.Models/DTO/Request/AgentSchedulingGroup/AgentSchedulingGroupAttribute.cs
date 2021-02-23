using Css.Api.Setup.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup
{
    public class AgentSchedulingGroupAttribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the skill group identifier.</summary>
        public int SkillTagId { get; set; }

        /// <summary>Gets or sets the timezone identifier.</summary>
        /// <value>The timezone identifier.</value>
        public int TimezoneId { get; set; }

        /// <summary>Gets or sets the first day of week.</summary>
        /// <value>The first day of week.</value>
        public int FirstDayOfWeek { get; set; }


        public bool EstartProvision { get; set; }
        /// <summary>
        /// Gets or sets the operation hour.
        /// </summary>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}