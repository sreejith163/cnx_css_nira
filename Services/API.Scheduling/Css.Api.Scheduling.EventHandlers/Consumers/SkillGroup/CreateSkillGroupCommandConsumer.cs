using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.SkillGroup;
using Css.Api.Core.EventBus.Events.SkillGroup;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Consumers.SkillGroup
{
    public class CreateSkillGroupCommandConsumer : IConsumer<CreateSkillGroupCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The skill group group repository</summary>
        private readonly ISkillGroupRepository _skillGroupGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="CreateSkillGroupCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="skillGroupRepository">The skill group repository.</param>
        /// <param name="uow">The uow.</param>
        public CreateSkillGroupCommandConsumer(IBusService busUtility, ISkillGroupRepository skillGroupRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _skillGroupGroupRepository = skillGroupRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<CreateSkillGroupCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.SkillGroup skillGroup = await _skillGroupGroupRepository.GetSkillGroup(new SkillGroupIdDetails
                {
                    SkillGroupId = context.Message.Id
                });

                if (skillGroup != null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                _skillGroupGroupRepository.CreateSkillGroup(
                        new Css.Api.Scheduling.Models.Domain.SkillGroup
                        {
                            SkillGroupId = context.Message.Id,
                            Name = context.Message.Name,
                            RefId = context.Message.RefId,
                            ClientId = context.Message.ClientId,
                            ClientLobGroupId = context.Message.ClientLobGroupId,
                            IsDeleted = false
                        }
                    );

                await _uow.Commit();

                await _busUtility.PublishEvent<ISkillGroupCreateSuccess>(MassTransitConstants.SkillGroupCreateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<CreateSkillGroupCommand> context)
        {
            await _busUtility.PublishEvent<ISkillGroupCreateFailed>(MassTransitConstants.SkillGroupCreateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.RefId,
                context.Message.ClientId,
                context.Message.ClientLobGroupId,
                context.Message.TimezoneId,
                context.Message.FirstDayOfWeek,
                context.Message.OperationHour,
                context.Message.ModifiedDate
            });
        }
    }
}