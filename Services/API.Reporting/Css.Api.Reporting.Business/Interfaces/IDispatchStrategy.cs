using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface for dispatch strategy
    /// </summary>
    public interface IDispatchStrategy
    {
        /// <summary>
        /// The method to push export info to external systems
        /// </summary>
        /// <returns></returns>
        Task<DispatchResponse> ExportAgent();
    }
}
