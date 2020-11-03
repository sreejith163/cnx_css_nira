using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.UnitTest.Services
{
    public class ClientServiceShould : IClientService
    {
        public readonly List<Client> clients;

        public ClientServiceShould()
        {
            clients = new List<Client>()
            {
                new Client { Id=1, RefId=1,Name="A",CreatedBy="Admin",CreatedDate=DateTime.Now,ModifiedBy="",ModifiedDate=DateTime.Now,IsDeleted=false },
                new Client { Id=2,RefId=2,Name="B",CreatedBy="Admin",CreatedDate=DateTime.Now,ModifiedBy="",ModifiedDate=DateTime.Now,IsDeleted=false },
                new Client { Id=3,RefId=3,Name="C",CreatedBy="Admin",CreatedDate=DateTime.Now,ModifiedBy="",ModifiedDate=DateTime.Now,IsDeleted=false }
            };
        }

        public async Task<CSSResponse> CreateClient(CreateClient clientDetails)
        {
            clientDetails = new CreateClient()
            {
                Name = "D",
                RefId = 4,
                CreatedBy = "admin"
            };
            clients.Add(new Client { Name = clientDetails.Name, RefId = clientDetails.RefId, CreatedBy = clientDetails.CreatedBy });
            return new CSSResponse(clientDetails, HttpStatusCode.Created);
        }

        public async Task<CSSResponse> DeleteClient(ClientIdDetails clientIdDetails)
        {
            var client = clients.Where(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false).FirstOrDefault();
            if (client != null)
            {
                clients.Remove(client);
                return new CSSResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }
        }

        public async Task<CSSResponse> GetClient(ClientIdDetails clientIdDetails)
        {
            var client = clients.Where(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false).FirstOrDefault();
            if (client != null)
            {
                return new CSSResponse(client, HttpStatusCode.OK);
            }
            else
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }
        }

        public async Task<CSSResponse> GetClients(ClientQueryParameters clientParameters)
        {
            return new CSSResponse(clients, HttpStatusCode.OK);
        }

        public Task<CSSResponse> UpdateClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails)
        {
            throw new NotImplementedException();
        }
    }
}
