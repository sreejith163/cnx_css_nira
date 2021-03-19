using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Models.DTO.Common;
using Css.Api.Job.Models.DTO.EStart;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.Services
{
    /// <summary>
    /// The EStart helper service
    /// </summary>
    public class EStartService : IEStartService
    {
        #region Public Methods

        /// <summary>
        /// A helper method to generate and club EStart export requests based on start date and end date
        /// </summary>
        /// <param name="timeDetails"></param>
        /// <returns></returns>
        public List<EStartExportRequest> GenerateExportRequests(List<SpanDetails> timeDetails)
        {
            List<EStartExportRequest> eStartExportRequests = new List<EStartExportRequest>();

            timeDetails.ForEach(x =>
            {
                string fromDate = x.CurrentDate.ToString("yyyy-MM-dd");
                string endDate = x.CurrentDate.AddDays(x.Days).ToString("yyyy-MM-dd");
                var req = eStartExportRequests.FirstOrDefault(x => x.StartDate.Equals(fromDate) && x.EndDate.Equals(endDate));
                if (req != null)
                {
                    req.TimezoneIds.Add(x.TimezoneId);
                }
                else
                {
                    eStartExportRequests.Add(new EStartExportRequest
                    {
                        StartDate = fromDate,
                        EndDate = endDate,
                        TimezoneIds = new List<int>() { x.TimezoneId }
                    });
                }
            });

            return eStartExportRequests;
        }

        /// <summary>
        /// A helper method to generate and club EStart export requests using the intra day details
        /// </summary>
        /// <param name="intraDayDetails"></param>
        /// <returns></returns>
        public List<EStartExportRequest> GenerateExportRequests(List<IntraDayDetails> intraDayDetails)
        {
            List<EStartExportRequest> eStartExportRequests = new List<EStartExportRequest>();

            intraDayDetails.ForEach(x =>
            {
                string fromDate = x.StartDate.ToString("yyyy-MM-dd");
                string endDate = x.EndDate.ToString("yyyy-MM-dd");
                var req = eStartExportRequests.FirstOrDefault(x => x.StartDate.Equals(fromDate) && x.EndDate.Equals(endDate));
                if (req != null)
                {
                    req.TimezoneIds.Add(x.TimezoneId);
                }
                else
                {
                    eStartExportRequests.Add(new EStartExportRequest
                    {
                        StartDate = fromDate,
                        EndDate = endDate,
                        TimezoneIds = new List<int>() { x.TimezoneId },
                        UpdatedInPastDays = x.RecentlyUpdatedInDays
                    });
                }
            });
            return eStartExportRequests;
        }

        /// <summary>
        /// Returns the requested times of the day
        /// </summary>
        /// <returns></returns>
        public string GetTimesOfDay(List<SpanFilter> filters)
        {
            return string.Join(", ", filters.Select(x => x.TimeOfDay).ToList());
        }
        #endregion
    }
}
