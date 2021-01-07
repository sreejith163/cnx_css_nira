using Css.Api.Core.EventBus.Commands.AgentSchedulingGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.AgentSchedulingGroup
{
    public class UpdateAgentSchedulingGroupCommandFault : IConsumer<Fault<UpdateAgentSchedulingGroupCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<UpdateAgentSchedulingGroupCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. AgentSchedulingGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
