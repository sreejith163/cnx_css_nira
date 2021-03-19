using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.SchedulingCode
{
    public class SchedulingCodeDeleteFailedConsumer : IConsumer<ISchedulingCodeDeleteFailed>
    {
        /// <summary>
        /// The schedulingCode service
        /// </summary>
        private readonly ISchedulingCodeService _schedulingCodeService;


        /// <summary>Initializes a new instance of the <see cref="SchedulingCodeDeleteFailedConsumer" /> class.</summary>
        /// <param name="schedulingCodeService">The schedulingCode service.</param>
        public SchedulingCodeDeleteFailedConsumer(ISchedulingCodeService schedulingCodeService)
        {
            _schedulingCodeService = schedulingCodeService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISchedulingCodeDeleteFailed> context)
        {
            SchedulingCodeIdDetails schedulingCodeIdDetails = new SchedulingCodeIdDetails
            {
                SchedulingCodeId = context.Message.Id
            };

            UpdateSchedulingCode updateSchedulingCode = new UpdateSchedulingCode
            {
                RefId = context.Message.RefId,
                Description = context.Message.Name,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                PriorityNumber = context.Message.PriorityNumber,
                TimeOffCode = context.Message.TimeOffCode,
                IconId = context.Message.IconId,
                SchedulingTypeCode =
                    JsonConvert.DeserializeObject<List<SchedulingCodeTypes>>(context.Message.SchedulingTypeCode),
                IsUpdateRevert = true
            };

            await _schedulingCodeService.RevertSchedulingCode(schedulingCodeIdDetails, updateSchedulingCode);

        }
    }
}