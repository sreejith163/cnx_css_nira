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
    public class SkillGroupDeleteFailedConsumer : IConsumer<ISkillGroupDeleteFailed>
    {
        /// <summary>
        /// The skillGroup service
        /// </summary>
        private readonly ISkillGroupService _skillGroupService;


        /// <summary>Initializes a new instance of the <see cref="SkillGroupDeleteFailedConsumer" /> class.</summary>
        /// <param name="skillGroupService">The skillGroup service.</param>
        public SkillGroupDeleteFailedConsumer(ISkillGroupService skillGroupService)
        {
            _skillGroupService = skillGroupService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISkillGroupDeleteFailed> context)
        {
            SkillGroupIdDetails skillGroupIdDetails = new SkillGroupIdDetails
            {
                SkillGroupId = context.Message.Id
            };

            UpdateSkillGroup updateSkillGroup = new UpdateSkillGroup
            {
                RefId = context.Message.RefId,
                Name = context.Message.Name,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                ClientLobGroupId = context.Message.ClientLobGroupId,
                TimezoneId = context.Message.TimezoneId,
                FirstDayOfWeek = context.Message.FirstDayOfWeek,
                OperationHour =
                    JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHour),
                IsUpdateRevert = true
            };

            await _skillGroupService.RevertSkillGroup(skillGroupIdDetails, updateSkillGroup);

        }
    }
}