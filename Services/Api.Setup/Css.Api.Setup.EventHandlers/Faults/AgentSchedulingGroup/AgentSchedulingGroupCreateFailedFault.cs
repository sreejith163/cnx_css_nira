using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.AgentSchedulingGroup
{
    /// <summary>
    /// The consumer class that consumes the fault of the event IAgentSchedulingGroupCreateFailed
    /// </summary>
    public class AgentSchedulingGroupCreateFailedFault : IConsumer<Fault<IAgentSchedulingGroupCreateFailed>>
    {
        /// <summary>
        /// The implementation when the fault of IAgentSchedulingGroupCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<IAgentSchedulingGroupCreateFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentSchedulingGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
