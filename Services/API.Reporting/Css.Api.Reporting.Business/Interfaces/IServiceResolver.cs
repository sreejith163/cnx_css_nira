using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// An interface for resolving all the requested services
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// The method returns an IImporter class mapped using the key present in mapper.json
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>The instance of the class implementing IImporter</returns>
        public IImporter GetImporter(string key);

        /// <summary>
        /// The method returns an IExporter class mapped using the key present in mapper.json
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>The instance of the class implementing IExporter</returns>
        public IExporter GetExporter(string key);

        /// <summary>
        /// The method returns an the data from a source using the key present in mapper.json and a mapper resolver(by default an FTP service)
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>A list of instances of DataFeed read from the source</returns>
        Task<List<DataFeed>> GetDataFromSource(string key);

        /// <summary>
        /// The method executes all post processing logics based on the status
        /// </summary>
        /// <param name="status">An integer value corresponding to the enum ProcessType<</param>
        /// <param name="feed">An instance of DataFeed for which the processing is to be carried out</param>
        Task Finalize(int status, DataFeed feed);
    }
}
