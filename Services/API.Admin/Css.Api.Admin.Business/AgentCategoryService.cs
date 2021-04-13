using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Admin.Models.DTO.Response.AgentCategory;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentCategory;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    /// <summary>
    /// Service for agentCategory part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.IAgentCategoryService" />
    public class AgentCategoryService : IAgentCategoryService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The bus
        /// </summary>
        private readonly IBusService _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="bus">The bus.</param>
        public AgentCategoryService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IBusService bus)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _bus = bus;
        }

        /// <summary>
        /// Gets the agent categories.
        /// </summary>
        /// <param name="agentCategoryQueryParameter">The agentCategory query parameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentCategories(AgentCategoryQueryParameter agentCategoryQueryParameter)
        {
            var agentCategories = await _repository.AgentCategories.GetAgentCategories(agentCategoryQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentCategories));

            return new CSSResponse(agentCategories, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails)
        {
            var agentCategory = await _repository.AgentCategories.GetAgentCategory(agentCategoryIdDetails);
            if (agentCategory == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentCategory = _mapper.Map<AgentCategoryDTO>(agentCategory);
            return new CSSResponse(mappedAgentCategory, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the agentCategory.
        /// </summary>
        /// <param name="agentCategoryDetails">The agentCategory details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateAgentCategory(CreateAgentCategory agentCategoryDetails)
        {
            var agentCategories = await _repository.AgentCategories.GetAgentCategoriesByName(new AgentCategoryNameDetails { Name = agentCategoryDetails.Name });
            if (agentCategories?.Count > 0)
            {
                return new CSSResponse($"Agent Category with name '{agentCategoryDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentCategoryRequest = _mapper.Map<AgentCategory>(agentCategoryDetails);
            _repository.AgentCategories.CreateAgentCategory(agentCategoryRequest);

            await _repository.SaveAsync();

            await _bus.SendCommand<CreateAgentCategoryCommand>(MassTransitConstants.AgentCategoryCreateCommandRouteKey,
                new
                {
                    agentCategoryRequest.Id,
                    agentCategoryRequest.Name,
                    agentCategoryType = agentCategoryRequest.DataTypeId,
                    agentCategoryRequest.DataTypeMinValue,
                    agentCategoryRequest.DataTypeMaxValue,
                    agentCategoryRequest.ModifiedDate,
                });

            return new CSSResponse(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <param name="agentCategoryDetails">The agentCategory details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails, UpdateAgentCategory agentCategoryDetails)
        {
            var agentCategory = await _repository.AgentCategories.GetAgentCategory(agentCategoryIdDetails);
            if (agentCategory == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentCategories = await _repository.AgentCategories.GetAgentCategoriesByName(new AgentCategoryNameDetails { Name = agentCategoryDetails.Name });
            if (agentCategories?.Count > 0 && agentCategories.IndexOf(agentCategoryIdDetails.AgentCategoryId) == -1)
            {
                return new CSSResponse($"Agent Category with name '{agentCategoryDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentCategoryDetailsPreUpdate = new AgentCategory
            {
                Name = agentCategory.Name,
                DataTypeId = agentCategory.DataTypeId,
                DataTypeMinValue = agentCategory.DataTypeMinValue,
                DataTypeMaxValue = agentCategory.DataTypeMaxValue,                
                ModifiedBy = agentCategory.ModifiedBy,
                IsDeleted = agentCategory.IsDeleted,
                ModifiedDate = agentCategory.ModifiedDate
            };

            var agentCategoryRequest = _mapper.Map(agentCategoryDetails, agentCategory);

            _repository.AgentCategories.UpdateAgentCategory(agentCategoryRequest);

            await _repository.SaveAsync();

            if (!agentCategoryDetails.IsUpdateRevert)
            {
                await _bus.SendCommand<UpdateAgentCategoryCommand>(
                    MassTransitConstants.AgentCategoryUpdateCommandRouteKey,
                    new
                    {
                        agentCategoryRequest.Id,
                        NameOldValue = agentCategoryDetailsPreUpdate.Name,
                        AgentCategoryTypeOldValue = agentCategoryDetailsPreUpdate.DataTypeId,
                        DataTypeMinValueOldValue = agentCategoryDetailsPreUpdate.DataTypeMinValue,
                        DataTypeMaxValueOldValue = agentCategoryDetailsPreUpdate.DataTypeMaxValue,
                        ModifiedByOldValue = agentCategoryDetailsPreUpdate.ModifiedBy,
                        ModifiedDateOldValue = agentCategoryDetailsPreUpdate.ModifiedDate,
                        IsDeletedOldValue = agentCategoryDetailsPreUpdate.IsDeleted,
                        NameNewValue = agentCategoryRequest.Name,
                        AgentCategoryTypeNewValue = agentCategoryRequest.DataTypeId,
                        DataTypeMinValueNewValue = agentCategoryRequest.DataTypeMinValue,
                        DataTypeMaxValueNewValue = agentCategoryRequest.DataTypeMaxValue,
                        ModifiedByNewValue = agentCategoryRequest.ModifiedBy,
                        ModifiedDateNewValue = agentCategoryRequest.ModifiedDate,
                        IsDeletedNewValue = agentCategoryRequest.IsDeleted
                    });
            }
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Reverts the agent category.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agent category identifier details.</param>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> RevertAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails, UpdateAgentCategory agentCategoryDetails)
        {
            var agentCategory = await _repository.AgentCategories.GetAgentCategory(agentCategoryIdDetails);
            if (agentCategory == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentCategories = await _repository.AgentCategories.GetAgentCategoriesByName(new AgentCategoryNameDetails { Name = agentCategoryDetails.Name });
            if (agentCategories?.Count > 0 && agentCategories.IndexOf(agentCategoryIdDetails.AgentCategoryId) == -1)
            {
                return new CSSResponse($"Client with name '{agentCategoryDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentCategoryRequest = _mapper.Map(agentCategoryDetails, agentCategory);
            agentCategoryRequest.ModifiedDate = agentCategoryDetails.ModifiedDate;
            _repository.AgentCategories.UpdateAgentCategory(agentCategoryRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails)
        {
            var agentCategory = await _repository.AgentCategories.GetAgentCategory(agentCategoryIdDetails);
            if (agentCategory == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentCategoryDetailsPreUpdate = new AgentCategory
            {
                Name = agentCategory.Name,
                DataTypeId = agentCategory.DataTypeId,
                DataTypeMinValue = agentCategory.DataTypeMinValue,
                DataTypeMaxValue = agentCategory.DataTypeMaxValue,
                ModifiedBy = agentCategory.ModifiedBy,
                IsDeleted = agentCategory.IsDeleted,
                ModifiedDate = agentCategory.ModifiedDate
            };

            agentCategory.IsDeleted = true;

            _repository.AgentCategories.UpdateAgentCategory(agentCategory);
            await _repository.SaveAsync();

            await _bus.SendCommand<DeleteAgentCategoryCommand>(
                MassTransitConstants.AgentCategoryDeleteCommandRouteKey,
                new
                {
                    agentCategory.Id,
                    agentCategory.Name,
                    agentCategory.DataTypeId,
                    agentCategory.DataTypeMinValue,
                    agentCategory.DataTypeMaxValue,
                    ModifiedByOldValue = agentCategoryDetailsPreUpdate.ModifiedBy,
                    IsDeletedOldValue = agentCategoryDetailsPreUpdate.IsDeleted,
                    ModifiedDateOldValue = agentCategoryDetailsPreUpdate.ModifiedDate,
                    IsDeletedNewValue = agentCategory.IsDeleted
                });

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
