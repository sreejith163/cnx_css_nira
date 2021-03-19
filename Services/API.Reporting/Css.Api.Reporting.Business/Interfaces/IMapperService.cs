using Css.Api.Reporting.Models.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// A helper service to access the mappings of the request
    /// </summary>
    public interface IMapperService
    {
        /// <summary>
        /// The mapper context object of the current request
        /// </summary>
        MappingContext Context { get; }

        /// <summary>
        /// The method to initialize the FTP service for the source/target
        /// </summary>
        /// <typeparam name="T">ISource/ITarget</typeparam>
        void InitializeFTP<T>()
            where T : class;

        /// <summary>
        /// A generic method to return the filter params for the current request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The instance of T</returns>
        T GetFilterParams<T>()
            where T : class;

        /// <summary>
        /// A generic method to return the query params for the current GET request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetQueryParams<T>()
            where T : class;
    }
}
