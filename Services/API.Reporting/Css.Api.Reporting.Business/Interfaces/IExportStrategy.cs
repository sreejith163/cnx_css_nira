using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface to implement the export strategy
    /// </summary>
    public interface IExportStrategy
    {
        /// <summary>
        /// The method for processing the strategy for the key
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>An instance of ExportResponse</returns>
        ExportResponse Process(string key);
    }
}
