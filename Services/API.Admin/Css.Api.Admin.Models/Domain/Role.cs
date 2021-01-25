using System;


namespace Css.Api.Admin.Models.Domain
{
    public class Role
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
