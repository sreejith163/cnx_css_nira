using Css.Api.Core.EventBus.Events.SkillGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.SkillGroup
{
    /// <summary>
    /// The consumer class that consumes the event ISkillGroupCreateSuccess
    /// </summary>
    public class SkillGroupCreateSuccessConsumer : IConsumer<ISkillGroupCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the ISkillGroupCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISkillGroupCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"SkillGroup creation successful - SkillGroup Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}

