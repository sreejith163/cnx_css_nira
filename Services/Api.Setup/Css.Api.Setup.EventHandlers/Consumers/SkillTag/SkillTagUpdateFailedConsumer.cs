using Css.Api.Core.EventBus.Events.SkillTag;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using MassTransit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.SkillTag
{
    public class SkillTagUpdateFailedConsumer : IConsumer<ISkillTagUpdateFailed>
    {
        /// <summary>
        /// The skillTag service
        /// </summary>
        private readonly ISkillTagService _skillTagService;

        /// <summary>Initializes a new instance of the <see cref="SkillTagUpdateFailedConsumer" /> class.</summary>
        /// <param name="skillTagService">The skillTag service.</param>
        public SkillTagUpdateFailedConsumer(ISkillTagService skillTagService)
        {
            _skillTagService = skillTagService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISkillTagUpdateFailed> context)
        {
            SkillTagIdDetails skillTagIdDetails = new SkillTagIdDetails
            {
                SkillTagId = context.Message.Id
            };

            UpdateSkillTag updateSkillTag = new UpdateSkillTag
            {
                Name = context.Message.NameOldValue,
                SkillGroupId = context.Message.SkillGroupIdOldValue,
                OperationHour =
                JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHourOldValue),
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _skillTagService.UpdateSkillTag(skillTagIdDetails, updateSkillTag);

        }
    }
}


