using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface to implement the activity strategy
    /// </summary>
    public interface IActivityStrategy
    {
        /// <summary>
        /// The method for processing the activity strategy
        /// </summary>
        /// <returns>An instance of ActivityResponse</returns>
        Task<ActivityResponse> Process();
    }
}
