using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.SchedulingCode;
using Css.Api.Core.EventBus.Events.SchedulingCode;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.EventHandlers.Consumers.SchedulingCode
{
    public class UpdateSchedulingCodeCommandConsumer : IConsumer<UpdateSchedulingCodeCommand>
    {

        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The schedulingCode name repository</summary>
        private readonly ISchedulingCodeRepository _schedulingCodeRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;


        /// <summary>Initializes a new instance of the <see cref="UpdateSchedulingCodeCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="schedulingCodeRepository">The schedulingCode repository.</param>
        /// <param name="uow">The uow.</param>
        public UpdateSchedulingCodeCommandConsumer(IBusService busUtility, ISchedulingCodeRepository schedulingCodeRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _schedulingCodeRepository = schedulingCodeRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<UpdateSchedulingCodeCommand> context)
        {
            try
            {
                NoSQL.SchedulingCode schedulingCode = await _schedulingCodeRepository.GetSchedulingCode(new SchedulingCodeIdDetails
                {
                    SchedulingCodeId = context.Message.Id
                });

                if (schedulingCode == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                schedulingCode.SchedulingCodeId = context.Message.Id;
                schedulingCode.Name = context.Message.NameNewValue;
                schedulingCode.RefId = context.Message.RefIdNewValue;
                schedulingCode.TimeOffCode = context.Message.TimeOffCodeNewValue;
                schedulingCode.IsDeleted = context.Message.IsDeletedNewValue;

                _schedulingCodeRepository.UpdateSchedulingCode(schedulingCode);

                await _uow.Commit();

                await _busUtility.PublishEvent<ISchedulingCodeUpdateSuccess>(MassTransitConstants.SchedulingCodeUpdateSuccessRouteKey, new
                {
                    Id = context.Message.Id
                });
            }
            catch (Exception ex)
            {
                await PublishFailedEvent(context);
            }
        }

        /// <summary>Publishes the failed event.</summary>
        /// <param name="context">The context.</param>
        private async Task PublishFailedEvent(ConsumeContext<UpdateSchedulingCodeCommand> context)
        {
            await _busUtility.PublishEvent<ISchedulingCodeUpdateFailed>(MassTransitConstants.SchedulingCodeUpdateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.NameOldValue,
                context.Message.RefIdOldValue,
                context.Message.PriorityNumberOldValue,
                context.Message.TimeOffCodeOldValue,
                context.Message.IconIdOldValue,
                context.Message.SchedulingTypeCodeOldValue,
                context.Message.ModifiedByOldValue,
                context.Message.ModifiedDateOldValue,
                context.Message.IsDeletedOldValue
            });
        }
    }
}
