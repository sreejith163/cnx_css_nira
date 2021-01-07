using Css.Api.Core.EventBus.Events.SkillGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.SkillGroup
{
    /// <summary>
    /// The consumer class that consumes the fault of the event ISkillGroupCreateFailed
    /// </summary>
    public class SkillGroupCreateSuccessFault : IConsumer<Fault<ISkillGroupCreateSuccess>>
    {
        /// <summary>
        /// The implementation when the fault of ISkillGroupCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<ISkillGroupCreateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

