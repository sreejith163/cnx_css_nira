using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns>An instance of StrategyResponse</returns>
        public async Task<ActivityResponse> Process()
        {
            ISource source = _serviceFactory.Map<ISource>();
            ITarget target = _serviceFactory.Map<ITarget>();
            _serviceFactory.Initialize();
            List<DataFeed> feeds = await source.Pull();
            return (ActivityResponse) await target.Push(feeds);
        }

        /// <summary>
        /// The method to collect the information from the source
        /// </summary>
        /// <returns>A json result</returns>
        public async Task<ActivityApiResponse> Collect()
        {
            ISource source = _serviceFactory.Map<ISource>();
            List<DataFeed> feeds = await source.Pull();
            var responseFeed = feeds.First();
            return JsonConvert.DeserializeObject<ActivityApiResponse>(Encoding.Default.GetString(responseFeed.Content));
        }

        /// <summary>
        /// The method to push information to the source
        /// </summary>
        /// <returns>A json result</returns>
        public async Task<ActivityApiResponse> Assign()
        {
            ITarget target = _serviceFactory.Map<ITarget>();
            try
            {
                var dataFeeds = _serviceFactory.GetRequestFeeds();
                if (!dataFeeds.Any())
                {
                    return new ActivityApiResponse()
                    {
                        StatusCode = System.Net.HttpStatusCode.NoContent,
                        Message = Messages.NoSchedulesToUpdate
                    };
                }

                return (ActivityApiResponse)await target.Push(dataFeeds);
            }
            catch(Exception ex)
            {
                return new ActivityApiResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }            
        }
        #endregion
    }
}
