using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface for the FTP service
    /// </summary>
    public interface IFTPService
    {
        /// <summary>
        /// The method to set FTP options
        /// </summary>
        /// <param name="options"></param>
        public void Set(Dictionary<string, string> options);

        /// <summary>
        /// The method to read data from FTP Inbox for the mapped context.
        /// </summary>
        /// <returns>A list of instances of DataFeed for all files present in import directory</returns>
        List<DataFeed> Read();

        /// <summary>
        /// The method to write data to FTP Outbox for the mapped context
        /// </summary>
        /// <param name="filename">The name of the destination file</param>
        /// <param name="contents">The content of the file</param>
        /// <returns>True if successful</returns>
        bool Write(string filename, string contents);

        /// <summary>
        /// Moves the processed file to the corresponding completed folder within the FTP
        /// </summary>
        /// <param name="feed">The instance of DataFeed which completed processing</param>
        Task MoveToProcessedFolder(DataFeed feed);

        /// <summary>
        /// Moves the unprocessed data to the corresponding folder within the FTP
        /// </summary>
        /// <param name="feed">The instance of DataFeed which completed processing</param>
        Task MoveToUnprocessedFolder(DataFeed feed);

        /// <summary>
        /// Moves the unprocessed file to the corresponding failed folder within the FTP
        /// </summary>
        /// <param name="feed">The instance of DataFeed which failed to process</param>
        Task MoveToFailedFolder(DataFeed feed);
    }
}
