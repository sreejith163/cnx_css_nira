using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockAgentCategoryData
    {
        /// <summary>
        /// The agentCategories
        /// </summary>
        private List<AgentCategory> agentCategoriesDB = new List<AgentCategory>()
        {
            new AgentCategory{ Id = 1, Name = "AgentCategory1", DataTypeId=1, DataTypeMinValue="Min", DataTypeMaxValue="Max", CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new AgentCategory{ Id = 2, Name = "AgentCategory2", DataTypeId=1, DataTypeMinValue="Min", DataTypeMaxValue="Max", CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new AgentCategory{ Id = 3, Name = "AgentCategory3", DataTypeId=1, DataTypeMinValue="Min", DataTypeMaxValue="Max", CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new AgentCategory{ Id = 4, Name = "AgentCategory4", DataTypeId=1, DataTypeMinValue="Min", DataTypeMaxValue="Max", CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new AgentCategory{ Id = 5, Name = "AgentCategory5", DataTypeId=1, DataTypeMinValue="Min", DataTypeMaxValue="Max", CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        };


        /// <summary>
        /// Gets the agentCategories.
        /// </summary>
        /// <param name="agentCategoryParameters">The agentCategory parameters.</param>
        /// <returns></returns>
        public CSSResponse GetAgentCategories(AgentCategoryQueryParameter agentCategoryParameters)
        {
            var agentCategories = agentCategoriesDB.Skip((agentCategoryParameters.PageNumber - 1) * agentCategoryParameters.PageSize).Take(agentCategoryParameters.PageSize);
            return new CSSResponse(agentCategories, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agentCategory.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        /// <returns></returns>
        public CSSResponse GetAgentCategory(AgentCategoryIdDetails agentCategoryId)
        {
            var agentCategory = agentCategoriesDB.Where(x => x.Id == agentCategoryId.AgentCategoryId && x.IsDeleted == false).FirstOrDefault();
            return agentCategory != null ? new CSSResponse(agentCategory, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the agentCategory.
        /// </summary>
        /// <param name="createAgentCategory">The create agentCategory.</param>
        /// <returns></returns>
        public CSSResponse CreateAgentCategory(CreateAgentCategory createAgentCategory)
        {
            if (agentCategoriesDB.Exists(x => x.IsDeleted == false && x.Name == createAgentCategory.Name))
            {
                return new CSSResponse($"Agent Category with name '{createAgentCategory.Name}' already exists.", HttpStatusCode.Conflict);
            }

            AgentCategory agentCategory = new AgentCategory()
            {
                Id = 4,
                Name = createAgentCategory.Name,
                CreatedBy = createAgentCategory.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            agentCategoriesDB.Add(agentCategory);

            return new CSSResponse(new AgentCategoryIdDetails { AgentCategoryId = agentCategory.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <param name="updateAgentCategory">The update agentCategory.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails, UpdateAgentCategory updateAgentCategory)
        {
            if (!agentCategoriesDB.Exists(x => x.IsDeleted == false && x.Id == agentCategoryIdDetails.AgentCategoryId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentCategoriesDB.Exists(x => x.IsDeleted == false && x.Name == updateAgentCategory.Name && x.Id != agentCategoryIdDetails.AgentCategoryId))
            {
                return new CSSResponse($"Agent Category with name '{updateAgentCategory.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentCategory = agentCategoriesDB.Where(x => x.Id == agentCategoryIdDetails.AgentCategoryId && x.IsDeleted == false).FirstOrDefault();
            agentCategory.ModifiedBy = updateAgentCategory.ModifiedBy;
            agentCategory.Name = updateAgentCategory.Name;
            agentCategory.ModifiedDate = DateTime.UtcNow;
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails)
        {
            if (!agentCategoriesDB.Exists(x => x.IsDeleted == false && x.Id == agentCategoryIdDetails.AgentCategoryId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentCategory = agentCategoriesDB.Where(x => x.Id == agentCategoryIdDetails.AgentCategoryId && x.IsDeleted == false).FirstOrDefault();

            agentCategoriesDB.Remove(agentCategory);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}