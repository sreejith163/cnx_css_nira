using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Response
{
    public class ImportStrategyFeed
    {
        /// <summary>
        /// The source of the import
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The size of the import
        /// </summary>
        public int Bytes { get; set; }
    }
}
