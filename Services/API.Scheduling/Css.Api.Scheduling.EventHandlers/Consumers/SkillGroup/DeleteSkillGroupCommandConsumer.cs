﻿using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
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
    public class DeleteSkillGroupCommandConsumer : IConsumer<DeleteSkillGroupCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The skillGroup name repository</summary>
        private readonly ISkillGroupRepository _skillGroupRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="DeleteSkillGroupCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="skillGroupRepository">The skillGroup repository.</param>
        /// <param name="uow">The uow.</param>
        public DeleteSkillGroupCommandConsumer(IBusService busUtility, ISkillGroupRepository skillGroupRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _skillGroupRepository = skillGroupRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<DeleteSkillGroupCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.SkillGroup skillGroup = await _skillGroupRepository.GetSkillGroup(new SkillGroupIdDetails
                {
                    SkillGroupId = context.Message.Id
                });

                if (skillGroup == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                skillGroup.SkillGroupId = context.Message.Id;
                skillGroup.IsDeleted = context.Message.IsDeletedNewValue;

                _skillGroupRepository.UpdateSkillGroup(skillGroup);

                await _uow.Commit();

                await _busUtility.PublishEvent<ISkillGroupDeleteSuccess>(MassTransitConstants.SkillGroupDeleteSuccessRouteKey, new
                {
                    context.Message.Id
                });
            }
            catch (Exception ex)
            {
                await PublishFailedEvent(context);
            }
        }

        /// <summary>Publishes the failed event.</summary>
        /// <param name="context">The context.</param>
        private async Task PublishFailedEvent(ConsumeContext<DeleteSkillGroupCommand> context)
        {
            await _busUtility.PublishEvent<ISkillGroupDeleteFailed>(MassTransitConstants.SkillGroupDeleteFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.RefId,
                context.Message.Name,
                context.Message.ClientId,
                context.Message.ClientLobGroupId,
                context.Message.TimezoneId,
                context.Message.FirstDayOfWeek,
                context.Message.OperationHour,
                context.Message.ModifiedByOldValue,
                context.Message.ModifiedDateOldValue,
                context.Message.IsDeletedOldValue
            });
        }
    }
}