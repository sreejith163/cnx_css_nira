using Css.Api.Core.EventBus.Events.ClientLOB;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.ClientLOB
{
    public class ClientLOBDeleteFailedConsumer : IConsumer<IClientLOBDeleteFailed>
    {
        /// <summary>
        /// The clientLOB service
        /// </summary>
        private readonly IClientLOBGroupService _clientLOBService;


        /// <summary>Initializes a new instance of the <see cref="ClientLOBDeleteFailedConsumer" /> class.</summary>
        /// <param name="clientLOBService">The clientLOB service.</param>
        public ClientLOBDeleteFailedConsumer(IClientLOBGroupService clientLOBService)
        {
            _clientLOBService = clientLOBService;
        }

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<IClientLOBDeleteFailed> context)
        {
            ClientLOBGroupIdDetails clientLOBIdDetails = new ClientLOBGroupIdDetails
            {
                ClientLOBGroupId = context.Message.Id
            };

            UpdateClientLOBGroup updateClientLOB = new UpdateClientLOBGroup
            {
                Name = context.Message.Name,
                ModifiedBy = context.Message.ModifiedByOldValue,
                IsDeleted = context.Message.IsDeletedOldValue,
                ModifiedDate = context.Message.ModifiedDateOldValue,
                ClientId = context.Message.ClientId,
                TimezoneId = context.Message.TimezoneId,
                FirstDayOfWeek = context.Message.FirstDayOfWeek,
                IsUpdateRevert = true
            };

            await _clientLOBService.RevertClientLOBGroup(clientLOBIdDetails, updateClientLOB);

        }
    }
}