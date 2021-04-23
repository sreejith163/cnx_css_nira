using Css.Api.Core.Models.Enums;
using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface for dispatch service
    /// </summary>
    public interface IDispatchService
    {
        /// <summary>
        /// The method to push agent info to the respective target
        /// </summary>
        /// <param name="target"></param>
        /// <returns>An instance of DispatchResponse</returns>
        Task<DispatchResponse> PushAgentInfo(ActivityOrigin target);
    }
}
