using Css.Api.SetupMenu.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.SetupMenu.Models.DTO.Response.SkillTag
{
    public class SkillTagDetailsDTO : SkillTagDTO
    {
        /// <summary>
        /// Gets or sets the operation hour.
        /// </summary>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}
