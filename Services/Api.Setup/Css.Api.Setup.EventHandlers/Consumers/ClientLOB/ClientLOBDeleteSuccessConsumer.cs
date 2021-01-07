using Css.Api.Core.EventBus.Events.ClientLOB;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.ClientLOB
{
    public class ClientLOBDeleteSuccessConsumer : IConsumer<IClientLOBDeleteSuccess>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IClientLOBDeleteSuccess> context)
        {
            var message = context.Message;
        }
    }
}
