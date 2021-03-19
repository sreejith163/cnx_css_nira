using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The activity log repository
    /// </summary>
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {

        /// <summary>Initializes a new instance of the <see cref="AgentRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public ActivityLogRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// A method to insert activity logs
        /// </summary>
        /// <param name="activityLogRequest"></param>
        public void CreateActivityLogs(List<ActivityLog> activityLogRequest)
        { 
            if (activityLogRequest.Any())
            {
                InsertManyAsync(activityLogRequest);
            }
        }
    }
}
