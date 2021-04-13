using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentCategory;
using Css.Api.Core.EventBus.Events.AgentCategory;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategory;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.EventHandlers.Consumers.AgentCategory
{
    public class CreateAgentCategoryCommandConsumer : IConsumer<CreateAgentCategoryCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The scheduling code repository</summary>
        private readonly IAgentCategoryRepository _agentCategoryGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="CreateAgentCategoryCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="agentCategoryRepository">The scheduling code repository.</param>
        /// <param name="uow">The uow.</param>
        public CreateAgentCategoryCommandConsumer(IBusService busUtility, IAgentCategoryRepository agentCategoryRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _agentCategoryGroupRepository = agentCategoryRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<CreateAgentCategoryCommand> context)
        {
            try
            {
                NoSQL.AgentCategory agentCategory = await _agentCategoryGroupRepository.GetAgentCategory(new AgentCategoryIdDetails
                {
                    AgentCategoryId = context.Message.Id
                });

                if (agentCategory != null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                _agentCategoryGroupRepository.CreateAgentCategory(
                        new NoSQL.AgentCategory
                        {
                            AgentCategoryId = context.Message.Id,
                            Name = context.Message.Name,
                            AgentCategoryType = (AgentCategoryType)context.Message.AgentCategoryType,
                            DataTypeMinValue = context.Message.DataTypeMinValue,
                            DataTypeMaxValue = context.Message.DataTypeMaxValue,
                            IsDeleted = false
                        }
                    );

                await _uow.Commit();

                await _busUtility.PublishEvent<IAgentCategoryCreateSuccess>(MassTransitConstants.AgentCategoryCreateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<CreateAgentCategoryCommand> context)
        {
            await _busUtility.PublishEvent<IAgentCategoryCreateFailed>(MassTransitConstants.AgentCategoryCreateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.AgentCategoryType,
                context.Message.DataTypeMinValue,
                context.Message.DataTypeMaxValue,
                context.Message.ModifiedDate
            });
        }
    }
}
