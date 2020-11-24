using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.SkillGroup
{
    public class SkillGroupQueryParameter : QueryStringParameters
    {
        /// <summary>Initializes a new instance of the <see cref="SkillGroupQueryParameter" /> class.</summary>
        public SkillGroupQueryParameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>Gets or sets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public int? ClientId { get; set; }

        /// <summary>Gets or sets the client lob group identifier.</summary>
        /// <value>The client lob group identifier.</value>
        public int? ClientLobGroupId { get; set; }
    }
}
