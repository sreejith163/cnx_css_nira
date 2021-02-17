using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.SchedulingCode
{
    public class SchedulingCodeUpdateFailedConsumer : IConsumer<ISchedulingCodeUpdateFailed>
    {
        /// <summary>
        /// The schedulingCode service
        /// </summary>
        private readonly ISchedulingCodeService _schedulingCodeService;

        /// <summary>Initializes a new instance of the <see cref="SchedulingCodeUpdateFailedConsumer" /> class.</summary>
        /// <param name="schedulingCodeService">The schedulingCode service.</param>
        public SchedulingCodeUpdateFailedConsumer(ISchedulingCodeService schedulingCodeService)
        {
            _schedulingCodeService = schedulingCodeService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISchedulingCodeUpdateFailed> context)
        {
            SchedulingCodeIdDetails schedulingCodeIdDetails = new SchedulingCodeIdDetails
            {
                SchedulingCodeId = context.Message.Id
            };

            UpdateSchedulingCode updateSchedulingCode = new UpdateSchedulingCode
            {
                Description = context.Message.NameOldValue,
                PriorityNumber = context.Message.PriorityNumberOldValue,
                TimeOffCode = context.Message.TimeOffCodeOldValue,
                IconId = context.Message.IconIdOldValue,
                SchedulingTypeCode =
                JsonConvert.DeserializeObject<List<SchedulingCodeTypes>>(context.Message.SchedulingTypeCodeOldValue),
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _schedulingCodeService.UpdateSchedulingCode(schedulingCodeIdDetails, updateSchedulingCode);

        }
    }
}


