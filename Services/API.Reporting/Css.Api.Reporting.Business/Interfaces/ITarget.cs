using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface to access the target
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// The name of the service
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The method to push data to the destination
        /// </summary>
        /// <param name="feeds">List of instances of DataFeed (sources)</param>
        /// <returns>An instance of ActivityResponse</returns>
        Task<ActivityResponse> Push(List<DataFeed> feeds);
    }
}
