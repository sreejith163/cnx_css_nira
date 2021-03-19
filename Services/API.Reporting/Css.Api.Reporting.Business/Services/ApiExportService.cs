using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// A helper service for api export processing
    /// </summary>
    public abstract class ApiExportService
    {
        #region Public Methods

        /// <summary>
        /// The method which has the general workflow for all API export processings
        /// </summary>
        /// <returns>An single element list of type DataFeed</returns>
        public async Task<List<DataFeed>> Process()
        {
            var response = await Export();
            return new List<DataFeed>()
            {
                new DataFeed()
                {
                    Feeder = "CSS",
                    Content = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response))
                }
            }; 
        }

        /// <summary>
        /// The business logic to process the API export
        /// </summary>
        /// <returns>An instance of ActivityApiResponse</returns>
        public abstract Task<ActivityApiResponse> Export();
        #endregion
    }
}
