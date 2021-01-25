using Css.Api.Core.Models.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.UserPermission
{
    public class UserPermissionQueryParameters : QueryStringParameters
    {
        public UserPermissionQueryParameters()
        {
            OrderBy = "Id";
        }

        //public int? UserRoleId { get; set; }
    }
}
