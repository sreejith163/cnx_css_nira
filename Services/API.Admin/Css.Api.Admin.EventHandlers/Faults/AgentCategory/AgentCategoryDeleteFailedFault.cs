using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults.AgentCategory
{

    public class AgentCategoryDeleteFailedFault : IConsumer<Fault<IAgentCategoryDeleteFailed>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<IAgentCategoryDeleteFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentCategory Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}