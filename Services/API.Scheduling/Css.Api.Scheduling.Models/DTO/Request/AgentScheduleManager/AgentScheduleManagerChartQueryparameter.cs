using Css.Api.Core.Models.DTO.Request;
using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class AgentScheduleManagerChartQueryparameter: QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerChartQueryparameter"/> class.
        /// </summary>
        public AgentScheduleManagerChartQueryparameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int? AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [exclude conflict schedule].
        /// </summary>
        public bool ExcludeConflictSchedule { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        public DateTime? Date { get; set; }
    }
}