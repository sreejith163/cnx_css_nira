using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.SkillGroup
{
    public class SkillGroupQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillGroupQueryparameter"/> class.
        /// </summary>
        public SkillGroupQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
