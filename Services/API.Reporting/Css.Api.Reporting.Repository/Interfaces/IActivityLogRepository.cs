using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for the activity log repository
    /// </summary>
    public interface IActivityLogRepository
    {
        /// <summary>
        /// A method to insert activity logs
        /// </summary>
        /// <param name="activityLogRequest"></param>
        void CreateActivityLogs(List<ActivityLog> activityLogRequest);
    }
}
