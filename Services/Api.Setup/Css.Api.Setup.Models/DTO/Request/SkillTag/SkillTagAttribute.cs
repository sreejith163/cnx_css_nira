using Css.Api.Setup.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.DTO.Request.SkillTag
{
    public class SkillTagAttribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the operation hour.
        /// </summary>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}