using Css.Api.Reporting.Models.DTO.Processing;
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
        /// A generic mapper to map a corresponding source/target for the current request
        /// </summary>
        /// <typeparam name="T">A class of ISource/ITarget</typeparam>
        /// <returns>An instance of T</returns>
        T Map<T>()
            where T : class;

        /// <summary>
        /// The method to initialize the data options
        /// </summary>
        void Initialize();

        /// <summary>
        /// The method to initialize the fetch request feeds
        /// </summary>
        /// <returns>The list of instances of DataFeed</returns>
        List<DataFeed> GetRequestFeeds();
    }
}
