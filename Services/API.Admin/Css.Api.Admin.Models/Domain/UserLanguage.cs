using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Domain
{
    public class UserLanguagePreference
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        public string LanguagePreference { get; set; }
    }
}
