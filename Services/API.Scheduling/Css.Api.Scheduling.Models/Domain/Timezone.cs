using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class Timezone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Abbreviation { get; set; }
        public int Offset { get; set; }
    }
}
