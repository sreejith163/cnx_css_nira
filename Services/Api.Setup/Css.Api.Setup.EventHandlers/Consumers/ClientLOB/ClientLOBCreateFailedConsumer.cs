using Css.Api.Core.EventBus.Events.ClientLOB;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Consumers.ClientLOB
{
    /// <summary>
    /// The consumer class that consumes the event IClientLOBCreateFailed
    /// </summary>
    public class ClientLOBCreateFailedConsumer : IConsumer<IClientLOBCreateFailed>
    {
        /// <summary>
        /// The clientLOB service
        /// </summary>
        private readonly IClientLOBGroupService _clientLOBService;

        /// <summary>Initializes a new instance of the <see cref="ClientLOBCreateFailedConsumer" /> class.</summary>
        /// <param name="clientLOBService">The clientLOB service.</param>
        public ClientLOBCreateFailedConsumer(IClientLOBGroupService clientLOBService)
        {
            _clientLOBService = clientLOBService;
        }

        /// <summary>
        ///  The business implementation when the IClientLOBCreateFailed occurs
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IClientLOBCreateFailed> context)
        {
            ClientLOBGroupIdDetails clientLOBIdDetails = new ClientLOBGroupIdDetails
            {
                ClientLOBGroupId = context.Message.Id
            };

            UpdateClientLOBGroup updateClientLOB = new UpdateClientLOBGroup
            {
                RefId = context.Message.RefId,
                Name = context.Message.Name,
                ClientId = context.Message.ClientId,
                TimezoneId = context.Message.TimezoneId,
                ModifiedDate = context.Message.ModifiedDate,
                IsDeleted = true,
                IsUpdateRevert = true
            };

            await _clientLOBService.UpdateClientLOBGroup(clientLOBIdDetails, updateClientLOB);
        }
    }
}
