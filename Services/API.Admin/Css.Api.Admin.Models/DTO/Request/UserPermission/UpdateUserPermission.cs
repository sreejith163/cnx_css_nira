using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.UserPermission
{
    public class UpdateUserPermissionDTO : UserPermissionAtrribute
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public bool IsUpdateRevert { get; set; }

        public bool IsDeleted { get; set; }
    }
}
