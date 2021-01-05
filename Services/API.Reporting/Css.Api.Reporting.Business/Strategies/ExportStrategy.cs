using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Strategies
{
    /// <summary>
    /// The strategy class for all export related operations
    /// </summary>
    public class ExportStrategy : IExportStrategy
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
        public ExportStrategy(IServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to process the export strategy based on the input key
        /// </summary>
        /// <param name="key">The 'Key' field defined in the mapper json</param>
        /// <returns>An instance of ExportResponse</returns>
        public ExportResponse Process(string key)
        {
            return _serviceResolver.GetExporter(key).Process();
        }
        #endregion
    }
}
