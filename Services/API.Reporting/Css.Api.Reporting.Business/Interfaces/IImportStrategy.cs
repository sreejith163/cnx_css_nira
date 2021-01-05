using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface to implement the import strategy
    /// </summary>
    public interface IImportStrategy
    {
        /// <summary>
        /// The method for processing the strategy for the key
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>An instance of ImportResponse</returns>
        Task<ImportStrategyResponse> Process(string key);
    }
}
