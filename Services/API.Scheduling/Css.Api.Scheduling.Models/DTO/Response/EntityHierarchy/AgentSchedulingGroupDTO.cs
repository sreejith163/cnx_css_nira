using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Css.Api.Scheduling.Models.DTO.Response.EntityHierarchy
{
    public class AgentSchedulingGroupDTO
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>Gets or sets the name of the client.</summary>
        /// <value>The name of the client.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ClientName { get; set; }

        /// <summary>Gets or sets the name of the client lob.</summary>
        /// <value>The name of the client lob.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ClientLOBName { get; set; }

        /// <summary>Gets or sets the name of the skill group.</summary>
        /// <value>The name of the skill group.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SkillGroupName { get; set; }

        /// <summary>Gets or sets the name of the skill tag.</summary>
        /// <value>The name of the skill tag.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SkillTagName { get; set; }
    }
}
