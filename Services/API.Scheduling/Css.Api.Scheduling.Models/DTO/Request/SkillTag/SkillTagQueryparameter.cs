using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.SkillTag
{
    public class SkillTagQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTagQueryparameter"/> class.
        /// </summary>
        public SkillTagQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
