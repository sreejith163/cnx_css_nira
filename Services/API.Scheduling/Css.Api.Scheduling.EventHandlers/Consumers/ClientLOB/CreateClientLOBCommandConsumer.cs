using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.ClientLOB;
using Css.Api.Core.EventBus.Events.ClientLOB;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Consumers.ClientLOB
{
    public class CreateClientLOBCommandConsumer : IConsumer<CreateClientLOBCommand>
    {
                /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The client lob group repository</summary>
        private readonly IClientLobGroupRepository _clientLOBGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="CreateClientLOBCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="clientLobRepository">The client lob repository.</param>
        /// <param name="uow">The uow.</param>
        public CreateClientLOBCommandConsumer(IBusService busUtility, IClientLobGroupRepository clientLobRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _clientLOBGroupRepository = clientLobRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<CreateClientLOBCommand> context)
        {
            try
            {
                Models.Domain.ClientLobGroup clientLob = await _clientLOBGroupRepository.GetClientLobGroup(new ClientLobGroupIdDetails
                {
                    ClientLobGroupId = context.Message.Id
                });

                if (clientLob != null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                _clientLOBGroupRepository.CreateClientLobGroup(
                        new Css.Api.Scheduling.Models.Domain.ClientLobGroup
                        {
                            ClientLobGroupId = context.Message.Id,
                            Name = context.Message.Name,
                            RefId = context.Message.RefId,
                            ClientId = context.Message.ClientId,
                            IsDeleted = false
                        }
                    );

                await _uow.Commit();

                await _busUtility.PublishEvent<IClientLOBCreateSuccess>(MassTransitConstants.ClientLOBCreateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<CreateClientLOBCommand> context)
        {
            await _busUtility.PublishEvent<IClientLOBCreateFailed>(MassTransitConstants.ClientLOBCreateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.RefId,
                context.Message.ClientId,
                context.Message.TimezoneId,
                context.Message.ModifiedDate
            });
        }
    }
}