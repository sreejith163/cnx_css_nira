using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ClientLOBGroupRepository : GenericRepository<ClientLobGroup>, IClientLOBGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The sort helper
        /// </summary>
        private readonly ISortHelper<ClientLOBGroupDTO> _sortHelper;

        /// <summary>
        /// The data shaper
        /// </summary>
        private readonly IDataShaper<ClientLOBGroupDTO> _dataShaper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLOBGroupRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="sortHelper">The sort helper.</param>
        /// <param name="dataShaper">The data shaper.</param>
        public ClientLOBGroupRepository(
            SchedulingContext repositoryContext,
            IMapper mapper,
            ISortHelper<ClientLOBGroupDTO> sortHelper,
            IDataShaper<ClientLOBGroupDTO> dataShaper)
            : base(repositoryContext)
        {
            _mapper = mapper;
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
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

            var filteredClientLOBGroups = FilterClientLOBGroups(clientLOBGroups, clientLOBGroupParameters.SearchKeyword, clientLOBGroupParameters.ClientId);

            var pagedClientLOBGroups = filteredClientLOBGroups
                .Skip((clientLOBGroupParameters.PageNumber - 1) * clientLOBGroupParameters.PageSize)
                .Take(clientLOBGroupParameters.PageSize)
                .Include(x => x.Client)
                .Include(x => x.Timezone);

            var mappedClientLOBGroups = pagedClientLOBGroups                
                .ProjectTo<ClientLOBGroupDTO>(_mapper.ConfigurationProvider);

            var sortedClientLOBGroups = _sortHelper.ApplySort(mappedClientLOBGroups, clientLOBGroupParameters.OrderBy);
            var shapedClientLOBGroups = _dataShaper.ShapeData(sortedClientLOBGroups, clientLOBGroupParameters.Fields);

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
