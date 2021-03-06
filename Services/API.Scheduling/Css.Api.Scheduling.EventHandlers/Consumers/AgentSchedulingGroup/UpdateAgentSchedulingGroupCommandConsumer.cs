﻿using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.EventHandlers.Consumers.AgentSchedulingGroup
{
    public class UpdateAgentSchedulingGroupCommandConsumer : IConsumer<UpdateAgentSchedulingGroupCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The agentSchedulingGroup name repository</summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="UpdateAgentSchedulingGroupCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="agentSchedulingGroupRepository">The agentSchedulingGroup repository.</param>
        /// <param name="uow">The uow.</param>
        public UpdateAgentSchedulingGroupCommandConsumer(IBusService busUtility, IAgentSchedulingGroupRepository agentSchedulingGroupRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<UpdateAgentSchedulingGroupCommand> context)
        {
            try
            {
                NoSQL.AgentSchedulingGroup agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails
                {
                    AgentSchedulingGroupId = context.Message.Id
                });

                if (agentSchedulingGroup == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                agentSchedulingGroup.AgentSchedulingGroupId = context.Message.Id;
                agentSchedulingGroup.ClientId = context.Message.ClientIdNewValue;
                agentSchedulingGroup.ClientLobGroupId = context.Message.ClientLobGroupIdNewValue;
                agentSchedulingGroup.SkillGroupId = context.Message.SkillGroupIdNewValue;
                agentSchedulingGroup.SkillTagId = context.Message.SkillTagIdNewValue;
                agentSchedulingGroup.TimezoneId = context.Message.TimezoneIdNewValue;
                agentSchedulingGroup.Name = context.Message.NameNewValue;
                agentSchedulingGroup.RefId = context.Message.RefIdNewValue;
                agentSchedulingGroup.IsDeleted = context.Message.IsDeletedNewValue;
                agentSchedulingGroup.TimezoneId = context.Message.TimezoneIdNewValue;
                agentSchedulingGroup.EstartProvision = context.Message.EstartProvisionNewValue;

                _agentSchedulingGroupRepository.UpdateAgentSchedulingGroup(agentSchedulingGroup);

                await _uow.Commit();

                await _busUtility.PublishEvent<IAgentSchedulingGroupUpdateSuccess>(MassTransitConstants.AgentSchedulingGroupUpdateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<UpdateAgentSchedulingGroupCommand> context)
        {
            await _busUtility.PublishEvent<IAgentSchedulingGroupUpdateFailed>(MassTransitConstants.AgentSchedulingGroupUpdateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.NameOldValue,
                context.Message.RefIdOldValue,
                context.Message.ClientIdOldValue,
                context.Message.ClientLobGroupIdOldvalue,
                context.Message.SkillGroupIdOldValue,
                context.Message.SkillTagIdOldValue,
                context.Message.TimezoneIdOldValue,
                context.Message.EstartProvisionOldValue,
                context.Message.FirstDayOfWeekOldValue,
                context.Message.OperationHourOldValue,
                context.Message.ModifiedByOldValue,
                context.Message.ModifiedDateOldValue,
                context.Message.IsDeletedOldValue
            });
        }
    }
}
