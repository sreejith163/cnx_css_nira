﻿using Newtonsoft.Json;
using System;

namespace Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup
{
    public class AgentSchedulingGroupDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client lob group.
        /// </summary>
        public string ClientLobGroupName { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the skill group.
        /// </summary>
        public string SkillGroupName { get; set; }

        /// <summary>Gets or sets the skill tag identifier.</summary>
        /// <value>The skill tag identifier.</value>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the first day of week.
        /// </summary>
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the timezone identifier.
        /// </summary>
        public int TimezoneId { get; set; }

        /// <summary>
        /// Gets or sets the timezone label.
        /// </summary>
        public string TimezoneLabel { get; set; }

        /// <summary>Gets or sets the name of the skill tag.</summary>
        /// <value>The name of the skill tag.</value>
        public string SkillTagName { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

