using Css.Api.Core.EventBus.Events.Client;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.Client;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.Client
{
    /// <summary>
    /// The consumer class that consumes the event IClientCreateFailed
    /// </summary>
    public class ClientCreateFailedConsumer : IConsumer<IClientCreateFailed>
    {
        /// <summary>
        /// The client service
        /// </summary>
        private readonly IClientService _clientService;

        /// <summary>Initializes a new instance of the <see cref="ClientCreateFailedConsumer" /> class.</summary>
        /// <param name="clientService">The client service.</param>
        public ClientCreateFailedConsumer(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        ///  The business implementation when the IClientCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IClientCreateFailed> context)
        {
            ClientIdDetails clientIdDetails = new ClientIdDetails
            {
                ClientId = context.Message.Id
            };

            UpdateClient updateClient = new UpdateClient
            {
                Name = context.Message.Name,
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _clientService.UpdateClient(clientIdDetails, updateClient);
        }
    }
}
