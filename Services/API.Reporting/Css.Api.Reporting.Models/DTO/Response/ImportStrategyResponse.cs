using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Response
{
    public class ImportStrategyResponse
    {
        /// <summary>
        /// The total number of source processed
        /// </summary>
        public int TotalSources { 
            get { 
                return Completed.Count + Failed.Count + Partial.Count; 
            } 
        }

        /// <summary>
        /// The final status
        /// </summary>
        public string Status { 
            get {
                if(Failed.Count == TotalSources)
                {
                    return ProcessStatus.Failed.GetDescription();
                }
                else if (Completed.Count == TotalSources)
                {
                    return ProcessStatus.Success.GetDescription();
                }
                else 
                {
                    return ProcessStatus.Partial.GetDescription();
                }
            } 
        }

        /// <summary>
        /// List of all successful imports
        /// </summary>
        public List<ImportStrategyFeed> Completed { get; set; }

        /// <summary>
        /// List of all failed imports
        /// </summary>
        public List<ImportStrategyFeed> Failed { get; set; }

        /// <summary>
        /// List of all partial processed imports
        /// </summary>
        public List<ImportStrategyFeed> Partial { get; set; }

        /// <summary>
        /// Constructor to initialize the properties
        /// </summary>
        public ImportStrategyResponse()
        {
            Completed = new List<ImportStrategyFeed>();
            Failed = new List<ImportStrategyFeed>();
            Partial = new List<ImportStrategyFeed>();
        }
    }
}
