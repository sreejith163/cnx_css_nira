using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface to access the source
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// The name of the source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The method to pull the data from the source
        /// </summary>
        /// <returns>A list of instances of DataFeed</returns>
        Task<List<DataFeed>> Pull();
    }
}
