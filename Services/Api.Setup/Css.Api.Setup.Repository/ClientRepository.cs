﻿using Css.Api.Core.Models.Domain;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.Client;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Css.Api.Setup.Models.DTO.Response.Client;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using Css.Api.Core.Utilities.Extensions;

namespace Css.Api.Setup.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientRepository(
            SetupContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetClients(ClientQueryParameters clientParameters)
        {
            var clients = FindByCondition(x => x.IsDeleted == false);

            var filteredClients = FilterClients(clients, clientParameters);

            var sortedClients = SortHelper.ApplySort(filteredClients, clientParameters.OrderBy);

            var pagedClients = sortedClients;

            if (!clientParameters.SkipPageSize)
            {
                pagedClients = sortedClients
                   .Skip((clientParameters.PageNumber - 1) * clientParameters.PageSize)
                   .Take(clientParameters.PageSize);
            }

            var mappedClients = pagedClients
                .ProjectTo<ClientDTO>(_mapper.ConfigurationProvider);

            var shapedClients = DataShaper.ShapeData(mappedClients, clientParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedClients, filteredClients.Count(), clientParameters.PageNumber, clientParameters.PageSize);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<Client> GetClient(ClientIdDetails clientIdDetails)
        {
            var client = FindByCondition(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false)
                .SingleOrDefault();

            return await Task.FromResult(client);
        }

        /// <summary>Gets all client.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Client> GetAllClient(ClientIdDetails clientIdDetails)
        {
            var client = FindByCondition(x => x.Id == clientIdDetails.ClientId )
                .SingleOrDefault();

            return await Task.FromResult(client);
        }

        /// <summary>
        /// Gets the name of the client by.
        /// </summary>
        /// <param name="clientNameDetails">The client name details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetClientsByName(ClientNameDetails clientNameDetails)
        {
            var clients = FindByCondition(x => string.Equals(x.Name.Trim(), clientNameDetails.Name.Trim(), StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(clients);
        }

        /// <summary>
        /// Gets the name of the client by name or refid.
        /// </summary>
        /// <param name="clientAttributes"></param>
        /// <returns></returns>
        public async Task<List<Client>> GetClientsByNameOrRefId(ClientAttributes clientAttributes)
        {
            var clients = FindByCondition(x => (string.Equals(x.Name.Trim(), clientAttributes.Name.Trim(), StringComparison.OrdinalIgnoreCase) || x.RefId == (clientAttributes.RefId ?? 0)) && x.IsDeleted == false)
                .ToList();

            return await Task.FromResult(clients);
        }

        /// <summary>Gets the name of all clients by.</summary>
        /// <param name="clientNameDetails">The client name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<int>> GetAllClientsByName(ClientNameDetails clientNameDetails)
        {
            var clients = FindByCondition(x => string.Equals(x.Name.Trim(), clientNameDetails.Name.Trim(), StringComparison.OrdinalIgnoreCase) )
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(clients);
        }   

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void CreateClient(Client client)
        {
            Create(client);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void UpdateClient(Client client)
        {
            Update(client);
        }

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void DeleteClient(Client client)
        {
            Delete(client);
        }

        /// <summary>
        /// Filters the clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        private IQueryable<Client> FilterClients(IQueryable<Client> clients, ClientQueryParameters clientParameters)
        {
            if (!clients.Any() || string.IsNullOrWhiteSpace(clientParameters.SearchKeyword))
            {
                return clients;
            }

            return clients.Where(o => o.Name.ToLower().Contains(clientParameters.SearchKeyword.Trim().ToLower()));
        }
    }
}
