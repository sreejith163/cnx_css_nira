using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Css.Api.Reporting.Models.Enums
{
    public enum ProcessStatus
    {
        [Description("Completed")]
        Success = 1,
        [Description("Failed")]
        Failed = 2,
        [Description("Processed")]
        Partial = 3
    }
}
