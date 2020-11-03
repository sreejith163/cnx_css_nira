using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
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

            var filteredClientLOBGroups =  SearchByLOBGroupName(clientLOBGroups, clientLOBGroupParameters.SearchKeyword);

            var pagedClientLOBGroups = filteredClientLOBGroups
                .Skip((clientLOBGroupParameters.PageNumber - 1) * clientLOBGroupParameters.PageSize)
                .Take(clientLOBGroupParameters.PageSize)
                .ToList();

            var mappedClientLOBGroups = pagedClientLOBGroups
                .AsQueryable()
                .ProjectTo<ClientLOBGroupDTO>(_mapper.ConfigurationProvider);

            var sortedClientLOBGroups = _sortHelper.ApplySort(mappedClientLOBGroups, clientLOBGroupParameters.OrderBy);
            var shapedClientLOBGroups = _dataShaper.ShapeData(sortedClientLOBGroups, clientLOBGroupParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedClientLOBGroups, clientLOBGroups.Count(), clientLOBGroupParameters.PageNumber, clientLOBGroupParameters.PageSize);
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
            var clientLOBGroup = FindByCondition(x => x.Id == clientLOBGroupIdDetails.ClientLOBGroupId && x.IsDeleted == false).
                SingleOrDefault();

            return await Task.FromResult(clientLOBGroup);
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

        /// <summary>Searches the name of the by lob group.</summary>
        /// <param name="clientLOBGroups">The client lob groups.</param>
        /// <param name="clientLOBGroupName">Name of the client lob group.</param>
        private IQueryable<ClientLobGroup> SearchByLOBGroupName(IQueryable<ClientLobGroup> clientLOBGroups, string clientLOBGroupName)
        {
            if (!clientLOBGroups.Any() || string.IsNullOrWhiteSpace(clientLOBGroupName))
                return clientLOBGroups;

            return clientLOBGroups.Where(o => o.Name.ToLower().Contains(clientLOBGroupName.Trim().ToLower()));
        }
    }
}

