using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Response.Role
{
    public class RoleDTO
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ModifiedDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
