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
        /// The method to read data from FTP for the defined key. All other FTP configs will be pulled from the mapper json using the key.
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>A list of instances of DataFeed for all files present in import directory</returns>
        Task<List<DataFeed>> Read(string key);

        /// <summary>
        /// Moves the processed file to the corresponding completed folder within the FTP
        /// </summary>
        /// <param name="feed">The instance of DataFeed which completed processing</param>
        Task MoveToProcessedFolder(DataFeed feed);

        /// <summary>
        /// Moves the unprocessed file to the corresponding failed folder within the FTP
        /// </summary>
        /// <param name="feed">The instance of DataFeed which failed to process</param>
        Task MoveToFailedFolder(DataFeed feed);
    }
}
