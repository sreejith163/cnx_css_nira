using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// A helper service for ftp post processing
    /// </summary>
    public abstract class FTPImportService
    {
        #region Private Properties

        /// <summary>
        /// The FTP service
        /// </summary>
        private readonly IFTPService _ftp;
        #endregion

        #region Public Properties

        /// <summary>
        /// The feedback
        /// </summary>
        public readonly ActivityResponse ImportFeedback;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="ftp"></param>
        public FTPImportService(IFTPService ftp)
        {
            _ftp = ftp;
            ImportFeedback = new ActivityResponse();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method which has the general workflow for all ftp processing items
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public async Task Process(DataFeed feed)
        {
            ActivityDataResponse resp = await Import(feed.Content);
            feed.Metadata = resp.Metadata;

            ActivityData activity = new ActivityData();
            activity.Bytes = feed.Content.Length;
            activity.DataSet = feed.Feeder;

            if (resp.Status == (int)ProcessStatus.Success)
            {
                await _ftp.MoveToProcessedFolder(feed);
            }
            else if (resp.Status == (int)ProcessStatus.Partial)
            {
                await _ftp.MoveToUnprocessedFolder(feed);
                activity.Metadata = feed.Metadata;
            }
            else
            {
                await _ftp.MoveToFailedFolder(feed);
            }

            switch (resp.Status)
            {
                case (int)ProcessStatus.Success:
                    ImportFeedback.Completed.Add(activity);
                    break;
                case (int)ProcessStatus.Failed:
                    ImportFeedback.Failed.Add(activity);
                    break;
                case (int)ProcessStatus.Partial:
                    ImportFeedback.Partial.Add(activity);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The business logic to process the import
        /// </summary>
        /// <param name="data">The input byte[] to be processed</param>
        /// <returns>An instance of ActivityDataResponse</returns>
        public abstract Task<ActivityDataResponse> Import(byte[] data);
        #endregion
    }
}
