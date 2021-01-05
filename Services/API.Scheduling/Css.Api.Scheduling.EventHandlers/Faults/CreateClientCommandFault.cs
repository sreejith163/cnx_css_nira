using Css.Api.Core.EventBus.Commands.Client;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults
{
    public class CreateClientCommandFault : IConsumer<Fault<CreateClientCommand>>
    {
        public Task Consume(ConsumeContext<Fault<CreateClientCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. Client Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
