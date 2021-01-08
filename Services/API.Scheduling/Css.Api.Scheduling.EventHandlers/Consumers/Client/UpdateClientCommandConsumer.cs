using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.Client;
using Css.Api.Core.EventBus.Events.Client;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.EventHandlers.Consumers.Client
{
    public class UpdateClientCommandConsumer : IConsumer<UpdateClientCommand>
    {

        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The client name repository</summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;


        /// <summary>Initializes a new instance of the <see cref="UpdateClientCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="uow">The uow.</param>
        public UpdateClientCommandConsumer(IBusService busUtility, IClientRepository clientRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _clientRepository = clientRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<UpdateClientCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.Client client = await _clientRepository.GetClient(new ClientIdDetails
                {
                    ClientId = context.Message.Id
                });

                if (client == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                client.ClientId = context.Message.Id;
                client.Name = context.Message.NameNewValue;
                client.IsDeleted = context.Message.IsDeletedNewValue;

                _clientRepository.UpdateClient(client);

                await _uow.Commit();

                await _busUtility.PublishEvent<IClientUpdateSuccess>(MassTransitConstants.ClientUpdateSuccessRouteKey, new
                {
                    Id = context.Message.Id
                });
            }
            catch (Exception ex)
            {
                await PublishFailedEvent(context);
            }
        }

        /// <summary>Publishes the failed event.</summary>
        /// <param name="context">The context.</param>
        private async Task PublishFailedEvent(ConsumeContext<UpdateClientCommand> context)
        {
            await _busUtility.PublishEvent<IClientUpdateFailed>(MassTransitConstants.ClientUpdateFailedRouteKey, new
            {
                Id = context.Message.Id,
                NameOldValue = context.Message.NameOldValue,
                ModifiedByOldValue = context.Message.ModifiedByOldValue,
                IsDeletedOldValue = context.Message.IsDeletedOldValue,
                ModifiedDateOldValue = context.Message.ModifiedDateOldValue
            });
        }
    }
}