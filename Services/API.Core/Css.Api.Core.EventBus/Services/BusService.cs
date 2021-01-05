using Css.Api.Core.EventBus.Commands;
using Css.Api.Core.EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Core.EventBus.Services
{
    /// <summary>
    /// A Utility service to send/publish commands to the service bus
    /// </summary>
    public class BusService : IBusService
    {
        /// <summary>
        ///  The configuration settings of the importing service
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The MassTransit interface to manage requests sent to the service bus
        /// </summary>
        private readonly IBusControl _busControl;


        /// <summary>
        ///  The service constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="busControl"></param>
        public BusService(IConfiguration configuration, IBusControl busControl)
        {
            _configuration = configuration;
            _busControl = busControl;
        }

        /// <summary>
        ///  A generic method to send commands to the service bus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task SendCommand<T>(string queueName, object values)
           where T : class
        {
            //Uri endpointUri = GetEndPointUri(queueName);
            //var endPoint = await _busControl.GetSendEndpoint(endpointUri);
            //await endPoint.Send<T>(values);
            await _busControl.Publish<T>(values, x =>
            {
                x.SetRoutingKey(queueName);
            });
        }

        /// <summary>
        /// A generic method to publish events to the service bus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task PublishEvent<T>(string routingKey, object values)
            where T : class
        {
            await _busControl.Publish<T>(values, x =>
            {
                x.SetRoutingKey(routingKey);
            });
            //var endPoint = await _busControl.GetPublishSendEndpoint<T>();
            //await endPoint.Send<T>(values, x =>
            //{
            //    x.SetRoutingKey(routingKey);
            //});
        }

        /// <summary>
        /// A private method that generates the end point URI
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>an instance of type - Uri</returns>
        private Uri GetEndPointUri(string endpoint)
        {
            return new Uri($"{_configuration.GetConnectionString("ServiceBus")}" +
                 $"{endpoint}");
        }

    }
}
