using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Reporting.Models.Domain;
using Css.Api.Reporting.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// The method to upsert agents to the mongo collection
        /// </summary>
        /// <param name="agents"></param>
        public void UpsertAsync(List<Agent> agents)
        {
            agents.ForEach(agent =>
            {
                var query = Builders<Agent>.Filter.Eq(i => i.Ssn, agent.Ssn);
                
                UpdateDefinition<Agent> update = Builders<Agent>.Update.Set(x => x.ModifiedDate, DateTime.UtcNow);
                update = update.Set(x => x.ModifiedBy, agent.ModifiedBy);

                update = update.SetOnInsert(x => x.CreatedDate, DateTime.UtcNow);
                update = update.SetOnInsert(x => x.CreatedBy, agent.CreatedBy);

                if (agent.AgentData != null && agent.AgentData.Any())
                {
                    update = update.Set(x => x.AgentData, agent.AgentData);
                }

                if (!string.IsNullOrWhiteSpace(agent.FirstName))
                {
                    update = update.Set(x => x.FirstName, agent.FirstName);
                }

                if (!string.IsNullOrWhiteSpace(agent.LastName))
                {
                    update = update.Set(x => x.LastName, agent.LastName);
                }

                if (!string.IsNullOrWhiteSpace(agent.Mu))
                {
                    update = update.Set(x => x.Mu, agent.Mu);
                }

                if (agent.SenDate != null)
                {
                    update = update.Set(x => x.SenDate, agent.SenDate);
                }

                if (agent.SenExt != null)
                {
                    update = update.Set(x => x.SenExt, agent.SenExt);
                }

                if (!string.IsNullOrWhiteSpace(agent.Sso))
                {
                    update = update.Set(x => x.Sso, agent.Sso);
                }


                UpdateOneAsync(query, update, new UpdateOptions
                {
                    IsUpsert = true
                });

            });


        }

        
    }
}
