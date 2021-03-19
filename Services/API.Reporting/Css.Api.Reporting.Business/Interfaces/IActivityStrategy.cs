using Css.Api.Reporting.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// The method to collect the information from the source
        /// </summary>
        /// <returns>An instance of ActivityApiResponse</returns>
        Task<ActivityApiResponse> Collect();

        /// <summary>
        /// The method to push information to the source
        /// </summary>
        /// <returns>An instance of ActivityApiResponse</returns>
        Task<ActivityApiResponse> Assign();
    }
}
