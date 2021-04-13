using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategory;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentCategoryRepository : GenericRepository<AgentCategory>, IAgentCategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentCategoryRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// Gets the agentCategorys.
        /// </summary>
        /// <param name="agentCategoryQueryparameter">The agentCategory queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentCategorys(AgentCategoryQueryparameter agentCategoryQueryparameter)
        {
            var agentCategorys = FilterBy(x => true);

            var filteredAgentCategorys = FilterAgentCategorys(agentCategorys, agentCategoryQueryparameter);

            var sortedAgentCategorys = SortHelper.ApplySort(filteredAgentCategorys, agentCategoryQueryparameter.OrderBy);

            var pagedAgentCategorys = sortedAgentCategorys;

            if (!agentCategoryQueryparameter.SkipPageSize)
            {
                pagedAgentCategorys = sortedAgentCategorys
                   .Skip((agentCategoryQueryparameter.PageNumber - 1) * agentCategoryQueryparameter.PageSize)
                   .Take(agentCategoryQueryparameter.PageSize);
            }

            var shapedAgentCategorys = DataShaper.ShapeData(pagedAgentCategorys, agentCategoryQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentCategorys, filteredAgentCategorys.Count(), agentCategoryQueryparameter.PageNumber, agentCategoryQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <returns></returns>
        public async Task<AgentCategory> GetAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails)
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false) &
                        Builders<AgentCategory>.Filter.Eq(i => i.AgentCategoryId, agentCategoryIdDetails.AgentCategoryId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the scheduling codes by ids.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns></returns>
        public async Task<long> GetAgentCategorysCountByIds(List<int> codes)
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false) & 
                        Builders<AgentCategory>.Filter.In(i => i.AgentCategoryId, codes);

            return await FindCountByIdAsync(query);
        }

        /// <summary>
        /// Finds the agent categorys.
        /// </summary>
        /// <returns></returns>
        public async Task<List<AgentCategory>> FindAgentCategorys()
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false);


            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// Gets the agentCategorys count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAgentCategorysCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the agentCategory.
        /// </summary>
        /// <param name="agentCategoryRequest">The agentCategory request.</param>
        public void CreateAgentCategory(AgentCategory agentCategoryRequest)
        {
            InsertOneAsync(agentCategoryRequest);
        }

        /// <summary>
        /// Updates the agentCategory.
        /// </summary>
        /// <param name="agentCategoryRequest">The agentCategory request.</param>
        public void UpdateAgentCategory(AgentCategory agentCategoryRequest)
        {
            ReplaceOneAsync(agentCategoryRequest);
        }

        /// <summary>
        /// Filters the agentCategorys.
        /// </summary>
        /// <param name="agentCategorys">The agentCategorys.</param>
        /// <param name="agentCategoryQueryparameter">The agentCategory queryparameter.</param>
        /// <returns></returns>
        private IQueryable<AgentCategory> FilterAgentCategorys(IQueryable<AgentCategory> agentCategorys, AgentCategoryQueryparameter agentCategoryQueryparameter)
        {
            if (!agentCategorys.Any())
            {
                return agentCategorys;
            }

            if (!string.IsNullOrWhiteSpace(agentCategoryQueryparameter.SearchKeyword))
            {
                agentCategorys = agentCategorys.Where(o => o.Name.ToLower().Contains(agentCategoryQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            return agentCategorys;
        }
    }
}


