using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults.AgentCategory
{
    public class AgentCategoryUpdateSuccessFault : IConsumer<Fault<IAgentCategoryUpdateSuccess>>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<IAgentCategoryUpdateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentCategory Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

