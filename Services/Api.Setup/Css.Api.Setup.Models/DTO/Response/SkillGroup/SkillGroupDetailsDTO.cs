using Css.Api.Setup.Models.DTO.Request.OperationHour;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.DTO.Response.SkillGroup
{
    public class SkillGroupDetailsDTO : SkillGroupDTO
    {
        /// <summary>Gets or sets the operation hours.</summary>
        /// <value>The operation hours.</value>
        public List<OperationHourAttribute> OperationHour { get; set; }
    }
}
