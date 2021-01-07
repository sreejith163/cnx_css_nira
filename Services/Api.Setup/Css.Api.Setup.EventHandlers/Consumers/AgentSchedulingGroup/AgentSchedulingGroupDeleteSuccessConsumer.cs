using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.AgentSchedulingGroup
{
    public class AgentSchedulingGroupDeleteSuccessConsumer : IConsumer<IAgentSchedulingGroupDeleteSuccess>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentSchedulingGroupDeleteSuccess> context)
        {
            var message = context.Message;
        }
    }
}

