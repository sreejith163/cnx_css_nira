using Css.Api.Core.EventBus.Events.SkillGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.SkillGroup
{

    public class SkillGroupDeleteFailedFault : IConsumer<Fault<ISkillGroupDeleteFailed>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<ISkillGroupDeleteFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}