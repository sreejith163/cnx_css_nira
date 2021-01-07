using Css.Api.Core.EventBus.Events.ClientLOB;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.ClientLOB
{
    /// <summary>
    /// The consumer class that consumes the event IClientLOBCreateSuccess
    /// </summary>
    public class ClientLOBCreateSuccessConsumer : IConsumer<IClientLOBCreateSuccess>
    {
        /// <summary>
        /// The business implementation when the IClientLOBCreateSuccess occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IClientLOBCreateSuccess> context)
        {
            //TODO - Logic for successful callback
            //_logger.LogInformation($"ClientLOB creation successful - ClientLOB Id {context.Message.Id}");   

            var message = context.Message;
        }
    }
}
