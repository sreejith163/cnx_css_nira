using System;
using System.Collections.Generic;

namespace Css.Api.AdminOps.Models.Domain
{
    public partial class AgentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DataTypeId { get; set; }
        public string DataTypeMinValue { get; set; }
        public string DataTypeMaxValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual AgentCategoryDataType DataType { get; set; }
    }
}
