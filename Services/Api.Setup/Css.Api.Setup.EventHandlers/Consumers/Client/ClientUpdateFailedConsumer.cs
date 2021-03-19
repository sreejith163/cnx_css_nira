using Css.Api.Core.EventBus.Events.Client;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.Client;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.Client
{
    public class ClientUpdateFailedConsumer : IConsumer<IClientUpdateFailed>
    {
        /// <summary>
        /// The client service
        /// </summary>
        private readonly IClientService _clientService;

        /// <summary>Initializes a new instance of the <see cref="ClientUpdateFailedConsumer" /> class.</summary>
        /// <param name="clientService">The client service.</param>
        public ClientUpdateFailedConsumer(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IClientUpdateFailed> context)
        {
            ClientIdDetails clientIdDetails = new ClientIdDetails
            {
                ClientId = context.Message.Id
            };

            UpdateClient updateClient = new UpdateClient
            {
                RefId = context.Message.RefIdOldValue,
                Name = context.Message.NameOldValue,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _clientService.UpdateClient(clientIdDetails, updateClient);

        }
    }
}

