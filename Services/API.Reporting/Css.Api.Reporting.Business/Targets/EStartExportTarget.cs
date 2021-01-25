using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Targets
{
    /// <summary>
    /// The EStart export target
    /// </summary>
    public class EStartExportTarget : ITarget
    {
        #region Private Properties

        /// <summary>
        /// The FTP service
        /// </summary>
        private readonly IFTPService _ftp;

        /// <summary>
        /// The clock helper service
        /// </summary>
        private readonly IScheduleClockService _scheduleClockService;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name => "EStartExpTar";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to intialize properties
        /// </summary>
        /// <param name="ftp"></param>
        /// <param name="scheduleClockService"></param>
        public EStartExportTarget(IFTPService ftp, IScheduleClockService scheduleClockService)
        {
            _ftp = ftp;
            _scheduleClockService = scheduleClockService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to push data to the target
        /// </summary>
        /// <param name="feeds"></param>
        /// <returns>An instance of ActivityResponse</returns>
        public async Task<ActivityResponse> Push(List<DataFeed> feeds)
        {
            DataFeed feed = feeds.First();
            var clockString = Encoding.Default.GetString(feed.Content);
            List<ScheduleClock> clocks = JsonConvert.DeserializeObject<List<ScheduleClock>>(clockString);
            
            ActivityResponse response = new ActivityResponse();
            
            if(!clocks.Any())
            {
                response.Failed.Add(new ActivityData()
                {
                    Bytes = 0,
                    Source = feed.Feeder
                });
                return response;
            }
            
            var exportText = _scheduleClockService.GenerateClocksText(clocks);
            var status = await Task.FromResult(_ftp.Write("test.ftp", exportText));
            
            if (status)
            {
                response.Failed.Add(new ActivityData() { Bytes = feed.Content.Length, Source = feed.Feeder });
                return response;
            }
            
            response.Completed.Add(new ActivityData() { Bytes = feed.Content.Length, Source = feed.Feeder });
            return response;
        }
        #endregion
    }
}
