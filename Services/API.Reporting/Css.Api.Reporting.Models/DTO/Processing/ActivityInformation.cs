using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ActivityInformation
    {
        public ActivityOrigin Origin { get; set; }
        public ActivityStatus Status { get; set; }
        public ActivityType Type { get; set; }
    }
}
