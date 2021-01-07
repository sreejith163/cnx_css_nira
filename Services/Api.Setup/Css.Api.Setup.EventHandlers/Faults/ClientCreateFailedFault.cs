using Css.Api.Core.EventBus.Events.Client;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults
{
    /// <summary>
    /// The consumer class that consumes the fault of the event IClientCreateFailed
    /// </summary>
    public class ClientCreateFailedFault : IConsumer<Fault<IClientCreateFailed>>
    {
        /// <summary>
        /// The implementation when the fault of IClientCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<IClientCreateFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. Client Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
