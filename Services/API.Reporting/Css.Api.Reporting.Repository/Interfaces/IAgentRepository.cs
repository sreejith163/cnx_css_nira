using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent repository
    /// </summary>
    public interface IAgentRepository
    {
        /// <summary>
        /// A method to pull all existing agents in the collection
        /// </summary>
        /// <param name="ssns"></param>
        /// <returns></returns>
        Task<List<Agent>> GetAgents(List<int> ssns);

        /// <summary>
        /// A method to upsert all input agents to the collection
        /// </summary>
        /// <param name="agents"></param>
        void Upsert(List<Agent> agents);
    }
}
