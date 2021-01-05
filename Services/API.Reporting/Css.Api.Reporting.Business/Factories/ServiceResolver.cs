using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Factories
{
    /// <summary>
    /// A class to resolve all the requested services
    /// </summary>
    public class ServiceResolver : IServiceResolver
    {
        #region Private Properties
        /// <summary>
        /// The factory object of type IServiceFactory
        /// </summary>
        private readonly IServiceFactory _factory;

        /// <summary>
        /// The object of the IFTPService
        /// </summary>
        private readonly IFTPService _ftp;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="ftp"></param>
        public ServiceResolver(IServiceFactory factory, IFTPService ftp)
        {
            _factory = factory;
            _ftp = ftp;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method resolves and returns the corresponding IExporter class for the key using the mappings present in mapper.json
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>The instance of the class implementing IExporter</returns>
        public IExporter GetExporter(string key)
        {
            return _factory.Map<IExporter>(key);
        }

        /// <summary>
        /// The method resolves and returns the corresponding IImporter class for the key using the mappings present in mapper.json
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>The instance of the class implementing IImporter</returns>
        public IImporter GetImporter(string key)
        {
            return _factory.Map<IImporter>(key);
        }

        /// <summary>
        /// The method returns the data from the FTP using the key mappings present in mapper.json
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>A list of instances of DataFeed read from the source</returns>
        public async Task<List<DataFeed>> GetDataFromSource(string key)
        {
            return await _ftp.Read(key);
        }

        /// <summary>
        /// This method executes all pending post processing operations based on the status of the process
        /// </summary>
        /// <param name="status">An integer value corresponding to the enum ProcessType</param>
        /// <param name="feed">An instance of DataFeed for which the processing is to be carried out</param>
        public async Task Finalize(int status, DataFeed feed)
        {
            List<int> processed = new List<int>() { (int)ProcessStatus.Success, (int)ProcessStatus.Partial };
            if(processed.Contains(status))
            {
                await _ftp.MoveToProcessedFolder(feed);
            }
            else
            {
                await _ftp.MoveToFailedFolder(feed);
            }
        }
        #endregion
    }
}
