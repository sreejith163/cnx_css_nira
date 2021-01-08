using Css.Api.Core.EventBus.Events.ClientLOB;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.ClientLOB
{
    /// <summary>
    /// The consumer class that consumes the fault of the event IClientLOBCreateFailed
    /// </summary>
    public class ClientLOBCreateSuccessFault : IConsumer<Fault<IClientLOBCreateSuccess>>
    {
        /// <summary>
        /// The implementation when the fault of IClientLOBCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<IClientLOBCreateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. ClientLOB Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
