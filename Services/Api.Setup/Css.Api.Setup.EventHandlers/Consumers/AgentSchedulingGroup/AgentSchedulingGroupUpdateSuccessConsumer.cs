using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.AgentSchedulingGroup
{
    public class AgentSchedulingGroupUpdateSuccessConsumer : IConsumer<IAgentSchedulingGroupUpdateSuccess>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentSchedulingGroupUpdateSuccess> context)
        {
            var message = context.Message;
        }
    }
}