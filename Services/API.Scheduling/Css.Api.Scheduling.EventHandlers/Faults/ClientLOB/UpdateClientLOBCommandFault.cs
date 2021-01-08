using Css.Api.Core.EventBus.Commands.ClientLOB;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.ClientLOB
{
    public class UpdateClientLOBCommandFault : IConsumer<Fault<UpdateClientLOBCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<UpdateClientLOBCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. ClientLOB Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

