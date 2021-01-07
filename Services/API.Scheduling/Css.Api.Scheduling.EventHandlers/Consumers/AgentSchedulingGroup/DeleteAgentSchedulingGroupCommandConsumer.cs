using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.EventHandlers.Consumers.AgentSchedulingGroup
{
    public class DeleteAgentSchedulingGroupCommandConsumer : IConsumer<DeleteAgentSchedulingGroupCommand>
    {

        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The agentSchedulingGroup name repository</summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;


        /// <summary>Initializes a new instance of the <see cref="DeleteAgentSchedulingGroupCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="agentSchedulingGroupRepository">The agentSchedulingGroup repository.</param>
        /// <param name="uow">The uow.</param>
        public DeleteAgentSchedulingGroupCommandConsumer(IBusService busUtility, IAgentSchedulingGroupRepository agentSchedulingGroupRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<DeleteAgentSchedulingGroupCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.AgentSchedulingGroup agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails
                {
                    AgentSchedulingGroupId = context.Message.Id
                });

                if (agentSchedulingGroup == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                agentSchedulingGroup.AgentSchedulingGroupId = context.Message.Id;
                agentSchedulingGroup.IsDeleted = context.Message.IsDeletedNewValue;

                _agentSchedulingGroupRepository.UpdateAgentSchedulingGroup(agentSchedulingGroup);

                await _uow.Commit();

                await _busUtility.PublishEvent<IAgentSchedulingGroupDeleteSuccess>(MassTransitConstants.AgentSchedulingGroupDeleteSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<DeleteAgentSchedulingGroupCommand> context)
        {
            await _busUtility.PublishEvent<IAgentSchedulingGroupDeleteFailed>(MassTransitConstants.AgentSchedulingGroupDeleteFailedRouteKey, new
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                ClientId = context.Message.ClientId,
                ClientLobGroupId = context.Message.ClientLobGroupId,
                SkillGroupId = context.Message.SkillGroupId,
                SkillTagId = context.Message.SkillTagId,
                TimezoneId = context.Message.TimezoneId,
                FirstDayOfWeek = context.Message.FirstDayOfWeek,
                OperationHour = context.Message.OperationHour,
                ModifiedByOldValue = context.Message.ModifiedByOldValue,
                ModifiedDateOldValue = context.Message.ModifiedDateOldValue,
                IsDeletedOldValue = context.Message.IsDeletedOldValue
            });
        }
    }
}