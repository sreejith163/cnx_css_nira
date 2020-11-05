using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockClientService
    {
        /// <summary>
        /// The clients
        /// </summary>
        public static List<Client> clients = new List<Client>()
        {
            new Client { Id=1,RefId=1,Name="A",CreatedBy="Admin",CreatedDate=DateTime.Now,ModifiedBy="",ModifiedDate=DateTime.Now,IsDeleted=false },
            new Client { Id=2,RefId=2,Name="B",CreatedBy="Admin",CreatedDate=DateTime.Now,ModifiedBy="",ModifiedDate=DateTime.Now,IsDeleted=false },
            new Client { Id=3,RefId=3,Name="C",CreatedBy="Admin",CreatedDate=DateTime.Now,ModifiedBy="",ModifiedDate=DateTime.Now,IsDeleted=false }
        };

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        public static CSSResponse GetClients(ClientQueryParameters clientParameters)
        {
            return new CSSResponse(clients, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client ok result.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static CSSResponse GetClientOKResult(ClientIdDetails clientId)
        {
            var client = clients.Where(x => x.Id == clientId.ClientId && x.IsDeleted == false).FirstOrDefault();
            return new CSSResponse(client, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client not found result.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static CSSResponse GetClientNotFoundResult(ClientIdDetails clientId)
        {
            var client = clients.Where(x => x.Id == clientId.ClientId && x.IsDeleted == false).FirstOrDefault();
            return new CSSResponse(client, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="createClient">The create client.</param>
        /// <returns></returns>
        public static CSSResponse CreateClient(CreateClient createClient)
        {
            Client client = new Client()
            {
                Id = 4,
                Name = createClient.Name,
                RefId = createClient.RefId,
                CreatedBy = createClient.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            clients.Add(client);

            return new CSSResponse(new ClientIdDetails { ClientId = client.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Deletes the client ok result.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static CSSResponse DeleteClientOKResult(ClientIdDetails clientId)
        {
            var client = clients.Where(x => x.Id == clientId.ClientId && x.IsDeleted == false).FirstOrDefault();
            clients.Remove(client);
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the client not found result.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static CSSResponse DeleteClientNotFoundResult(ClientIdDetails clientId)
        {
            return new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Updates the client ok result.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="updateClient">The update client.</param>
        /// <returns></returns>
        public static object UpdateClientOKResult(ClientIdDetails clientIdDetails,UpdateClient updateClient)
        {
            var client = clients.Where(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false).FirstOrDefault();
            client.ModifiedBy = updateClient.ModifiedBy;
            client.Name = updateClient.Name;
            client.ModifiedDate = DateTime.UtcNow;
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the client not found result.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="updateClient">The update client.</param>
        /// <returns></returns>
        public static object UpdateClientNotFoundResult(ClientIdDetails clientIdDetails, UpdateClient updateClient)
        {
            return new CSSResponse(HttpStatusCode.NotFound);
        }
    }
}
