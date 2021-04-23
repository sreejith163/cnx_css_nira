using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults.AgentCategory
{
    /// <summary>
    /// The consumer class that consumes the fault of the event AgentCategory
    /// </summary>
    public class AgentCategoryCreateFailedFault : IConsumer<Fault<IAgentCategoryCreateFailed>>
    {
        /// <summary>
        /// The implementation when the fault of AgentCategory occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<IAgentCategoryCreateFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentCategory Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
