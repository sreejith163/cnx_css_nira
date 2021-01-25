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
    public class AgentSchedulingGroupUpdateFailedConsumer : IConsumer<IAgentSchedulingGroupUpdateFailed>
    {
        /// <summary>
        /// The agentSchedulingGroup service
        /// </summary>
        private readonly IAgentSchedulingGroupService _agentSchedulingGroupService;

        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupUpdateFailedConsumer" /> class.</summary>
        /// <param name="agentSchedulingGroupService">The agentSchedulingGroup service.</param>
        public AgentSchedulingGroupUpdateFailedConsumer(IAgentSchedulingGroupService agentSchedulingGroupService)
        {
            _agentSchedulingGroupService = agentSchedulingGroupService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IAgentSchedulingGroupUpdateFailed> context)
        {
            AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails
            {
                AgentSchedulingGroupId = context.Message.Id
            };

            UpdateAgentSchedulingGroup updateAgentSchedulingGroup = new UpdateAgentSchedulingGroup
            {
                Name = context.Message.NameOldValue,
                RefId = context.Message.RefIdOldValue,
                SkillTagId = context.Message.SkillTagIdOldValue,
                TimezoneId = context.Message.TimezoneIdOldValue,
                FirstDayOfWeek = context.Message.FirstDayOfWeekOldValue,
                OperationHour =
                JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHourOldValue),
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _agentSchedulingGroupService.UpdateAgentSchedulingGroup(agentSchedulingGroupIdDetails, updateAgentSchedulingGroup);

        }
    }
}