using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The factory interface to resolve all the services
    /// </summary>
    public interface IServiceFactory
    {
        /// <summary>
        /// A generic mapper to map a corresponding importer/exporter
        /// </summary>
        /// <typeparam name="T">A class of IImporter/IExporter</typeparam>
        /// <param name="key">The 'Key' field from the mapper json</param>
        /// <returns>An instance of the class implemention IImporter/IExporter</returns>
        T Map<T>(string key)
            where T : class;
    }
}
