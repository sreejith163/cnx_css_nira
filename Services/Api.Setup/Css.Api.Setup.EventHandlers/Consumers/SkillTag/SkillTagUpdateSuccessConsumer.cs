using Css.Api.Core.EventBus.Events.SkillTag;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.SkillTag
{
    public class SkillTagUpdateSuccessConsumer : IConsumer<ISkillTagUpdateSuccess>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISkillTagUpdateSuccess> context)
        {
            var message = context.Message;
        }
    }
}