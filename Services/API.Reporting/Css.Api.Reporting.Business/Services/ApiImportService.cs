using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// A helper service for api import processing
    /// </summary>
    public abstract class ApiImportService
    {
        #region Public Methods

        /// <summary>
        /// The method which has the general workflow for all API import processings
        /// </summary>
        /// <param name="dataFeeds"></param>
        /// <returns></returns>
        public async Task<ActivityApiResponse> Process(List<DataFeed> dataFeeds)
        {
            var respObject = new List<ActivityApiData>();
            ActivityApiResponse response = new ActivityApiResponse()
            {
                Data = respObject,
                StatusCode = HttpStatusCode.OK
            };

            foreach(var feed in dataFeeds)
            {
                respObject.Add(await Import(feed));
            }

            response.StatusCode = respObject.All(x => x.Status == ProcessStatus.Success.ToString())
                                ? HttpStatusCode.OK : 
                                        respObject.All(x => x.Status == ProcessStatus.Failed.ToString()) 
                                        ? HttpStatusCode.UnprocessableEntity : HttpStatusCode.PartialContent;
            return response;
        }

        /// <summary>
        /// The business logic to process the API import
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public abstract Task<ActivityApiData> Import(DataFeed feed);
        #endregion
    }
}
