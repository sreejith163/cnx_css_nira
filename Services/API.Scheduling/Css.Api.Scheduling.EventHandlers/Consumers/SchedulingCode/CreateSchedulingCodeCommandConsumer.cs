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
    public class CreateSchedulingCodeCommandConsumer : IConsumer<CreateSchedulingCodeCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The scheduling code repository</summary>
        private readonly ISchedulingCodeRepository _schedulingCodeGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="CreateSchedulingCodeCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="uow">The uow.</param>
        public CreateSchedulingCodeCommandConsumer(IBusService busUtility, ISchedulingCodeRepository schedulingCodeRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _schedulingCodeGroupRepository = schedulingCodeRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<CreateSchedulingCodeCommand> context)
        {
            try
            {
                NoSQL.SchedulingCode schedulingCode = await _schedulingCodeGroupRepository.GetSchedulingCode(new SchedulingCodeIdDetails
                {
                    SchedulingCodeId = context.Message.Id
                });

                if (schedulingCode != null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                _schedulingCodeGroupRepository.CreateSchedulingCode(
                        new Css.Api.Core.Models.Domain.NoSQL.SchedulingCode
                        {
                            SchedulingCodeId = context.Message.Id,
                            Name = context.Message.Name,
                            TimeOffCode = context.Message.TimeOffCode,
                            IsDeleted = false
                        }
                    );

                await _uow.Commit();

                await _busUtility.PublishEvent<ISchedulingCodeCreateSuccess>(MassTransitConstants.SchedulingCodeCreateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<CreateSchedulingCodeCommand> context)
        {
            await _busUtility.PublishEvent<ISchedulingCodeCreateFailed>(MassTransitConstants.SchedulingCodeCreateFailedRouteKey, new
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                PriorityNumber = context.Message.PriorityNumber,
                TimeOffCode = context.Message.TimeOffCode,
                IconId = context.Message.IconId,
                SchedulingTypeCode = context.Message.SchedulingTypeCode,
                ModifiedDate = context.Message.ModifiedDate
            });
        }
    }
}
