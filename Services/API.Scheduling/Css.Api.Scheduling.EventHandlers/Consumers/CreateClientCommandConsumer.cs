using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.Client;
using Css.Api.Core.EventBus.Events.Client;
using Css.Api.Core.EventBus.Services;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Consumers
{
    public class CreateClientCommandConsumer : IConsumer<CreateClientCommand>
    {
        private readonly IBusService _busUtility;

        public CreateClientCommandConsumer(IBusService busUtility)
        {
            _busUtility = busUtility;
        }

        public async Task Consume(ConsumeContext<CreateClientCommand> context)
        {
            //TO DO 
            // Add business logic to insert the client details 
            //throw new Exception("Test exception");

            //Call the sucess/failure event post processing
            await _busUtility.PublishEvent<IClientCreateSuccess>(MassTransitConstants.ClientCreateSuccessRouteKey, new
            {
                Id = context.Message.Id
            });
        }
    }
}
