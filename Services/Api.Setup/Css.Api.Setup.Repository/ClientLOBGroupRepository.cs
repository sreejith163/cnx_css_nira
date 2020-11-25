using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.Client;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Setup.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository
{
    public class ClientLOBGroupRepository : EFCoreGenericRepository<ClientLobGroup>, IClientLOBGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLOBGroupRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientLOBGroupRepository(
            SetupContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLOBGroupParameters">The client lob group parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<PagedList<Entity>> GetClientLOBGroups(ClientLOBGroupQueryParameter clientLOBGroupParameters)
        {
            var clientLOBGroups = FindByCondition(x => x.IsDeleted == false);

            var filteredClientLOBGroups = FilterClientLOBGroups(clientLOBGroups, clientLOBGroupParameters.SearchKeyword, clientLOBGroupParameters.ClientId)
                .Include(x => x.Timezone);

            var sortedClientLOBGroups = SortHelper.ApplySort(filteredClientLOBGroups, clientLOBGroupParameters.OrderBy);

            var pagedClientLOBGroups = sortedClientLOBGroups
                .Skip((clientLOBGroupParameters.PageNumber - 1) * clientLOBGroupParameters.PageSize)
                .Take(clientLOBGroupParameters.PageSize)
                .Include(x => x.Client);

            var mappedClientLOBGroups = pagedClientLOBGroups
                .ProjectTo<ClientLOBGroupDTO>(_mapper.ConfigurationProvider);

            var shapedClientLOBGroups = DataShaper.ShapeData(mappedClientLOBGroups, clientLOBGroupParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedClientLOBGroups, filteredClientLOBGroups.Count(), clientLOBGroupParameters.PageNumber, clientLOBGroupParameters.PageSize);
        }

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<ClientLobGroup> GetClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails)
        {
            var clientLOBGroup = FindByCondition(x => x.Id == clientLOBGroupIdDetails.ClientLOBGroupId && x.IsDeleted == false)
                .Include(x => x.Client)
                .Include(x => x.Timezone)
                .SingleOrDefault();

            return await Task.FromResult(clientLOBGroup);
        }

        /// <summary>
        /// Gets the client lob groups count by client identifier.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<int> GetClientLOBGroupsCountByClientId(ClientIdDetails clientIdDetails)
        {
            var count = FindByCondition(x => x.ClientId == clientIdDetails.ClientId && x.IsDeleted == false)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Gets the name of the client lob groups identifier by client identifier and group.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientLOBGroupNameDetails">The client lob group name details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetClientLOBGroupsIdByClientIdAndGroupName(ClientIdDetails clientIdDetails, ClientLOBGroupNameDetails clientLOBGroupNameDetails)
        {
            var count = FindByCondition
                (x => x.ClientId == clientIdDetails.ClientId && string.Equals(x.Name, clientLOBGroupNameDetails.Name, StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        public void CreateClientLOBGroup(ClientLobGroup clientLOBGroupDetails)
        {
            Create(clientLOBGroupDetails);
        }

        /// <summary>
        /// Updates the client lob group.
        /// </summary>
        /// <param name="clientLobGroup">The client lob group.</param>
        public void UpdateClientLOBGroup(ClientLobGroup clientLobGroup)
        {
            Update(clientLobGroup);
        }

        /// <summary>Deletes the client lob group.</summary>
        /// <param name="clientLobGroup">The client lob group.</param>
        public void DeleteClientLOBGroup(ClientLobGroup clientLobGroup)
        {
            Delete(clientLobGroup);
        }

        /// <summary>
        /// Filters the client lob groups.
        /// </summary>
        /// <param name="clientLOBGroups">The client lob groups.</param>
        /// <param name="clientLOBGroupName">Name of the client lob group.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        private IQueryable<ClientLobGroup> FilterClientLOBGroups(IQueryable<ClientLobGroup> clientLOBGroups, string clientLOBGroupName, int? clientId)
        {
            if (!clientLOBGroups.Any())
            {
                return clientLOBGroups;
            }

            if (clientId.HasValue && clientId != default(int))
            {
                clientLOBGroups = clientLOBGroups.Where(x => x.ClientId == clientId);
            }

            if (!string.IsNullOrWhiteSpace(clientLOBGroupName))
            {
                clientLOBGroups = clientLOBGroups.Where(o => o.Name.ToLower().Contains(clientLOBGroupName.Trim().ToLower()));
            }

            return clientLOBGroups;
        }
    }
}
