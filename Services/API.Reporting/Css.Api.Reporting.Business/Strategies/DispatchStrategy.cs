using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Strategies
{
    /// <summary>
    /// The strategy for all dispatch related requests
    /// </summary>
    public class DispatchStrategy : IDispatchStrategy
    {
        #region Private Fields

        /// <summary>
        /// The factory object of type IServiceFactory
        /// </summary>
        private readonly IServiceFactory _serviceFactory;

        /// <summary>
        /// The dispatch service
        /// </summary>
        private readonly IDispatchService _dispatchService;
        #endregion

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="serviceFactory"></param>
        /// <param name="dispatchService"></param>
        public DispatchStrategy(IServiceFactory serviceFactory, IDispatchService dispatchService)
        {
            _serviceFactory = serviceFactory;
            _dispatchService = dispatchService;
        }

        /// <summary>
        /// The method to export agent info to external systems
        /// </summary>
        /// <returns></returns>
        public async Task<DispatchResponse> ExportAgent()
        {
            _serviceFactory.Initialize();
            var target = _serviceFactory.GetTarget();
            var response = await _dispatchService.PushAgentInfo(target);

            if (response.Status.Equals(ProcessStatus.Success))
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
            }
            return response;
        }
    }
}
