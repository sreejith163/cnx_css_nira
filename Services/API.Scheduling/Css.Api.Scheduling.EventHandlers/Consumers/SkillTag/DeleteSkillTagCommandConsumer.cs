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
    public class DeleteSkillTagCommandConsumer : IConsumer<DeleteSkillTagCommand>
    {

        /// <summary>The bus utility</summary>
        private readonly IBusService _busUtility;

        /// <summary>The skillTag name repository</summary>
        private readonly ISkillTagRepository _skillTagRepository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;


        /// <summary>Initializes a new instance of the <see cref="DeleteSkillTagCommandConsumer" /> class.</summary>
        /// <param name="busUtility">The bus utility.</param>
        /// <param name="skillTagRepository">The skillTag repository.</param>
        /// <param name="uow">The uow.</param>
        public DeleteSkillTagCommandConsumer(IBusService busUtility, ISkillTagRepository skillTagRepository, IUnitOfWork uow)
        {
            _busUtility = busUtility;
            _skillTagRepository = skillTagRepository;
            _uow = uow;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <exception cref="Exception"></exception>
        public async Task Consume(ConsumeContext<DeleteSkillTagCommand> context)
        {
            try
            {
                Css.Api.Scheduling.Models.Domain.SkillTag skillTag = await _skillTagRepository.GetSkillTag(new SkillTagIdDetails
                {
                    SkillTagId = context.Message.Id
                });

                if (skillTag == null)
                {
                    await PublishFailedEvent(context);
                    return;
                }

                skillTag.SkillTagId = context.Message.Id;
                skillTag.IsDeleted = context.Message.IsDeletedNewValue;

                _skillTagRepository.UpdateSkillTag(skillTag);

                await _uow.Commit();

                await _busUtility.PublishEvent<ISkillTagDeleteSuccess>(MassTransitConstants.SkillTagDeleteSuccessRouteKey, new
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
        private async Task PublishFailedEvent(ConsumeContext<DeleteSkillTagCommand> context)
        {
            await _busUtility.PublishEvent<ISkillTagDeleteFailed>(MassTransitConstants.SkillTagDeleteFailedRouteKey, new
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                ClientId = context.Message.ClientId,
                ClientLobGroupId = context.Message.ClientLobGroupId,
                SkillGroupId = context.Message.SkillGroupId,
                OperationHour = context.Message.OperationHour,
                ModifiedByOldValue = context.Message.ModifiedByOldValue,
                ModifiedDateOldValue = context.Message.ModifiedDateOldValue,
                IsDeletedOldValue = context.Message.IsDeletedOldValue
            });
        }
    }
}
