using System;
using System.Collections.Generic;

namespace Css.Api.SetupMenu.Models.Domain
{
    public partial class OperationHourOpenType
    {
        public OperationHourOpenType()
        {
            OperationHour = new HashSet<OperationHour>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OperationHour> OperationHour { get; set; }
    }
}
