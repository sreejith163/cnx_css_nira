using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Setup.UnitTest.Mock
{
    public class MockClientData
    {
        /// <summary>
        /// The clients
        /// </summary>
        private  List<Client> clientsDB = new List<Client>()
        {
            new Client() { Id = 1, RefId = 1, Name= "A", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 2, RefId = 2, Name= "B", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 3, RefId = 3, Name= "C", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 4, RefId = 4, Name= "D", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 5, RefId = 5, Name= "E", CreatedBy = "Admin", CreatedDate = DateTime.Now }
        };

        /// <summary>
        /// The client lob groups
        /// </summary>
        private List<ClientLobGroup> clientLOBGroupsDB = new List<ClientLobGroup>()
        {
            new ClientLobGroup{ Id = 1, ClientId = 1, FirstDayOfWeek = 1, Name = "A", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 2, ClientId = 2, FirstDayOfWeek = 1, Name = "B", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 3, ClientId = 3, FirstDayOfWeek = 1, Name = "C", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        };

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        public CSSResponse GetClients(ClientQueryParameters clientParameters)
        {
            var clients = clientsDB.Skip((clientParameters.PageNumber - 1) * clientParameters.PageSize).Take(clientParameters.PageSize);
            return new CSSResponse(clients, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public CSSResponse GetClient(ClientIdDetails clientId)
        {
            var client = clientsDB.Where(x => x.Id == clientId.ClientId && x.IsDeleted == false).FirstOrDefault();
            return client != null ? new CSSResponse(client, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="createClient">The create client.</param>
        /// <returns></returns>
        public CSSResponse CreateClient(CreateClient createClient)
        {
            if (clientsDB.Exists(x => x.IsDeleted == false && x.Name == createClient.Name))
            {
                return new CSSResponse($"Client with name '{createClient.Name}' already exists.", HttpStatusCode.Conflict);
            }

            Client client = new Client()
            {
                Id = 4,
                Name = createClient.Name,
                RefId = createClient.RefId,
                CreatedBy = createClient.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            clientsDB.Add(client);

            return new CSSResponse(new ClientIdDetails { ClientId = client.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="updateClient">The update client.</param>
        /// <returns></returns>
        public CSSResponse UpdateClient(ClientIdDetails clientIdDetails, UpdateClient updateClient)
        {
            if (!clientsDB.Exists(x => x.IsDeleted == false && x.Id == clientIdDetails.ClientId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (clientsDB.Exists(x => x.IsDeleted == false && x.Name == updateClient.Name && x.Id != clientIdDetails.ClientId))
            {
                return new CSSResponse($"Client with name '{updateClient.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var client = clientsDB.Where(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false).FirstOrDefault();
            client.ModifiedBy = updateClient.ModifiedBy;
            client.Name = updateClient.Name;
            client.ModifiedDate = DateTime.UtcNow;
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteClient(ClientIdDetails clientIdDetails)
        {
            if (!clientsDB.Exists(x => x.IsDeleted == false && x.Id == clientIdDetails.ClientId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var client = clientsDB.Where(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false).FirstOrDefault();

            if (clientLOBGroupsDB.Exists(x => x.IsDeleted == false && x.ClientId == clientIdDetails.ClientId))
            {
                return new CSSResponse($"The client {client.Name} has dependency with other modules", HttpStatusCode.FailedDependency);
            }

            clientsDB.Remove(client);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
