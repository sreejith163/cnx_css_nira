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
        /// The method for processing the import strategy
        /// </summary>
        /// <returns>An instance of ImportResponse</returns>
        Task<TargetResponse> Process();
    }
}
