using Css.Api.Core.EventBus.Commands.Client;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.Client
{
    public class UpdateClientCommandFault : IConsumer<Fault<UpdateClientCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<UpdateClientCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. Client Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
