using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.UserPermission
{
    public class CreateUserPermissionDTO : UserPermissionAtrribute
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string CreatedBy { get; set; }

    }
}
