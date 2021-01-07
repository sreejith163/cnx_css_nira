using Css.Api.Core.EventBus.Events.Client;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers
{
    /// <summary>
    /// The consumer class that consumes the event IClientCreateFailed
    /// </summary>
    public class ClientCreateFailedConsumer : IConsumer<IClientCreateFailed>
    {
        /// <summary>
        ///  The business implementation when the IClientCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IClientCreateFailed> context)
        {
            //TODO - Logic for rollback
            //_logger.LogInformation($"Client creation failed - Client Id {context.Message.Id}");
            var message = context.Message;
        }
    }
}
