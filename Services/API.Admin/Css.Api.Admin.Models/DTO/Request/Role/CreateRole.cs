using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.Role
{
    public class CreateRoleDTO
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
