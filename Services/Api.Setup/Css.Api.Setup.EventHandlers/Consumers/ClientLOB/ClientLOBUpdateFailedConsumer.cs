using Css.Api.Core.EventBus.Events.ClientLOB;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.ClientLOB
{
    public class ClientLOBUpdateFailedConsumer : IConsumer<IClientLOBUpdateFailed>
    {
        /// <summary>
        /// The clientLOB service
        /// </summary>
        private readonly IClientLOBGroupService _clientLOBService;

        /// <summary>Initializes a new instance of the <see cref="ClientLOBUpdateFailedConsumer" /> class.</summary>
        /// <param name="clientLOBService">The clientLOB service.</param>
        public ClientLOBUpdateFailedConsumer(IClientLOBGroupService clientLOBService)
        {
            _clientLOBService = clientLOBService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IClientLOBUpdateFailed> context)
        {
            ClientLOBGroupIdDetails clientLOBIdDetails = new ClientLOBGroupIdDetails
            {
                ClientLOBGroupId = context.Message.Id
            };

            UpdateClientLOBGroup updateClientLOB = new UpdateClientLOBGroup
            {
                Name = context.Message.NameOldValue,
                ClientId = context.Message.ClientIdOldValue,
                TimezoneId = context.Message.TimezoneIdOldValue,
                FirstDayOfWeek = context.Message.FirstDayOfWeekOldValue,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                IsUpdateRevert = true
            };

            await _clientLOBService.UpdateClientLOBGroup(clientLOBIdDetails, updateClientLOB);

        }
    }
}

