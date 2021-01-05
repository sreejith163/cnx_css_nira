using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Strategies
{
    /// <summary>
    /// The strategy class for all import related operations
    /// </summary>
    public class ImportStrategy : IImportStrategy
    {
        #region Private Fields
        
        /// <summary>
        /// The service resolver to fetch related services
        /// </summary>
        private readonly IServiceResolver _serviceResolver;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="serviceResolver"></param>
        public ImportStrategy(IServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to process the import strategy based on the input key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An instance of ImportResponse</returns>
        public async Task<ImportStrategyResponse> Process(string key)
        {
            ImportStrategyResponse response = new ImportStrategyResponse();
            IImporter importer = _serviceResolver.GetImporter(key);
            List<DataFeed> feeds = await _serviceResolver.GetDataFromSource(key);
            foreach(DataFeed feed in feeds)
            {
                ImportResponse resp = await importer.Import(feed.Content);
                feed.Metadata = resp.Metadata;
                
                await _serviceResolver.Finalize(resp.Status, feed);

                ImportStrategyFeed strategyFeed = new ImportStrategyFeed();
                strategyFeed.Bytes = feed.Content.Length;
                strategyFeed.Source = feed.Path;

                switch(resp.Status)
                {
                    case (int)ProcessStatus.Success:
                        response.Completed.Add(strategyFeed);
                        break;
                    case (int)ProcessStatus.Failed:
                        response.Failed.Add(strategyFeed);
                        break;
                    case (int)ProcessStatus.Partial:
                        response.Partial.Add(strategyFeed);
                        break;
                    default:
                        break;
                }
            }
            return response;
        }
        #endregion
    }
}
