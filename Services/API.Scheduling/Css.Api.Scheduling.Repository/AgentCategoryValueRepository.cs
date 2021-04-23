using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using Css.Api.Scheduling.Models.DTO.Response.AgentCategoryValueView;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentCategoryValueRepository : GenericRepository<Agent>, IAgentCategoryValueRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>Initializes a new instance of the <see cref="AgentCategoryValueRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentCategoryValueRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<Entity>> GetAgentCategoryValues(AgentCategoryValueQueryParameter agentCategoryValueQueryParameter)
        {
            var agentAdmins = FilterBy(x => x.IsDeleted == false);

            var filteredAgentAdmins = FilterAgentCategoryValue(agentAdmins, agentCategoryValueQueryParameter.AgentSchedulingGroupId, agentCategoryValueQueryParameter.AgentCategoryId);

            var sortedAgentAdmins = SortHelper.ApplySort(filteredAgentAdmins, agentCategoryValueQueryParameter.OrderBy);

            var pagedAgentAdmins = sortedAgentAdmins;

            if (!agentCategoryValueQueryParameter.SkipPageSize)
            {
                pagedAgentAdmins = sortedAgentAdmins
                   .Skip((agentCategoryValueQueryParameter.PageNumber - 1) * agentCategoryValueQueryParameter.PageSize)
                   .Take(agentCategoryValueQueryParameter.PageSize);
            }

            var mappedAgentAdmins = pagedAgentAdmins
            .ProjectTo<AgentCategoryValueDTO>(_mapper.ConfigurationProvider);

            var shapedAgentAdmins = DataShaper.ShapeData(mappedAgentAdmins, agentCategoryValueQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentAdmins, filteredAgentAdmins.Count(), agentCategoryValueQueryParameter.PageNumber, agentCategoryValueQueryParameter.PageSize);
        }

        private IQueryable<Agent> FilterAgentCategoryValue(IQueryable<Agent> agentAdmins, int agentSchedulingGroupId, int agentCategoryId)
        {
            if (!agentAdmins.Any())
            {
                return agentAdmins;
            }

            if (agentSchedulingGroupId != default(int))
            {
                agentAdmins = agentAdmins.Where(x => x.AgentSchedulingGroupId == agentSchedulingGroupId);
            }

            if (agentCategoryId != default(int))
            {
                agentAdmins = agentAdmins.Where(x => x.AgentCategoryValues.Any(acv => acv.CategoryId == agentCategoryId));
            }

            //if (!string.IsNullOrWhiteSpace(searchKeyword))
            //{
            //    agentAdmins = agentAdmins.ToList().Where(o => o.Sso.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
            //                                        o.FirstName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
            //                                        o.LastName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
            //                                        o.Ssn.ToString().Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)
            //                                        ).AsQueryable();
            //}

            return agentAdmins;
        }
    }
}
