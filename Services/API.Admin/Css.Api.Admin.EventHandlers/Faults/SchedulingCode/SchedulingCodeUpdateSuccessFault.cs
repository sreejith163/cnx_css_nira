using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults.SchedulingCode
{
    public class SchedulingCodeUpdateSuccessFault : IConsumer<Fault<ISchedulingCodeUpdateSuccess>>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<ISchedulingCodeUpdateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SchedulingCode Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

