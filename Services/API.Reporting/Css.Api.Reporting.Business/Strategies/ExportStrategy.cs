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
        private readonly IServiceFactory _serviceFactory;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="serviceFactory"></param>
        public ExportStrategy(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to process the export strategy
        /// </summary>
        /// <returns></returns>
        public void Process()
        {
            
        }
        #endregion
    }
}
