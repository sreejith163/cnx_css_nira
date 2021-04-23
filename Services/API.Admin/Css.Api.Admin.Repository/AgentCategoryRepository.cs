using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Admin.Models.DTO.Response.AgentCategory;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class AgentCategoryRepository : GenericRepository<AgentCategory>, IAgentCategoryRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentCategoryRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agentCategories.
        /// </summary>
        /// <param name="agentCategoryParameters">The agentCategory parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentCategories(AgentCategoryQueryParameter agentCategoryParameters)
        {
            var agentCategories = FindByCondition(x => x.IsDeleted == false);

            var filteredAgentCategories = FilterAgentCategories(agentCategories, agentCategoryParameters);

            var sortedAgentCategories = SortHelper.ApplySort(filteredAgentCategories, agentCategoryParameters.OrderBy);

            var pagedAgentCategories = sortedAgentCategories;

            if (!agentCategoryParameters.SkipPageSize)
            {
                pagedAgentCategories = sortedAgentCategories
                .Skip((agentCategoryParameters.PageNumber - 1) * agentCategoryParameters.PageSize)
                .Take(agentCategoryParameters.PageSize);
            }

            var mappedAgentCategories = pagedAgentCategories
                .ProjectTo<AgentCategoryDTO>(_mapper.ConfigurationProvider);

            var shapedAgentCategories = DataShaper.ShapeData(mappedAgentCategories, agentCategoryParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentCategories, filteredAgentCategories.Count(), agentCategoryParameters.PageNumber, agentCategoryParameters.PageSize);
        }

        /// <summary>
        /// Gets the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <returns></returns>
        public async Task<AgentCategory> GetAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails)
        {
            var agentCategory = FindByCondition(x => x.Id == agentCategoryIdDetails.AgentCategoryId && x.IsDeleted == false)
                .Include(x => x.DataType)
                .SingleOrDefault();

            return await Task.FromResult(agentCategory);
        }




        /// <summary>
        /// Gets the name of the agentCategory by.
        /// </summary>
        /// <param name="agentCategoryNameDetails">The agentCategory name details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetAgentCategoriesByName(AgentCategoryNameDetails agentCategoryNameDetails)
        {
            var agentCategories = FindByCondition(x => string.Equals(x.Name.Trim(), agentCategoryNameDetails.Name.Trim(), StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(agentCategories);
        }

        /// <summary>
        /// Creates the agentCategory.
        /// </summary>
        /// <param name="agentCategory">The agentCategory.</param>
        public void CreateAgentCategory(AgentCategory agentCategory)
        {
            Create(agentCategory);
        }

        /// <summary>
        /// Updates the agentCategory.
        /// </summary>
        /// <param name="agentCategory">The agentCategory.</param>
        public void UpdateAgentCategory(AgentCategory agentCategory)
        {
            Update(agentCategory);
        }

        /// <summary>
        /// Deletes the agentCategory.
        /// </summary>
        /// <param name="agentCategory">The agentCategory.</param>
        public void DeleteAgentCategory(AgentCategory agentCategory)
        {
            Delete(agentCategory);
        }

        /// <summary>
        /// Filters the agentCategories.
        /// </summary>
        /// <param name="agentCategories">The agentCategories.</param>
        /// <param name="agentCategoryParameters">The agent category parameters.</param>
        /// <returns></returns>
        private IQueryable<AgentCategory> FilterAgentCategories(IQueryable<AgentCategory> agentCategories, AgentCategoryQueryParameter agentCategoryParameters)
        {
            if (!agentCategories.Any() || string.IsNullOrWhiteSpace(agentCategoryParameters.SearchKeyword))
            {
                return agentCategories;
            }

            return agentCategories.Where(o => o.Name.ToLower().Contains(agentCategoryParameters.SearchKeyword.Trim().ToLower()) ||
                                              o.CreatedBy.ToLower().Contains(agentCategoryParameters.SearchKeyword.Trim().ToLower()) ||
                                              o.ModifiedBy.ToLower().Contains(agentCategoryParameters.SearchKeyword.Trim().ToLower()));
        }
    }
}
