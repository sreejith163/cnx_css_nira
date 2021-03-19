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
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.EventHandlers.Consumers.AgentSchedulingGroup
{
    public class CreateAgentSchedulingGroupCommandConsumer : IConsumer<CreateAgentSchedulingGroupCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The agent scheduling group repository</summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="CreateAgentSchedulingGroupCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="agentSchedulingGroupRepository">The agent scheduling group repository.</param>
        /// <param name="uow">The uow.</param>
        public CreateAgentSchedulingGroupCommandConsumer(IBusService busUtility, IAgentSchedulingGroupRepository agentSchedulingGroupRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _agentSchedulingGroupGroupRepository = agentSchedulingGroupRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<CreateAgentSchedulingGroupCommand> context)
        {
            try
            {
                NoSQL.AgentSchedulingGroup agentSchedulingGroup = await _agentSchedulingGroupGroupRepository.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails
                {
                    AgentSchedulingGroupId = context.Message.Id
                });

                if (agentSchedulingGroup != null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                _agentSchedulingGroupGroupRepository.CreateAgentSchedulingGroup(
                        new NoSQL.AgentSchedulingGroup
                        {
                            AgentSchedulingGroupId = context.Message.Id,
                            Name = context.Message.Name,
                            RefId = context.Message.RefId,
                            ClientId = context.Message.ClientId,
                            ClientLobGroupId = context.Message.ClientLobGroupId,
                            SkillGroupId = context.Message.SkillGroupId,
                            SkillTagId = context.Message.SkillTagId,
                            TimezoneId = context.Message.TimezoneId,
                            EstartProvision = context.Message.EstartProvision,
                            IsDeleted = false
                        }
                    );

                await _uow.Commit();

                await _busUtility.PublishEvent<IAgentSchedulingGroupCreateSuccess>(MassTransitConstants.AgentSchedulingGroupCreateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<CreateAgentSchedulingGroupCommand> context)
        {
            await _busUtility.PublishEvent<IAgentSchedulingGroupCreateFailed>(MassTransitConstants.AgentSchedulingGroupCreateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.RefId,
                context.Message.ClientId,
                context.Message.ClientLobGroupId,
                context.Message.SkillGroupId,
                context.Message.SkillTagId,
                context.Message.TimezoneId,
                context.Message.EstartProvision,
                context.Message.FirstDayOfWeek,
                context.Message.OperationHour,
                context.Message.ModifiedDate
            });
        }
    }
}
