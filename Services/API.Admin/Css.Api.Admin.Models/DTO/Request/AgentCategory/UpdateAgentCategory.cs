using System;

namespace Css.Api.Admin.Models.DTO.Request.AgentCategory
{
    public class UpdateAgentCategory : AgentCategoryAttribute
    {
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is update revert.
        /// </summary>
        public bool IsUpdateRevert { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}