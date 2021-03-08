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
    public class ClientLOBGroupRepository : GenericRepository<ClientLobGroup>, IClientLOBGroupRepository
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

            var filteredClientLOBGroups = FilterClientLOBGroups(clientLOBGroups, clientLOBGroupParameters)
                .Include(x => x.Timezone);

            var sortedClientLOBGroups = SortHelper.ApplySort(filteredClientLOBGroups, clientLOBGroupParameters.OrderBy);

            var pagedClientLOBGroups = sortedClientLOBGroups;

            if (!clientLOBGroupParameters.SkipPageSize)
            {
                pagedClientLOBGroups = sortedClientLOBGroups
                   .Skip((clientLOBGroupParameters.PageNumber - 1) * clientLOBGroupParameters.PageSize)
                   .Take(clientLOBGroupParameters.PageSize)
                   .Include(x => x.Client);
            }

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

        /// <summary>Gets all client lob group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<ClientLobGroup> GetAllClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails)
        {
            var clientLOBGroup = FindByCondition(x => x.Id == clientLOBGroupIdDetails.ClientLOBGroupId )
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
                (x => x.ClientId == clientIdDetails.ClientId && string.Equals(x.Name.Trim(), clientLOBGroupNameDetails.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Gets the name of the client lob groups identifier by client identifier and group or refid.
        /// </summary>
        /// <param name="clientAttributes"></param>
        /// <returns></returns>
        public async Task<List<ClientLobGroup>> GetClientLOBGroupsIdByClientIdAndGroupNameOrRefId(ClientLOBGroupAttribute clientLOBGroupAttribute)
        {
            var clientLOBGroups = FindByCondition(x => x.ClientId == clientLOBGroupAttribute.ClientId && (string.Equals(x.Name.Trim(), clientLOBGroupAttribute.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase) || x.RefId == (clientLOBGroupAttribute.RefId ?? 0)) && x.IsDeleted == false).ToList();

            return await Task.FromResult(clientLOBGroups);
        }

        /// <summary>Gets the name of all client lob groups identifier by client identifier and group.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientLOBGroupNameDetails">The client lob group name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<int>> GetAllClientLOBGroupsIdByClientIdAndGroupName(ClientIdDetails clientIdDetails, ClientLOBGroupNameDetails clientLOBGroupNameDetails)
        {
            var count = FindByCondition
                (x => x.ClientId == clientIdDetails.ClientId && string.Equals(x.Name.Trim(), clientLOBGroupNameDetails.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase) )
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
        /// <param name="clientLOBGroupParameters">The client lob group parameters.</param>
        /// <returns></returns>
        private IQueryable<ClientLobGroup> FilterClientLOBGroups(IQueryable<ClientLobGroup> clientLOBGroups, ClientLOBGroupQueryParameter clientLOBGroupParameters)
        {
            if (!clientLOBGroups.Any())
            {
                return clientLOBGroups;
            }

            if (clientLOBGroupParameters.ClientId.HasValue && clientLOBGroupParameters.ClientId != default(int))
            {
                clientLOBGroups = clientLOBGroups.Where(x => x.ClientId == clientLOBGroupParameters.ClientId);
            }

            if (!string.IsNullOrWhiteSpace(clientLOBGroupParameters.SearchKeyword))
            {
                clientLOBGroups = clientLOBGroups.Where(o => o.Name.ToLower().Contains(clientLOBGroupParameters.SearchKeyword.Trim().ToLower()));
            }

            return clientLOBGroups;
        }
    }
}
