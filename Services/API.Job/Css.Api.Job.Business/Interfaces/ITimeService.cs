using Css.Api.Job.Models.DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.Interfaces
{
    /// <summary>
    /// The interface for timeservice
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// A helper method to retreive all time information for the input span filters
        /// </summary>
        /// <param name="spanFilters">List of span filters</param>
        /// <returns>List of instances of SpanDetails</returns>
        Task<List<SpanDetails>> GetTimeDetails(List<SpanFilter> spanFilters);

        /// <summary>
        /// A helper method to retreive all time information for the input intra day filters
        /// </summary>
        /// <param name="intraDayFilters">List of intra day filters</param>
        /// <returns>List of instances of IntraDayDetails</returns>
        Task<List<IntraDayDetails>> GetTimeDetails(List<IntraDayFilter> intraDayFilters);
    }
}
