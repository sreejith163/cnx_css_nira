using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.AgentSchedulingGroup
{
    /// <summary>
    /// The consumer class that consumes the event IAgentSchedulingGroupCreateSuccess
    /// </summary>
    public class AgentSchedulingGroupCreateSuccessConsumer : IConsumer<IAgentSchedulingGroupCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the IAgentSchedulingGroupCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAgentSchedulingGroupCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"AgentSchedulingGroup creation successful - AgentSchedulingGroup Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}

