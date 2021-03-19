using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.EStart;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Sources
{
    /// <summary>
    /// The estart export source class
    /// </summary>
    public class EStartExportSource : ISource
    {
        #region Private Properties

        /// <summary>
        /// The schedule clock helper service
        /// </summary>
        private readonly IScheduleService _scheduleClockService;

        /// <summary>
        /// The mapper service
        /// </summary>
        private readonly IMapperService _mapperService;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the source
        /// </summary>
        public string Name => "EStartExp";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="scheduleClockService"></param>
        /// <param name="mapperService"></param>
        public EStartExportSource(IScheduleService scheduleClockService, IMapperService mapperService)
        {
            _scheduleClockService = scheduleClockService;
            _mapperService = mapperService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to pull the data from the source
        /// </summary>
        /// <returns>A list of instances of DataFeed</returns>
        public async Task<List<DataFeed>> Pull()
        {
            var filter = _mapperService.GetFilterParams<EStartFilter>();
            filter.EstartProvision = true;
            var clockData = await _scheduleClockService.GetCalendarCharts(filter);

            return new List<DataFeed>() {
                new DataFeed()
                {
                    Feeder = string.Join("-","CSS",DataOptions.Mongo.GetDescription()),
                    Content = Encoding.Default.GetBytes(JsonConvert.SerializeObject(clockData))
                }
            };
        }
        #endregion
    }
}
