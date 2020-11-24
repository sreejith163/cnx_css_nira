using Css.Api.Setup.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.DTO.Response.SkillTag
{
    public class SkillTagDetailsDTO : SkillTagDTO
    {
        /// <summary>
        /// Gets or sets the operation hour.
        /// </summary>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}
