using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.AgentCategory
{
    /// <summary>
    /// The consumer class that consumes the event IAgentCategoryCreateSuccess
    /// </summary>
    public class AgentCategoryCreateSuccessConsumer : IConsumer<IAgentCategoryCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the IAgentCategoryCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAgentCategoryCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"AgentCategory creation successful - AgentCategory Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}


