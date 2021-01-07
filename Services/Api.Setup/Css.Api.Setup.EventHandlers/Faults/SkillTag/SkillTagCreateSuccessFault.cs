using Css.Api.Core.EventBus.Events.SkillTag;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.SkillTag
{
    /// <summary>
    /// The consumer class that consumes the fault of the event ISkillTagCreateFailed
    /// </summary>
    public class SkillTagCreateSuccessFault : IConsumer<Fault<ISkillTagCreateSuccess>>
    {
        /// <summary>
        /// The implementation when the fault of ISkillTagCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<ISkillTagCreateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillTag Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

