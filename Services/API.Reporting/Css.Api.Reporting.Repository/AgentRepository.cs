using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for agent collection
    /// </summary>
    public class AgentRepository : GenericRepository<Agent>, IAgentRepository
    {
        /// <summary>Initializes a new instance of the <see cref="AgentRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// A method to pull all existing agents in the collection
        /// </summary>
        /// <param name="ssns"></param>
        /// <returns></returns>
        public async Task<List<Agent>> GetAgents(List<string> ssns)
        {
            var agents = FilterBy(x => !x.IsDeleted && ssns.Contains(x.Ssn));
            return await Task.FromResult(agents.ToList());
        }

        /// <summary>
        /// A method to pull all existing agents in the input agent scheduling group
        /// </summary>
        /// <param name="agentSchedulingGroupId"></param>
        /// <returns></returns>
        public async Task<List<Agent>> GetAgents(int agentSchedulingGroupId)
        {
            var agents = FilterBy(x => !x.IsDeleted && x.AgentSchedulingGroupId == agentSchedulingGroupId);
            return await Task.FromResult(agents.ToList());
        }

        /// <summary>
        /// The method to upsert agents to the mongo collection
        /// </summary>
        /// <param name="agents"></param>
        public void Upsert(List<Agent> agents)
        {
            var bulkUpsert = new List<WriteModel<Agent>>();
            agents.ForEach(agent =>
            {
                var query = Builders<Agent>.Filter.Eq(i => i.Ssn, agent.Ssn);
                
                UpdateDefinition<Agent> update = Builders<Agent>.Update.Set(x => x.IsDeleted, agent.IsDeleted);

                update = update.SetOnInsert(x => x.CreatedDate, agent.CreatedDate);
                update = update.SetOnInsert(x => x.CreatedBy, agent.CreatedBy);
            
                update = update.Set(x => x.ModifiedDate, agent.ModifiedDate);
                update = update.Set(x => x.ModifiedBy, agent.ModifiedBy);

                update = SetBasicInformation(update, agent);
                update = SetMUInformation(update, agent);
                
                if (agent.HireDate != null)
                {
                    update = update.Set(x => x.HireDate, agent.HireDate);
                }

                if (agent.SenExt != null)
                {
                    update = update.Set(x => x.SenExt, agent.SenExt);
                }

                var updateAgent = new UpdateOneModel<Agent>(query, update) 
                { 
                    IsUpsert = true 
                };

                bulkUpsert.Add(updateAgent);
            });

            if(bulkUpsert.Any())
            {
                BulkWriteAsync(bulkUpsert);
            }
        }
        
        /// <summary>
        /// A helper method to set basic information for the agent
        /// </summary>
        /// <param name="update"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        private UpdateDefinition<Agent> SetBasicInformation(UpdateDefinition<Agent> update, Agent agent)
        {
            if (agent.AgentCategoryValues != null && agent.AgentCategoryValues.Any())
            {
                update = update.Set(x => x.AgentCategoryValues, agent.AgentCategoryValues);
            }

            if (!string.IsNullOrWhiteSpace(agent.Sso))
            {
                update = update.Set(x => x.Sso, agent.Sso);
            }

            if (!string.IsNullOrWhiteSpace(agent.FirstName))
            {
                update = update.Set(x => x.FirstName, agent.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(agent.LastName))
            {
                update = update.Set(x => x.LastName, agent.LastName);
            }

            if (!string.IsNullOrWhiteSpace(agent.AgentRole))
            {
                update = update.Set(x => x.AgentRole, agent.AgentRole);
            }

            if (!string.IsNullOrWhiteSpace(agent.SupervisorName))
            {
                update = update.Set(x => x.SupervisorName, agent.SupervisorName)
                            .Set(x => x.SupervisorId, agent.SupervisorId)
                            .Set(x => x.SupervisorSso, agent.SupervisorSso);
            }

            return update;
        }

        /// <summary>
        /// A helper method to set all MU information for the agent
        /// </summary>
        /// <param name="update"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        private UpdateDefinition<Agent> SetMUInformation(UpdateDefinition<Agent> update, Agent agent)
        {
            if (!string.IsNullOrWhiteSpace(agent.Mu))
            {
                update = update.Set(x => x.Mu, agent.Mu);
            }

            if (agent.AgentSchedulingGroupId > 0)
            {
                update = update.Set(x => x.AgentSchedulingGroupId, agent.AgentSchedulingGroupId);
            }

            if (agent.ClientId > 0)
            {
                update = update.Set(x => x.ClientId, agent.ClientId);
            }

            if (agent.ClientLobGroupId > 0)
            {
                update = update.Set(x => x.ClientLobGroupId, agent.ClientLobGroupId);
            }

            if (agent.SkillGroupId > 0)
            {
                update = update.Set(x => x.SkillGroupId, agent.SkillGroupId);
            }
            
            if (agent.SkillTagId > 0)
            {
                update = update.Set(x => x.SkillTagId, agent.SkillTagId);
            }

            return update;
        }
    }
}
