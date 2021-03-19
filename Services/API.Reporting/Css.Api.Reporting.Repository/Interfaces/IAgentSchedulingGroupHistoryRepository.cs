using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent scheduling group history
    /// </summary>
    public interface IAgentSchedulingGroupHistoryRepository
    {

        /// <summary>
        /// The method to update the history for the input agents and their current scheduling group details
        /// </summary>
        /// <param name="agentSchedulingGroupHistories"></param>
        /// <returns></returns>
        void UpdateAgentSchedulingGroupHistory(List<AgentSchedulingGroupHistory> agentSchedulingGroupHistories);
    }
}
