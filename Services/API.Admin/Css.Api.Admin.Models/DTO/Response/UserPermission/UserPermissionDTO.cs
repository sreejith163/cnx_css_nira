using System;
using Newtonsoft.Json;

namespace Css.Api.Admin.Models.DTO.Response.UserPermission
{
    public class UserPermissionDTO
    {
    
        public int Id { get; set; }

        public string Firstname { get; set; }
 
        public string Lastname { get; set; }

        public int UserRoleId { get; set; }

        public int RoleIndex { get; set; }

        public string RoleName { get; set; }

        public string Sso { get; set; }
        public string EmployeeId { get; set; }

        public string LanguagePreference { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}
