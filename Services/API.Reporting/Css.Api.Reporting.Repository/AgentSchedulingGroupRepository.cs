using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for agent scheduling group collection
    /// </summary>
    public class AgentSchedulingGroupRepository : GenericRepository<AgentSchedulingGroup>, IAgentSchedulingGroupRepository
    {

        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentSchedulingGroupRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// Method to fetch all agent scheduling groups
        /// </summary>
        /// <returns>A list of instances of AgentSchedulingGroup</returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups()
        {
            var agents = FilterBy(x => !x.IsDeleted);
            return await Task.FromResult(agents.ToList());
        }
    }
}
