using Css.Api.Core.EventBus.Events.SkillTag;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.SkillTag
{
    /// <summary>
    /// The consumer class that consumes the event ISkillTagCreateSuccess
    /// </summary>
    public class SkillTagCreateSuccessConsumer : IConsumer<ISkillTagCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the ISkillTagCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISkillTagCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"SkillTag creation successful - SkillTag Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}

