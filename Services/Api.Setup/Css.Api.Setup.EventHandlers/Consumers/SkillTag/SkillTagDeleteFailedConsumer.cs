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
    public class SkillTagDeleteFailedConsumer : IConsumer<ISkillTagDeleteFailed>
    {
        /// <summary>
        /// The skillTag service
        /// </summary>
        private readonly ISkillTagService _skillTagService;


        /// <summary>Initializes a new instance of the <see cref="SkillTagDeleteFailedConsumer" /> class.</summary>
        /// <param name="skillTagService">The skillTag service.</param>
        public SkillTagDeleteFailedConsumer(ISkillTagService skillTagService)
        {
            _skillTagService = skillTagService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISkillTagDeleteFailed> context)
        {
            SkillTagIdDetails skillTagIdDetails = new SkillTagIdDetails
            {
                SkillTagId = context.Message.Id
            };

            UpdateSkillTag updateSkillTag = new UpdateSkillTag
            {
                RefId = context.Message.RefId,
                Name = context.Message.Name,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                SkillGroupId = context.Message.SkillGroupId,
                OperationHour =
                    JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHour),
                IsUpdateRevert = true
            };

            await _skillTagService.RevertSkillTag(skillTagIdDetails, updateSkillTag);

        }
    }
}
