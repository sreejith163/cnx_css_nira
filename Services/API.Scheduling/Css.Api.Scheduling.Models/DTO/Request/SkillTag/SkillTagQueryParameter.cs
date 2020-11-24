using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.SkillTag
{
    public class SkillTagQueryParameter : QueryStringParameters
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTagQueryParameter"/> class.
        /// </summary>
        public SkillTagQueryParameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        /// <value>
        /// The skill group identifier.
        /// </value>
        public int? SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        /// <value>
        /// The client lob group identifier.
        /// </value>
        public int? ClientLobGroupId { get; set; }
    }
}