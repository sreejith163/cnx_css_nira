using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.EntityHierarchy
{
    public class SkillGroupDTO
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

        /// <summary>Gets or sets the skill tags.</summary>
        /// <value>The skill tags.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SkillTagDTO> SkillTags { get; set; }
    }
}
