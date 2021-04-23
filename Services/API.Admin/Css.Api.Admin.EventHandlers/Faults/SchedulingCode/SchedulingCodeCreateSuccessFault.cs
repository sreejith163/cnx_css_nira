using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults.SchedulingCode
{
    /// <summary>
    /// The consumer class that consumes the fault of the event ISchedulingCodeCreateFailed
    /// </summary>
    public class SchedulingCodeCreateSuccessFault : IConsumer<Fault<ISchedulingCodeCreateSuccess>>
    {
        /// <summary>
        /// The implementation when the fault of ISchedulingCodeCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<ISchedulingCodeCreateSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SchedulingCode Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

