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
    public class DeleteClientLOBCommandConsumer : IConsumer<DeleteClientLOBCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The clientLOB name repository</summary>
        private readonly IClientLobGroupRepository _clientLOBRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="DeleteClientLOBCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="clientLOBRepository">The clientLOB repository.</param>
        /// <param name="uow">The uow.</param>
        public DeleteClientLOBCommandConsumer(IBusService busUtility, IClientLobGroupRepository clientLOBRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _clientLOBRepository = clientLOBRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<DeleteClientLOBCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.ClientLobGroup clientLOB = await _clientLOBRepository.GetClientLobGroup(new ClientLobGroupIdDetails
                {
                    ClientLobGroupId = context.Message.Id
                });

                if (clientLOB == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                clientLOB.ClientLobGroupId = context.Message.Id;
                clientLOB.IsDeleted = context.Message.IsDeletedNewValue;

                _clientLOBRepository.UpdateClientLobGroup(clientLOB);

                await _uow.Commit();

                await _busUtility.PublishEvent<IClientLOBDeleteSuccess>(MassTransitConstants.ClientLOBDeleteSuccessRouteKey, new
                {
                    context.Message.Id
                });
            }
            catch (Exception ex)
            {
                await PublishFailedEvent(context);
            }
        }

        /// <summary>Publishes the failed event.</summary>
        /// <param name="context">The context.</param>
        private async Task PublishFailedEvent(ConsumeContext<DeleteClientLOBCommand> context)
        {
            await _busUtility.PublishEvent<IClientLOBDeleteFailed>(MassTransitConstants.ClientLOBDeleteFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.RefId,
                context.Message.Name,
                context.Message.ClientId,
                context.Message.TimezoneId,
                context.Message.FirstDayOfWeek,
                context.Message.ModifiedByOldValue,
                context.Message.IsDeletedOldValue,
                context.Message.ModifiedDateOldValue
            });
        }
    }
}

