using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.SkillTag;
using Css.Api.Core.EventBus.Events.SkillTag;
using Css.Api.Core.EventBus.Services;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Repository.Interfaces;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Consumers.SkillTag
{
    public class CreateSkillTagCommandConsumer : IConsumer<CreateSkillTagCommand>
    {
        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The skill tag repository</summary>
        private readonly ISkillTagRepository _skillTagRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>Initializes a new instance of the <see cref="CreateSkillTagCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="skillTagRepository">The skill tag repository.</param>
        /// <param name="uow">The uow.</param>
        public CreateSkillTagCommandConsumer(IBusService busUtility, ISkillTagRepository skillTagRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _skillTagRepository = skillTagRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<CreateSkillTagCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.SkillTag skillTag = await _skillTagRepository.GetSkillTag(new SkillTagIdDetails
                {
                    SkillTagId = context.Message.Id
                });

                if (skillTag != null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                _skillTagRepository.CreateSkillTag(
                        new Css.Api.Scheduling.Models.Domain.SkillTag
                        {
                            SkillTagId = context.Message.Id,
                            Name = context.Message.Name,
                            RefId = context.Message.RefId,
                            ClientId = context.Message.ClientId,
                            ClientLobGroupId = context.Message.ClientLobGroupId,
                            SkillGroupId = context.Message.SkillGroupId,
                            IsDeleted = false
                        }
                    );

                await _uow.Commit();

                await _busUtility.PublishEvent<ISkillTagCreateSuccess>(MassTransitConstants.SkillTagCreateSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<CreateSkillTagCommand> context)
        {
            await _busUtility.PublishEvent<ISkillTagCreateFailed>(MassTransitConstants.SkillTagCreateFailedRouteKey, new
            {
                context.Message.Id,
                context.Message.Name,
                context.Message.RefId,
                context.Message.ClientId,
                context.Message.ClientLobGroupId,
                context.Message.SkillGroupId,
                context.Message.OperationHour,
                context.Message.ModifiedDate
            });
        }
    }
}