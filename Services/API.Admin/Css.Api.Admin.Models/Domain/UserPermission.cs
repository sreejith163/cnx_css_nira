using System;

namespace Css.Api.Admin.Models.Domain
{
    public class UserPermission
    {
        public int Id { get; set; }

        public int UserRoleId { get; set; }

        public string Sso { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string EmployeeId { get; set; }

        public string LanguagePreference { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Role Role { get; set; }

    }

}
