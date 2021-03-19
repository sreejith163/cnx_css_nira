using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.SchedulingCode
{
    /// <summary>
    /// The consumer class that consumes the event ISchedulingCodeCreateFailed
    /// </summary>
    public class SchedulingCodeCreateFailedConsumer : IConsumer<ISchedulingCodeCreateFailed>
    {
        /// <summary>
        /// The schedulingCode service
        /// </summary>
        private readonly ISchedulingCodeService _schedulingCodeService;

        /// <summary>Initializes a new instance of the <see cref="SchedulingCodeCreateFailedConsumer" /> class.</summary>
        /// <param name="schedulingCodeService">The schedulingCode service.</param>
        public SchedulingCodeCreateFailedConsumer(ISchedulingCodeService schedulingCodeService)
        {
            _schedulingCodeService = schedulingCodeService;
        }

        /// <summary>
        ///  The business implementation when the ISchedulingCodeCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISchedulingCodeCreateFailed> context)
        {
            SchedulingCodeIdDetails schedulingCodeIdDetails = new SchedulingCodeIdDetails
            {
                SchedulingCodeId = context.Message.Id
            };

            UpdateSchedulingCode updateSchedulingCode = new UpdateSchedulingCode
            {
                RefId = context.Message.RefId,
                Description = context.Message.Name,
                PriorityNumber = context.Message.PriorityNumber,
                TimeOffCode = context.Message.TimeOffCode,
                IconId = context.Message.IconId,
                ModifiedDate = context.Message.ModifiedDate,
                SchedulingTypeCode =
                    JsonConvert.DeserializeObject<List<SchedulingCodeTypes>>(context.Message.SchedulingTypeCode),
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _schedulingCodeService.UpdateSchedulingCode(schedulingCodeIdDetails, updateSchedulingCode);
        }
    }
}
