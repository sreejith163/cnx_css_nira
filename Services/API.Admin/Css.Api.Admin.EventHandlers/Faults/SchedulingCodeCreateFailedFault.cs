using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Faults
{
    /// <summary>
    /// The consumer class that consumes the fault of the event ISchedulingCodeCreateFailed
    /// </summary>
    public class SchedulingCodeCreateFailedFault : IConsumer<Fault<ISchedulingCodeCreateFailed>>
    {
        /// <summary>
        /// The implementation when the fault of ISchedulingCodeCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<Fault<ISchedulingCodeCreateFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SchedulingCode Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
