using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Core.EventBus.Events.AgentCategory;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.AgentCategory
{
    /// <summary>
    /// The consumer class that consumes the event IAgentCategoryCreateFailed
    /// </summary>
    public class AgentCategoryCreateFailedConsumer : IConsumer<IAgentCategoryCreateFailed>
    {
        /// <summary>
        /// The agentCategory service
        /// </summary>
        private readonly IAgentCategoryService _agentCategoryService;

        /// <summary>Initializes a new instance of the <see cref="AgentCategoryCreateFailedConsumer" /> class.</summary>
        /// <param name="agentCategoryService">The agentCategory service.</param>
        public AgentCategoryCreateFailedConsumer(IAgentCategoryService agentCategoryService)
        {
            _agentCategoryService = agentCategoryService;
        }

        /// <summary>
        ///  The business implementation when the IAgentCategoryCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAgentCategoryCreateFailed> context)
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
                ModifiedDate = context.Message.ModifiedDate,
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _agentCategoryService.UpdateAgentCategory(agentCategoryIdDetails, updateAgentCategory);
        }
    }
}
