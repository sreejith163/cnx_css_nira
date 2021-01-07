using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.SchedulingCode
{
    /// <summary>
    /// The consumer class that consumes the event ISchedulingCodeCreateSuccess
    /// </summary>
    public class SchedulingCodeCreateSuccessConsumer : IConsumer<ISchedulingCodeCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the ISchedulingCodeCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISchedulingCodeCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"SchedulingCode creation successful - SchedulingCode Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}


