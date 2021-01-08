using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.AgentSchedulingGroup
{
    public class AgentSchedulingGroupUpdateSuccessFault : IConsumer<Fault<IAgentSchedulingGroupUpdateSuccess>>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<IAgentSchedulingGroupUpdateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentSchedulingGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

