using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Response.UserPermission
{
    public class UserPermissionAgent
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Sso { get; set; }
        public string EmployeeId { get; set; }
    }
}
