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
    /// The consumer class that consumes the event IClientCreateSuccess
    /// </summary>
    public class ClientCreateSuccessConsumer : IConsumer<IClientCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the IClientCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IClientCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"Client creation successful - Client Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}
