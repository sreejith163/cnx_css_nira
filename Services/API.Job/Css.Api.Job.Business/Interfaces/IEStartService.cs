using Css.Api.Job.Models.DTO.Common;
using Css.Api.Job.Models.DTO.EStart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Business.Interfaces
{
    /// <summary>
    /// The interface for EStart helper service
    /// </summary>
    public interface IEStartService
    {
        /// <summary>
        /// A helper method to generate and club EStart export requests based on start date and end date
        /// </summary>
        /// <param name="timeDetails"></param>
        /// <returns></returns>
        List<EStartExportRequest> GenerateExportRequests(List<SpanDetails> timeDetails);

        /// <summary>
        /// A helper method to generate and club EStart export requests using the intra day details
        /// </summary>
        /// <param name="intraDayDetails"></param>
        /// <returns></returns>
        List<EStartExportRequest> GenerateExportRequests(List<IntraDayDetails> intraDayDetails);

        /// <summary>
        /// Returns the requested times of the day
        /// </summary>
        /// <returns></returns>
        string GetTimesOfDay(List<SpanFilter> filters);
    }
}
