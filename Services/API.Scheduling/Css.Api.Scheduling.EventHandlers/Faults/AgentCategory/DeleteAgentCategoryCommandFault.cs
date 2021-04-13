using Css.Api.Core.EventBus.Commands.AgentCategory;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.AgentCategory
{
    public class DeleteAgentCategoryCommandFault : IConsumer<Fault<DeleteAgentCategoryCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<DeleteAgentCategoryCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentCategory Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}