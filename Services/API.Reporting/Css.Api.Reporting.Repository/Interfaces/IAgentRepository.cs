using Css.Api.Reporting.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent repository
    /// </summary>
    public interface IAgentRepository
    {
        /// <summary>
        /// A method to upsert all input agents to the collection
        /// </summary>
        /// <param name="agents"></param>
        void UpsertAsync(List<Agent> agents);
    }
}
