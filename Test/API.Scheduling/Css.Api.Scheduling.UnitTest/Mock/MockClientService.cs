using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockClientService
    {
        /// <summary>
        /// The clients
        /// </summary>
        private  List<Client> clientsDB = new List<Client>()
        {
            new Client() { Id = 1, RefId = 1, Name= "A", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 2, RefId = 2, Name= "B", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 3, RefId = 3, Name= "C", CreatedBy = "Admin", CreatedDate = DateTime.Now }
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
            clientsDB.Remove(client);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
