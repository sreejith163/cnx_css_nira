using Css.Api.Core.EventBus.Events.Client;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.Client
{
    /// <summary>
    /// The consumer class that consumes the fault of the event IClientCreateFailed
    /// </summary>
    public class ClientCreateSuccessFault : IConsumer<Fault<IClientCreateSuccess>>
    {
        /// <summary>
        /// The implementation when the fault of IClientCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<IClientCreateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. Client Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
