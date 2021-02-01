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
    /// The strategy class for all activity related operations
    /// </summary>
    public class ActivityStrategy : IActivityStrategy
    {
        #region Private Fields

        /// <summary>
        /// The factory object of type IServiceFactory
        /// </summary>
        private readonly IServiceFactory _serviceFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="serviceFactory"></param>
        public ActivityStrategy(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to process the activity strategy
        /// </summary>
        /// <returns>An instance of ActivityResponse</returns>
        public async Task<ActivityResponse> Process()
        {
            ISource source = _serviceFactory.Map<ISource>();
            ITarget target = _serviceFactory.Map<ITarget>();
            _serviceFactory.Initialize();
            List<DataFeed> feeds = await source.Pull();
            return await target.Push(feeds);
        }
        #endregion
    }
}