using Css.Api.Core.EventBus.Events.SkillGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.SkillGroup
{
    public class SkillGroupDeleteSuccessConsumer : IConsumer<ISkillGroupDeleteSuccess>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISkillGroupDeleteSuccess> context)
        {
            var message = context.Message;
        }
    }
}

