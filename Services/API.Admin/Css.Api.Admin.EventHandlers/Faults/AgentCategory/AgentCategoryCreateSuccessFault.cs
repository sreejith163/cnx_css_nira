using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults.AgentCategory
{
    /// <summary>
    /// The consumer class that consumes the fault of the event IAgentCategoryCreateFailed
    /// </summary>
    public class AgentCategoryCreateSuccessFault : IConsumer<Fault<IAgentCategoryCreateSuccess>>
    {
        /// <summary>
        /// The implementation when the fault of IAgentCategoryCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<IAgentCategoryCreateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentCategory Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

