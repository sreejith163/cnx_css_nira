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
    /// <summary>
    /// The consumer class that consumes the event ISkillTagCreateFailed
    /// </summary>
    public class SkillTagCreateFailedConsumer : IConsumer<ISkillTagCreateFailed>
    {
        /// <summary>
        /// The skillTag service
        /// </summary>
        private readonly ISkillTagService _skillTagService;

        /// <summary>Initializes a new instance of the <see cref="SkillTagCreateFailedConsumer" /> class.</summary>
        /// <param name="skillTagService">The skillTag service.</param>
        public SkillTagCreateFailedConsumer(ISkillTagService skillTagService)
        {
            _skillTagService = skillTagService;
        }

        /// <summary>
        ///  The business implementation when the ISkillTagCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISkillTagCreateFailed> context)
        {
            SkillTagIdDetails skillTagIdDetails = new SkillTagIdDetails
            {
                SkillTagId = context.Message.Id
            };

            UpdateSkillTag updateSkillTag = new UpdateSkillTag
            {
                RefId = context.Message.RefId,
                Name = context.Message.Name,
                SkillGroupId = context.Message.SkillGroupId,
                ModifiedDate = context.Message.ModifiedDate,
                OperationHour =
                    JsonConvert.DeserializeObject<List<OperationHourAttribute>>(context.Message.OperationHour),
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _skillTagService.UpdateSkillTag(skillTagIdDetails, updateSkillTag);
        }
    }
}

