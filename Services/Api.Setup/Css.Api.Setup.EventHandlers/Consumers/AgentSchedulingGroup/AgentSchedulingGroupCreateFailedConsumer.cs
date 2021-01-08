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
    /// <summary>
    /// The consumer class that consumes the event IAgentSchedulingGroupCreateFailed
    /// </summary>
    public class AgentSchedulingGroupCreateFailedConsumer : IConsumer<IAgentSchedulingGroupCreateFailed>
    {
        /// <summary>
        /// The agentSchedulingGroup service
        /// </summary>
        private readonly IAgentSchedulingGroupService _agentSchedulingGroupService;

        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupCreateFailedConsumer" /> class.</summary>
        /// <param name="agentSchedulingGroupService">The agentSchedulingGroup service.</param>
        public AgentSchedulingGroupCreateFailedConsumer(IAgentSchedulingGroupService agentSchedulingGroupService)
        {
            _agentSchedulingGroupService = agentSchedulingGroupService;
        }

        /// <summary>
        ///  The business implementation when the IAgentSchedulingGroupCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAgentSchedulingGroupCreateFailed> context)
        {
            AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails
            {
                AgentSchedulingGroupId = context.Message.Id
            };

            UpdateAgentSchedulingGroup updateAgentSchedulingGroup = new UpdateAgentSchedulingGroup
            {
                Name = context.Message.Name,
                SkillTagId = context.Message.SkillTagId,
                TimezoneId = context.Message.TimezoneId,
                ModifiedDate = context.Message.ModifiedDate,
                FirstDayOfWeek = context.Message.FirstDayOfWeek,
                OperationHour =
                    JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHour),
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _agentSchedulingGroupService.UpdateAgentSchedulingGroup(agentSchedulingGroupIdDetails, updateAgentSchedulingGroup);
        }
    }
}
