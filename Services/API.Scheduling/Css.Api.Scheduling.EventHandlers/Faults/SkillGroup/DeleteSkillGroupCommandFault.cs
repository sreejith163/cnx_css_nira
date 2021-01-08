using Css.Api.Core.EventBus.Commands.SkillGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.SkillGroup
{
    public class DeleteSkillGroupCommandFault : IConsumer<Fault<DeleteSkillGroupCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<DeleteSkillGroupCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
