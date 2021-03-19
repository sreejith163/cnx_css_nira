using Css.Api.Core.EventBus.Events.SkillGroup;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using MassTransit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.SkillGroup
{
    /// <summary>
    /// The consumer class that consumes the event ISkillGroupCreateFailed
    /// </summary>
    public class SkillGroupCreateFailedConsumer : IConsumer<ISkillGroupCreateFailed>
    {
        /// <summary>
        /// The skillGroup service
        /// </summary>
        private readonly ISkillGroupService _skillGroupService;

        /// <summary>Initializes a new instance of the <see cref="SkillGroupCreateFailedConsumer" /> class.</summary>
        /// <param name="skillGroupService">The skillGroup service.</param>
        public SkillGroupCreateFailedConsumer(ISkillGroupService skillGroupService)
        {
            _skillGroupService = skillGroupService;
        }

        /// <summary>
        ///  The business implementation when the ISkillGroupCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISkillGroupCreateFailed> context)
        {
            SkillGroupIdDetails skillGroupIdDetails = new SkillGroupIdDetails
            {
                SkillGroupId = context.Message.Id
            };

            UpdateSkillGroup updateSkillGroup = new UpdateSkillGroup
            {
                RefId = context.Message.RefId,
                Name = context.Message.Name,
                ClientLobGroupId = context.Message.ClientLobGroupId,
                TimezoneId = context.Message.TimezoneId,
                ModifiedDate = context.Message.ModifiedDate,
                FirstDayOfWeek = context.Message.FirstDayOfWeek,
                OperationHour =
                    JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHour),
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _skillGroupService.UpdateSkillGroup(skillGroupIdDetails, updateSkillGroup);
        }
    }
}
