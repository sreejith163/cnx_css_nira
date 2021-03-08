using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Css.Api.Scheduling.Models.DTO.Response.EntityHierarchy
{
    public class AgentSchedulingGroupDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client reference identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ClientRefId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client lob identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ClientLOBId { get; set; }

        /// <summary>
        /// Gets or sets the client lob reference identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ClientLOBRefId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client lob.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ClientLOBName { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group reference identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SkillGroupRefId { get; set; }

        /// <summary>
        /// Gets or sets the name of the skill group.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SkillGroupName { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag reference identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SkillTagRefId { get; set; }

        /// <summary>
        /// Gets or sets the name of the skill tag.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SkillTagName { get; set; }
    }
}
