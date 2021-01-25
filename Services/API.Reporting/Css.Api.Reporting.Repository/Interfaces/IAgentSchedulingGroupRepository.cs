using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent scheduling group collection repository
    /// </summary>
    public interface IAgentSchedulingGroupRepository
    {
        /// <summary>
        /// Method to fetch all agent scheduling groups
        /// </summary>
        /// <returns>A list of instances of AgentSchedulingGroup</returns>
        Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups();
    }
}
