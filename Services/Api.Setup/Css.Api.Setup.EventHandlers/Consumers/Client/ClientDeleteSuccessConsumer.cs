using Css.Api.Core.EventBus.Events.Client;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.Client
{
    public class ClientDeleteSuccessConsumer : IConsumer<IClientDeleteSuccess>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IClientDeleteSuccess> context)
        {
            var message = context.Message;
        }
    }
}
