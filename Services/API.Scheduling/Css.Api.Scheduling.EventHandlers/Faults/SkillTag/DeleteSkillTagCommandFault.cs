using Css.Api.Core.EventBus.Commands.SkillTag;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.SkillTag
{
    public class DeleteSkillTagCommandFault : IConsumer<Fault<DeleteSkillTagCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<DeleteSkillTagCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillTag Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
