using Css.Api.Core.Models.DTO.Common;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.EStart;
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
        private readonly IScheduleService _scheduleClockService;

        /// <summary>
        /// The mapper service
        /// </summary>
        private readonly IMapperService _mapperService;
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
        public EStartExportTarget(IFTPService ftp, IScheduleService scheduleClockService, IMapperService mapperService)
        {
            _ftp = ftp;
            _scheduleClockService = scheduleClockService;
            _mapperService = mapperService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to push data to the target
        /// </summary>
        /// <param name="feeds"></param>
        /// <returns>An instance of StrategyResponse</returns>
        public async Task<StrategyResponse> Push(List<DataFeed> feeds)
        {
            DataFeed feed = feeds.First();
            var chartsString = Encoding.Default.GetString(feed.Content);
            List<CalendarChart> charts = JsonConvert.DeserializeObject<List<CalendarChart>>(chartsString);
            
            if(!charts.Any())
            {
                return new ActivityResponse()
                {
                    Failed = new List<ActivityData>()
                    {
                        new ActivityData()
                        {
                            Bytes = 0,
                            DataSet = feed.Feeder,
                            Metadata = Messages.ExportNoData
                        }
                    }
                };
            }

            return await Export(charts);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// The method that implements the export logic
        /// </summary>
        /// <param name="charts"></param>
        /// <returns></returns>
        private async Task<ActivityResponse> Export(List<CalendarChart> charts)
        {
            ActivityResponse response = new ActivityResponse();
            EStartFilter requestFilter = _mapperService.GetFilterParams<EStartFilter>();
            var timezones = charts.Select(x => x.TimezoneOffset).Distinct().ToList();

            timezones.ForEach(async timezone =>
            {
                var timezoneClocks = charts.Where(x => x.TimezoneOffset.Equals(timezone)).ToList();
                string fileName = string.Join("_", "CSS", timezone, 
                                    requestFilter.StartDate.ToString("yyyyMMdd"), requestFilter.EndDate.ToString("yyyyMMdd")) 
                                + ".ftp";
                var exportText = _scheduleClockService.GenerateExportText(timezoneClocks);
                var status = await Task.FromResult(_ftp.Write(fileName, exportText));
                if(status)
                {
                    response.Completed.Add(new ActivityData() { Bytes = exportText.Length, DataSet = fileName });
                }
                else
                {
                    response.Failed.Add(new ActivityData() { Bytes = exportText.Length, DataSet = fileName, Metadata = Messages.ExportFailed });
                }
            });
            
            return await Task.FromResult(response);
        }
        #endregion
    }
}
