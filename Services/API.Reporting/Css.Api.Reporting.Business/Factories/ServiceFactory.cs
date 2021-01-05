using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Response;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Css.Api.Reporting.Business.Factories
{
    /// <summary>
    /// The factory to resolve all the services
    /// </summary>
    public class ServiceFactory : IServiceFactory
    {
        #region Private Properties 
        /// <summary>
        /// The IOptions of values in mapper.json
        /// </summary>
        private readonly IOptions<MapperSettings> _mapper;

        /// <summary>
        /// An Enumerable of all available IImporters
        /// </summary>
        private readonly IEnumerable<IImporter> _importers;

        /// <summary>
        /// An Enumerable of all available IExporters
        /// </summary>
        private readonly IEnumerable<IExporter> _exporters;
        #endregion

        #region Constructor
        /// <summary>
        ///  Constructor to initialize all properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="importers"></param>
        /// <param name="exporters"></param>
        public ServiceFactory(IOptions<MapperSettings> mapper, IEnumerable<IImporter> importers, IEnumerable<IExporter> exporters)
        {
            _mapper = mapper;
            _importers = importers;
            _exporters = exporters;
        }        
        #endregion

        #region Public Methods

        /// <summary>
        /// A generic mapper to map a corresponding importer/exporter 
        /// </summary>
        /// <typeparam name="T">A class of IImporter/IExporter</typeparam>
        /// <param name="key">The 'Key' field from the mapper json</param>
        /// <returns>The instance of the class implementing IImporter/IExporter</returns>
        public T Map<T>(string key)
            where T : class
        {
            T service;

            if(typeof(T).GetTypeInfo().IsAssignableFrom(typeof(IImporter).Ge‌​tTypeInfo()))
            {
                var importMap = _mapper.Value.Imports.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.InvariantCultureIgnoreCase));
                if (importMap == null)
                {
                    throw new MappingException(string.Format(Messages.MappingNotFound, key));
                }
                service = (T) _importers.FirstOrDefault(x => string.Equals(x.Name, importMap.Service, StringComparison.InvariantCultureIgnoreCase));
            }
            else if (typeof(T).GetTypeInfo().IsAssignableFrom(typeof(IExporter).Ge‌​tTypeInfo()))
            {
                var exportMap = _mapper.Value.Exports.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.InvariantCultureIgnoreCase));
                if (exportMap == null)
                {
                    throw new MappingException(string.Format(Messages.MappingNotFound, key));
                }
                service = (T) _exporters.FirstOrDefault(x => string.Equals(x.Name, exportMap.Service, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                throw new MappingException(string.Format(Messages.ServiceMappingNotFound, key));
            }

            return service;
        }
        #endregion
    }

}
