using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.AgentCategory
{
    public class AgentCategoryUpdateFailedConsumer : IConsumer<IAgentCategoryUpdateFailed>
    {
        /// <summary>
        /// The agentCategory service
        /// </summary>
        private readonly IAgentCategoryService _agentCategoryService;

        /// <summary>Initializes a new instance of the <see cref="AgentCategoryUpdateFailedConsumer" /> class.</summary>
        /// <param name="agentCategoryService">The agentCategory service.</param>
        public AgentCategoryUpdateFailedConsumer(IAgentCategoryService agentCategoryService)
        {
            _agentCategoryService = agentCategoryService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentCategoryUpdateFailed> context)
        {
            AgentCategoryIdDetails agentCategoryIdDetails = new AgentCategoryIdDetails
            {
                AgentCategoryId = context.Message.Id
            };

            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory
            {
                Name = context.Message.NameOldValue,
                DataTypeId = context.Message.AgentCategoryTypeOldValue,
                DataTypeMinValue = context.Message.DataTypeMinValueOldValue,
                DataTypeMaxValue = context.Message.DataTypeMaxValueOldValue,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _agentCategoryService.UpdateAgentCategory(agentCategoryIdDetails, updateAgentCategory);
        }
    }
}


