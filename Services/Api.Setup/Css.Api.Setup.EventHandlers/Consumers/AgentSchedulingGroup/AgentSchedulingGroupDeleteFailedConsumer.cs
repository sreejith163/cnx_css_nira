using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using MassTransit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.AgentSchedulingGroup
{
    public class AgentSchedulingGroupDeleteFailedConsumer : IConsumer<IAgentSchedulingGroupDeleteFailed>
    {
        /// <summary>
        /// The agentSchedulingGroup service
        /// </summary>
        private readonly IAgentSchedulingGroupService _agentSchedulingGroupService;


        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupDeleteFailedConsumer" /> class.</summary>
        /// <param name="agentSchedulingGroupService">The agentSchedulingGroup service.</param>
        public AgentSchedulingGroupDeleteFailedConsumer(IAgentSchedulingGroupService agentSchedulingGroupService)
        {
            _agentSchedulingGroupService = agentSchedulingGroupService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentSchedulingGroupDeleteFailed> context)
        {
            AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails
            {
                AgentSchedulingGroupId = context.Message.Id
            };

            UpdateAgentSchedulingGroup updateAgentSchedulingGroup = new UpdateAgentSchedulingGroup
            {
                Name = context.Message.Name,
                RefId = context.Message.RefId,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                SkillTagId = context.Message.SkillTagId,
                TimezoneId = context.Message.TimezoneId,
                EstartProvision = context.Message.EstartProvision,
                FirstDayOfWeek = context.Message.FirstDayOfWeek,
                OperationHour =
                    JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHour),
                IsUpdateRevert = true
            };

            await _agentSchedulingGroupService.RevertAgentSchedulingGroup(agentSchedulingGroupIdDetails, updateAgentSchedulingGroup);

        }
    }
}