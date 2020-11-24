using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Setup.UnitTest.Mock
{
    public class MockClientLOBGroupData
    {
        /// <summary>
        /// The client lob groups
        /// </summary>
        private List<ClientLobGroup> clientLOBGroupsDB = new List<ClientLobGroup>()
        {
            new ClientLobGroup{ Id = 1, ClientId = 1, FirstDayOfWeek = 1, Name = "A", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 2, ClientId = 2, FirstDayOfWeek = 1, Name = "B", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 3, ClientId = 3, FirstDayOfWeek = 1, Name = "C", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 4, ClientId = 2, FirstDayOfWeek = 1, Name = "D", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        };

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLOBGroupQueryParameters">The client lob group query parameters.</param>
        /// <returns></returns>
        public CSSResponse GetClientLOBGroups(ClientLOBGroupQueryParameter clientLOBGroupQueryParameters)
        {
            var clientLOBGroups = clientLOBGroupsDB.Skip((clientLOBGroupQueryParameters.PageNumber - 1) * clientLOBGroupQueryParameters.PageSize).Take(clientLOBGroupQueryParameters.PageSize);
            return new CSSResponse(clientLOBGroups, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns></returns>
        public CSSResponse GetClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupId)
        {
            var clientLOBGroup = clientLOBGroupsDB.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            return clientLOBGroup != null ? new CSSResponse(clientLOBGroup, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="createClientLOBGroup">The create client lob group.</param>
        /// <returns></returns>
        public CSSResponse CreateClientLOBGroup(CreateClientLOBGroup createClientLOBGroup)
        {
            if (clientLOBGroupsDB.Exists(x => x.IsDeleted == false && x.Name == createClientLOBGroup.Name && x.ClientId == createClientLOBGroup.ClientId))
            {
                return new CSSResponse($"Client LOB Group with name '{createClientLOBGroup.Name}' already exists.", HttpStatusCode.Conflict);
            }

            ClientLobGroup clientLOBGroup = new ClientLobGroup()
            {
                Id = 4,
                Name = createClientLOBGroup.Name,
                RefId = createClientLOBGroup.RefId,
                CreatedBy=createClientLOBGroup.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            clientLOBGroupsDB.Add(clientLOBGroup);

            return new CSSResponse(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroup.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the client ok result.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <param name="updateClientLOBGroup">The update client lob group.</param>
        /// <returns></returns>
        public CSSResponse UpdateClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupId, UpdateClientLOBGroup updateClientLOBGroup)
        {
            if (!clientLOBGroupsDB.Exists(x => x.IsDeleted == false && x.Id == clientLOBGroupId.ClientLOBGroupId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (clientLOBGroupsDB.Exists(x => x.IsDeleted == false && x.Name == updateClientLOBGroup.Name && x.ClientId == updateClientLOBGroup.ClientId && x.Id != clientLOBGroupId.ClientLOBGroupId))
            {
                return new CSSResponse($"Client LOB Group with name '{updateClientLOBGroup.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientLOBGroup = clientLOBGroupsDB.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            clientLOBGroup.ModifiedBy = updateClientLOBGroup.ModifiedBy;
            clientLOBGroup.Name = updateClientLOBGroup.Name;
            clientLOBGroup.ModifiedDate = DateTime.UtcNow;
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns></returns>
        public CSSResponse DeleteClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupId)
        {
            if (!clientLOBGroupsDB.Exists(x => x.IsDeleted == false && x.Id == clientLOBGroupId.ClientLOBGroupId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientLOBGroup = clientLOBGroupsDB.Where(x => x.Id == clientLOBGroupId.ClientLOBGroupId && x.IsDeleted == false).FirstOrDefault();
            clientLOBGroupsDB.Remove(clientLOBGroup);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
