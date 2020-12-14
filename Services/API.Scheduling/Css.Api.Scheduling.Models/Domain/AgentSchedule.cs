using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("agent_schedule")]
    public class AgentSchedule : BaseDocument
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee.
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateTo { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AgentScheduleManager AgentScheduleManager { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? CreatedDate { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
