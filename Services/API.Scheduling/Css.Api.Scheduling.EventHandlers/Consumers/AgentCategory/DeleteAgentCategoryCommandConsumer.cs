using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentCategory;
using Css.Api.Core.EventBus.Events.AgentCategory;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategory;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.EventHandlers.Consumers.AgentCategory
{
    public class DeleteAgentCategoryCommandConsumer : IConsumer<DeleteAgentCategoryCommand>
    {
        /// <summary>
        /// The bus utility
        /// </summary>
        private readonly IBusService _busUtility;

        /// <summary>
        /// The agentCategory name repository
        /// </summary>
        private readonly IAgentCategoryRepository _agentCategoryRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="DeleteAgentCategoryCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="agentCategoryRepository">The agentCategory repository.</param>
        /// <param name="uow">The uow.</param>
        public DeleteAgentCategoryCommandConsumer(IBusService busUtility, IAgentCategoryRepository agentCategoryRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _agentCategoryRepository = agentCategoryRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<DeleteAgentCategoryCommand> context)
        {
            try
            {
                NoSQL.AgentCategory agentCategory = await _agentCategoryRepository.GetAgentCategory(new AgentCategoryIdDetails
                {
                    AgentCategoryId = context.Message.Id
                });

                if (agentCategory == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                agentCategory.AgentCategoryId = context.Message.Id;
                agentCategory.IsDeleted = context.Message.IsDeletedNewValue;

                _agentCategoryRepository.UpdateAgentCategory(agentCategory);

                await _uow.Commit();

                await _busUtility.PublishEvent<IAgentCategoryDeleteSuccess>(MassTransitConstants.AgentCategoryDeleteSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<DeleteAgentCategoryCommand> context)
        {
            await _busUtility.PublishEvent<IAgentCategoryDeleteFailed>(MassTransitConstants.AgentCategoryDeleteFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.AgentCategoryType,
                context.Message.DataTypeMinValue,
                context.Message.DataTypeMaxValue,
                context.Message.ModifiedByOldValue,
                context.Message.ModifiedDateOldValue,
                context.Message.IsDeletedOldValue
            });
        }
    }
}