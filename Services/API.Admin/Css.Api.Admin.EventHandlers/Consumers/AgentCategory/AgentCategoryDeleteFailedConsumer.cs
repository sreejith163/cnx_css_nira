using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.AgentCategory
{
    public class AgentCategoryDeleteFailedConsumer : IConsumer<IAgentCategoryDeleteFailed>
    {
        /// <summary>
        /// The agentCategory service
        /// </summary>
        private readonly IAgentCategoryService _agentCategoryService;


        /// <summary>Initializes a new instance of the <see cref="AgentCategoryDeleteFailedConsumer" /> class.</summary>
        /// <param name="agentCategoryService">The agentCategory service.</param>
        public AgentCategoryDeleteFailedConsumer(IAgentCategoryService agentCategoryService)
        {
            _agentCategoryService = agentCategoryService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentCategoryDeleteFailed> context)
        {
            AgentCategoryIdDetails agentCategoryIdDetails = new AgentCategoryIdDetails
            {
                AgentCategoryId = context.Message.Id
            };

            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory
            {
                Name = context.Message.Name,
                DataTypeId = context.Message.AgentCategoryType,
                DataTypeMinValue = context.Message.DataTypeMinValue,
                DataTypeMaxValue = context.Message.DataTypeMaxValue,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _agentCategoryService.RevertAgentCategory(agentCategoryIdDetails, updateAgentCategory);
        }
    }
}