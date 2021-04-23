using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView
{
    public class AgentSchedulingGroupList
    {
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the agents scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

   
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

   
    }
}
