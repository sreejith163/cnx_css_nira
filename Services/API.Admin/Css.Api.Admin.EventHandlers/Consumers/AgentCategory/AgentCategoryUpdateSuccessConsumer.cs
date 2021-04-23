using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.AgentCategory
{
    public class AgentCategoryUpdateSuccessConsumer : IConsumer<IAgentCategoryUpdateSuccess>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentCategoryUpdateSuccess> context)
        {
            var message = context.Message;
        }
    }
}
