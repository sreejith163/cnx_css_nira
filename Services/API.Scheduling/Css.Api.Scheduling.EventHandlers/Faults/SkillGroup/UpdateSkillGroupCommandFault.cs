using Css.Api.Core.EventBus.Commands.SkillGroup;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.SkillGroup
{
    public class UpdateSkillGroupCommandFault : IConsumer<Fault<UpdateSkillGroupCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<UpdateSkillGroupCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillGroup Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}