using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Css.Api.Reporting.Models.Enums
{
    public enum DataOptions
    {
        [Description("FTP")]
        FTP = 1,
        [Description("MongoDB")]
        Mongo = 2,
    }
}
