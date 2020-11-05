using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockClientLOBGroupService
    {
        /// <summary>
        /// The clients
        /// </summary>
        public static List<ClientLobGroup> clientLOBGroups = new List<ClientLobGroup>()
        {
            new ClientLobGroup{Id=1,ClientId=1,ClientName="ClientA",FirstDayOfWeek=1,Name="A",IsDeleted=false,TimezoneId=1,RefId=1},
            new ClientLobGroup{Id=2,ClientId=2,ClientName="ClientB",FirstDayOfWeek=1,Name="B",IsDeleted=false,TimezoneId=1,RefId=1},
            new ClientLobGroup{Id=3,ClientId=3,ClientName="ClientC",FirstDayOfWeek=1,Name="C",IsDeleted=false,TimezoneId=1,RefId=1}
        };

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLOBGroupQueryParameters">The client lob group query parameters.</param>
        /// <returns></returns>
        public static CSSResponse GetClientLOBGroups(ClientLOBGroupQueryParameter clientLOBGroupQueryParameters)
        {
            return new CSSResponse(clientLOBGroups, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client lob group ok result.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns></returns>
        public static CSSResponse GetClientLOBGroupOKResult(ClientLOBGroupIdDetails clientLOBGroupId)
        {
            var client = clientLOBGroups.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            return new CSSResponse(client, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client lob group not found result.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns></returns>
        public static CSSResponse GetClientLOBGroupNotFoundResult(ClientLOBGroupIdDetails clientLOBGroupId)
        {
            var client = clientLOBGroups.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            return new CSSResponse(client, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="createClientLOBGroup">The create client lob group.</param>
        /// <returns></returns>
        public static CSSResponse CreateClientLOBGroup(CreateClientLOBGroup createClientLOBGroup)
        {
            ClientLobGroup clientLOBGroup = new ClientLobGroup()
            {
                Id = 4,
                ClientName = createClientLOBGroup.name,
                RefId = createClientLOBGroup.RefId,
                CreatedBy=createClientLOBGroup.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            clientLOBGroups.Add(clientLOBGroup);

            return new CSSResponse(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroup.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Deletes the client lob group ok result.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns></returns>
        public static CSSResponse DeleteClientLOBGroupOKResult(ClientLOBGroupIdDetails clientLOBGroupId)
        {
            var clientLOBGroup = clientLOBGroups.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            clientLOBGroups.Remove(clientLOBGroup);
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the client not found result.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static CSSResponse DeleteClientLOBGroupNotFoundResult(ClientLOBGroupIdDetails clientLOBGroupId)
        {
            return new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Updates the client ok result.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <param name="updateClientLOBGroup">The update client lob group.</param>
        /// <returns></returns>
        public static object UpdateClientOKResult(ClientLOBGroupIdDetails clientLOBGroupId, UpdateClientLOBGroup updateClientLOBGroup)
        {
            var clientLOBGroup = clientLOBGroups.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            clientLOBGroup.ModifiedBy = updateClientLOBGroup.ModifiedBy;
            clientLOBGroup.Name = updateClientLOBGroup.name;
            clientLOBGroup.ModifiedDate = DateTime.UtcNow;
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the client not found result.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <param name="updateClientLOBGroup">The update client lob group.</param>
        /// <returns></returns>
        public static object UpdateClientNotFoundResult(ClientLOBGroupIdDetails clientLOBGroupId, UpdateClientLOBGroup updateClientLOBGroup)
        {
            return new CSSResponse(HttpStatusCode.NotFound);
        }
    }
}
