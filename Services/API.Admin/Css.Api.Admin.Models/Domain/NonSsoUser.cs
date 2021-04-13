using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Domain
{
    public class NonSsoUser
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        public string FirstName { get; set; }
        public string Lastname { get; set; }

        public string Sso { get; set; }

    }
}
