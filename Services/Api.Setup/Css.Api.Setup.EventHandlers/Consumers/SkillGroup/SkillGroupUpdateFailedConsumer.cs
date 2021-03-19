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
    public class SkillGroupUpdateFailedConsumer : IConsumer<ISkillGroupUpdateFailed>
    {
        /// <summary>
        /// The skillGroup service
        /// </summary>
        private readonly ISkillGroupService _skillGroupService;

        /// <summary>Initializes a new instance of the <see cref="SkillGroupUpdateFailedConsumer" /> class.</summary>
        /// <param name="skillGroupService">The skillGroup service.</param>
        public SkillGroupUpdateFailedConsumer(ISkillGroupService skillGroupService)
        {
            _skillGroupService = skillGroupService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISkillGroupUpdateFailed> context)
        {
            SkillGroupIdDetails skillGroupIdDetails = new SkillGroupIdDetails
            {
                SkillGroupId = context.Message.Id
            };

            UpdateSkillGroup updateSkillGroup = new UpdateSkillGroup
            {
                RefId = context.Message.RefIdOldValue,
                Name = context.Message.NameOldValue,
                ClientLobGroupId = context.Message.ClientLobGroupIdOldvalue,
                TimezoneId = context.Message.TimezoneIdOldValue,
                FirstDayOfWeek = context.Message.FirstDayOfWeekOldValue,
                OperationHour =
                JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHourOldValue),
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _skillGroupService.UpdateSkillGroup(skillGroupIdDetails, updateSkillGroup);

        }
    }
}


