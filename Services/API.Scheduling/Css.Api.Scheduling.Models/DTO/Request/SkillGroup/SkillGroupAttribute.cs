using Css.Api.Scheduling.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.SkillGroup
{
    public class SkillGroupAttribute
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the client lob group identifier.</summary>
        /// <value>The client lob group identifier.</value>
        public int ClientLobGroupId { get; set; }

        /// <summary>Gets or sets the timezone identifier.</summary>
        /// <value>The timezone identifier.</value>
        public int TimezoneId { get; set; }

        /// <summary>Gets or sets the first day of week.</summary>
        /// <value>The first day of week.</value>
        public int FirstDayOfWeek { get; set; }

        /// <summary>Gets or sets the operation hours.</summary>
        /// <value>The operation hours.</value>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}
