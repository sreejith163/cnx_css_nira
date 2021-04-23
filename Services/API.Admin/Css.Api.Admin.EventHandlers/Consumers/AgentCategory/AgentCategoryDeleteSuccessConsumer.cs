using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.AgentCategory
{
    public class AgentCategoryDeleteSuccessConsumer : IConsumer<IAgentCategoryDeleteSuccess>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentCategoryDeleteSuccess> context)
        {
            var message = context.Message;
        }
    }
}